using SectorMapQuest.Graphics;
using SectorMapQuest.Managers;
using SectorMapQuest.Controller;

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

    //камера и управление ей
    private CameraManager _camera;
    private MapCameraController _cameraController;

    //состо€ни€
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

        //создаЄм drawable игрока
        _playerDrawable = new PlayerDrawable(_playerPosition);

        //генераци€ карты радиусом 3 гекса от центра
        _mapManager.Generate(3);

        _camera = new CameraManager();

        //создание и настройка отрисовщика карты
        _drawable = new HexMapDrawable(_mapManager, _playerDrawable, _camera);
        MapCanvas.Drawable = _drawable;

        MapCanvas.SizeChanged += OnSizeChanged; //дл€ центрировани€ карты
        //дл€ обработки касаний
        MapCanvas.StartInteraction += OnTouchStart;
        MapCanvas.DragInteraction += OnTouchMove;
        MapCanvas.EndInteraction += OnTouchEnd;

        //подписка на событие открыти€ сектора
        _progressManager.SectorOpened += OnSectorOpened;

        
        _cameraController = new MapCameraController(() => MapCanvas.Invalidate(), _camera);

        var pinch = new PinchGestureRecognizer();
        pinch.PinchUpdated += (s, e) => _cameraController.OnPinchUpdated(e);

        var pan = new PanGestureRecognizer();
        pan.PanUpdated += (s, e) => _cameraController.OnPanUpdated(e);

        MapCanvas.GestureRecognizers.Add(pinch);
        MapCanvas.GestureRecognizers.Add(pan);
    }

    //обработчик изменени€ размера холста
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
        //получаем границы карты в пиксел€х
        var bounds = _mapManager.GetWorldBounds(_drawable.HexSize);

        //центр экрана
        var screenCenter = new PointF(
            (float)(MapCanvas.Width / 2),
            (float)(MapCanvas.Height / 2));

        //центр карты в мировых координатах
        var mapCenter = new PointF(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        //запрашиваем перерисовку
        MapCanvas.Invalidate();
    }

    //обработчик касани€ карты
    private void OnTouchStart(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 0)
            return;

        var screen = e.Touches[0];
        var world = _camera.ScreenToWorld(screen);

        // провер€ем, попали ли по игроку
        if (Distance(world, _playerPosition.Position) < 15)
            _draggingPlayer = true;
    }

    //перетаскиывание player
    private void OnTouchMove(object sender, TouchEventArgs e)
    {
        if (!_draggingPlayer || e.Touches.Length == 0)
            return;

        var world = _camera.ScreenToWorld(e.Touches[0]);

        _playerPosition.MoveTo(world);

        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapCanvas.Invalidate();
    }

    //заканчиваем перетаскивание
    private void OnTouchEnd(object sender, TouchEventArgs e)
    {
        _draggingPlayer = false;
    }

    //перевод из экранных координат в мировые
    private PointF ScreenToWorld(PointF screen)
    {
        return _camera.ScreenToWorld(screen);
    }

    //рассто€ние между двум€ точками
    private static float Distance(PointF a, PointF b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    //вызываетс€ при открытии нового сектора.
    private void OnSectorOpened(Sector sector)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SectorPopup.IsVisible = true;
        });
    }

    //закрывает overlay-окно "Ќовый сектор".
    private void OnPopupClosed(object sender, EventArgs e)
    {
        SectorPopup.IsVisible = false;
    }
}