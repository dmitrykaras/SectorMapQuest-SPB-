using SectorMapQuest__SPB_.Views;

namespace SectorMapQuest__SPB_;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new MainMenuPage());
    }
}