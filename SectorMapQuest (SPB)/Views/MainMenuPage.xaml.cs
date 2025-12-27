namespace SectorMapQuest__SPB_.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private async void OnOpenMapClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage());
    }
}