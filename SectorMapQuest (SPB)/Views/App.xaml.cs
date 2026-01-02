using SectorMapQuest.Views;
using SectorMapQuest.Managers;


namespace SectorMapQuest__SPB_;

public partial class App : Application
{
    public MapManager MapManager { get; }
    public ProgressManager ProgressManager { get; }
    public PlayerPositionManager PlayerPositionManager { get; }

    public App()
    {
        InitializeComponent();

        MapManager = new MapManager();
        ProgressManager = new ProgressManager();
        PlayerPositionManager = new PlayerPositionManager();

        MainPage = new MainPage(
            MapManager,
            ProgressManager,
            PlayerPositionManager
        );
    }
}
