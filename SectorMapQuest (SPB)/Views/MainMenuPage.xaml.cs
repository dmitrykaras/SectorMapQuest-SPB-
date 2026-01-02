using Microsoft.Maui.Controls;
using SectorMapQuest.Managers;
using SectorMapQuest.Views;

namespace SectorMapQuest__SPB_.Views;

public partial class MainMenuPage : ContentPage
{
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;

    public MainMenuPage(
            MapManager mapManager,
            ProgressManager progressManager,
            PlayerPositionManager playerPosition)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPosition = playerPosition;
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new MapPage(
                _mapManager,
                _progressManager,
                _playerPosition
            )
        );
    }

    void OnFeedTapped(object sender, TappedEventArgs e)
    {
        Activate(FeedText);
        PageHost.Content = new FeedPage().Content;
    }

    void OnStatsTapped(object sender, TappedEventArgs e)
    {
        Activate(StatsText);
        PageHost.Content = new StatsPage().Content;
    }

    void OnProfileTapped(object sender, TappedEventArgs e)
    {
        Activate(ProfileText);
        PageHost.Content = new ProfilePage().Content;
    }

    void Activate(Label active)
    {
        MapText.TextColor = Colors.Gray;
        FeedText.TextColor = Colors.Gray;
        StatsText.TextColor = Colors.Gray;
        ProfileText.TextColor = Colors.Gray;

        active.TextColor = Colors.White;
    }

    private void OnSettingsClicked(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }
}