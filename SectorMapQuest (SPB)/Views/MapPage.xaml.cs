using SectorMapQuest.Managers;
using SectorMapQuest.Graphics;

namespace SectorMapQuest.Views;

public partial class MapPage : ContentPage
{
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;
    private readonly HexMapDrawable _drawable;

    public MapPage(
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
        MapView.Drawable = _drawable;

        MapView.StartInteraction += OnStartInteraction;
    }

    private void OnStartInteraction(object sender, TouchEventArgs e)
    {
        var point = e.Touches[0];

        HandleTap(point);
    }

    private void HandleTap(PointF position)
    {
        // позже: определим какой шестиугольник нажат
        // пока Ч просто откроем центральный сектор
        _playerPosition.MoveTo(0, 0);
        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapView.Invalidate();
    }
}