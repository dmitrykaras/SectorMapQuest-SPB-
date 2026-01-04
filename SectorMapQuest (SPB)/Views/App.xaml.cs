using SectorMapQuest.Views;
using SectorMapQuest.Managers;


namespace SectorMapQuest__SPB_;

public partial class App : Application
{
    public MapManager MapManager { get; } //сервис отвечающий за управление картой
    public ProgressManager ProgressManager { get; } //сервис отвечающий за прогресс игрока
    public PlayerPositionManager PlayerPositionManager { get; } //сервис отвечающий за позицию игрока

    public App()
    {
        InitializeComponent();

        //инициализация менеджеров
        MapManager = new MapManager();
        ProgressManager = new ProgressManager();
        PlayerPositionManager = new PlayerPositionManager();

        //создание главной страницы с передачей зависимостей
        MainPage = new MainPage(
            MapManager,
            ProgressManager,
            PlayerPositionManager
        );
    }
}
