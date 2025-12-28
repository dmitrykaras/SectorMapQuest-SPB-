namespace SectorMapQuest__SPB_.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private async void OnMapClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Map clicked", "OK");
    }

    private async void OnStatsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Statistics clicked", "OK");
    }

    private async void OnFeedClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Feed clicked", "OK");
    }

    private async void OnProfileClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Profile clicked", "OK");
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await DisplayAlert("OK", "Settings clicked", "OK");
    }
}
