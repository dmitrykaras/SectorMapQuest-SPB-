using SectorMapQuest.Graphics;
using SectorMapQuest.Managers;

namespace SectorMapQuest.Views.Map;

public partial class MapView : ContentView
{
    //logic managers
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;

    //отрисовка
    private readonly PlayerDrawable _playerDrawable;
    private readonly HexMapDrawable _drawable;

    //состояния
    private bool _centered; // флаг: карта уже была центрирована
    private bool _draggingPlayer; // флаг: сейчас перетаскиваем игрока

    public MapView(
        MapManager mapManager,
        ProgressManager progressManager,
        PlayerPositionManager playerPosition)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPosition = playerPosition;

        //создаём drawable игрока
        _playerDrawable = new PlayerDrawable(_playerPosition);

        //генерация карты радиусом 3 гекса от центра
        _mapManager.Generate(3);

        //создание и настройка отрисовщика карты
        _drawable = new HexMapDrawable(_mapManager, _playerDrawable);
        MapCanvas.Drawable = _drawable;

        MapCanvas.SizeChanged += OnSizeChanged; //для центрирования карты
        //для обработки касаний
        MapCanvas.StartInteraction += OnTouchStart;
        MapCanvas.DragInteraction += OnTouchMove;
        MapCanvas.EndInteraction += OnTouchEnd;

        //подписка на событие открытия сектора
        _progressManager.SectorOpened += OnSectorOpened;
    }

    //обработчик изменения размера холста
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (_centered || MapCanvas.Width <= 0 || MapCanvas.Height <= 0)
            return;

        CenterMap();
        _centered = true;
    }

    //центрирует карту на экране
    private void CenterMap()
    {
        //получаем границы карты в пикселях
        var bounds = _mapManager.GetWorldBounds(_drawable.HexSize);

        //центр экрана
        var screenCenter = new PointF(
            (float)(MapCanvas.Width / 2),
            (float)(MapCanvas.Height / 2));

        //центр карты в мировых координатах
        var mapCenter = new PointF(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        //сбрасываем масштаб и вычисляем смещение для центрирования
        _drawable.Scale = 1f;

        //вычисляем смещение, чтобы центр карты совпал с центром экрана
        _drawable.Offset = new PointF(
            screenCenter.X - mapCenter.X,
            screenCenter.Y - mapCenter.Y
        );

        //запрашиваем перерисовку
        MapCanvas.Invalidate();
    }

    //обработчик касания карты
    private void OnTouchStart(object sender, TouchEventArgs e)
    {
        var worldPoint = ScreenToWorld(e.Touches[0]);

        // проверяем, попали ли по игроку
        if (Distance(worldPoint, _playerPosition.Position) < 15)
            _draggingPlayer = true;
    }

    private void OnTouchMove(object sender, TouchEventArgs e)
    {
        if (!_draggingPlayer)
            return;

        var worldPoint = ScreenToWorld(e.Touches[0]);

        _playerPosition.MoveTo(worldPoint);

        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapCanvas.Invalidate();
    }

    private void OnTouchEnd(object sender, TouchEventArgs e)
    {
        _draggingPlayer = false;
    }

    private PointF ScreenToWorld(PointF screen)
    {
        return new PointF(
            (screen.X - _drawable.Offset.X) / _drawable.Scale,
            (screen.Y - _drawable.Offset.Y) / _drawable.Scale
        );
    }

    private static float Distance(PointF a, PointF b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    //вызывается при открытии нового сектора.
    private void OnSectorOpened(Sector sector)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SectorPopup.IsVisible = true;
        });
    }

    //закрывает overlay-окно "Новый сектор".
    private void OnPopupClosed(object sender, EventArgs e)
    {
        SectorPopup.IsVisible = false;
    }
}