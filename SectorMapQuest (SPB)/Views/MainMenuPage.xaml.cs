using Microsoft.Maui.Controls;

namespace SectorMapQuest__SPB_.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    void OnMapTapped(object sender, TappedEventArgs e)
    {
        Activate(MapText);
        PageHost.Content = new MapPage().Content;
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