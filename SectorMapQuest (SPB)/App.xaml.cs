namespace SectorMapQuest__SPB_;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new NavigationPage(new Views.MainMenuPage()));
    }
}