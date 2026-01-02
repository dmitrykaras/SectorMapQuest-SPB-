using SectorMapQuest.Graphics;
using SectorMapQuest.Managers;

namespace SectorMapQuest.Views.Map;

public partial class MapView : ContentView
{
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;

    private readonly HexMapDrawable _drawable;
    private bool _centered;

    public MapView(
        MapManager mapManager,
        ProgressManager progressManager,
        PlayerPositionManager playerPosition)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPosition = playerPosition;

        _mapManager.Generate(3);

        _drawable = new HexMapDrawable(_mapManager);
        MapCanvas.Drawable = _drawable;

        MapCanvas.SizeChanged += OnSizeChanged;
        MapCanvas.StartInteraction += OnTouch;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (_centered || MapCanvas.Width <= 0)
            return;

        CenterMap();
        _centered = true;
    }

    private void CenterMap()
    {
        var bounds = _mapManager.GetWorldBounds(_drawable.HexSize);

        var screenCenter = new PointF(
            (float)(MapCanvas.Width / 2),
            (float)(MapCanvas.Height / 2));

        var mapCenter = new PointF(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        _drawable.Scale = 1f;
        _drawable.Offset = new PointF(
            screenCenter.X - mapCenter.X,
            screenCenter.Y - mapCenter.Y
        );

        MapCanvas.Invalidate();
    }

    private void OnTouch(object sender, TouchEventArgs e)
    {
        _playerPosition.MoveTo(0, 0);

        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapCanvas.Invalidate();
    }
}