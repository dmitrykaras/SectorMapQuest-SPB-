using SectorMapQuest.Graphics;
using SectorMapQuest.Managers;

namespace SectorMapQuest.Views.Map;

public partial class MapView : ContentView
{
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;

    private readonly HexMapDrawable _drawable;
    private bool _centered; // флаг, чтобы центрировать карту только один раз

    public MapView(
        MapManager mapManager,
        ProgressManager progressManager,
        PlayerPositionManager playerPosition)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPosition = playerPosition;

        //генерация карты радиусом 3 гекса от центра
        _mapManager.Generate(3);

        //создание и настройка отрисовщика карты
        _drawable = new HexMapDrawable(_mapManager);
        MapCanvas.Drawable = _drawable;

        MapCanvas.SizeChanged += OnSizeChanged; //для центрирования карты
        MapCanvas.StartInteraction += OnTouch; //для обработки касаний
    }

    //обработчик изменения размера холста
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (_centered || MapCanvas.Width <= 0)
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
        _drawable.Offset = new PointF(
            screenCenter.X - mapCenter.X,
            screenCenter.Y - mapCenter.Y
        );

        //запрашиваем перерисовку
        MapCanvas.Invalidate();
    }

    //обработчик касания карты
    private void OnTouch(object sender, TouchEventArgs e)
    {
        //временная логкика...
        _playerPosition.MoveTo(0, 0);

        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapCanvas.Invalidate();
    }
}