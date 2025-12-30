namespace SectorMapQuest__SPB_.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage());
    }

    private async void OnStatsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatsPage());
    }

    private async void OnFeedClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FeedPage());
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Settings clicked", "OK");
    }
}
