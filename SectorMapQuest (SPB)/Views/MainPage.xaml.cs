using SectorMapQuest.Views.Map;
using SectorMapQuest.Managers;
using SectorMapQuest__SPB_.Views;

namespace SectorMapQuest.Views;

public partial class MainPage : ContentPage
{
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPositionManager;

    public MainPage(MapManager mapManager,
        ProgressManager progressManager,
        PlayerPositionManager playerPositionManager)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPositionManager = playerPositionManager;

        ShowMap();
    }

    void OnStartClicked(object sender, TappedEventArgs e)
    {
        ShowMap();
    }

    void OnFeedTapped(object sender, TappedEventArgs e)
    {
        PageHost.Content = new Label { Text = "Feed", TextColor = Colors.White };
    }

    void OnStatsTapped(object sender, TappedEventArgs e)
    {
        PageHost.Content = new Label { Text = "Stats", TextColor = Colors.White };
    }

    void OnProfileTapped(object sender, TappedEventArgs e)
    {
        PageHost.Content = new Label { Text = "Profile", TextColor = Colors.White };
    }

    private void ShowMap()
    {
        PageHost.Content = new MapView(
            _mapManager,
            _progressManager,
            _playerPositionManager
        );
    }

    private void OnSettingsClicked(object sender, TappedEventArgs e)
    {
        PageHost.Content = new SettingsPage().Content;
    }
}