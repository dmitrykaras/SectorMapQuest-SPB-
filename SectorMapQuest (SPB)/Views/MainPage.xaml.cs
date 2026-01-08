using SectorMapQuest.Views.Map;
using SectorMapQuest.Managers;
using SectorMapQuest.Views.Settings;

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

        ShowMap(); //при старте показываем карту
    }

    //обработчики события нажатий на кпоки в нижней панели
    void OnMapClicked(object sender, TappedEventArgs e)
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

    //нажатие на кнопку настроек
    private void OnSettingsClicked(object sender, TappedEventArgs e)
    {
        var settings = new SettingsView();
        settings.BackRequested += ShowMap;

        PageHost.Content = settings;

        BottomBar.IsVisible = false; //скрываем нижнюю панель
        BackButton.IsVisible = true;
    }

    //показывает карту в основном контейнере страницы
    private void ShowMap()
    {
        PageHost.Content = new MapView(
            _mapManager,
            _progressManager,
            _playerPositionManager
        );

        BottomBar.IsVisible = true; //возвращаем нижнюю панель
    }

    //закрыть всплывающее окно
    private void OnPopupClosed(object sender, EventArgs e) { SectorPopup.IsVisible = false; }

    //нажатие на стрелку назад
    private void OnBackClicked(object sender, TappedEventArgs e)
    {
        BackButton.IsVisible = false;
        BottomBar.IsVisible = true;
        ShowMap();
    }

    //обработчик нажатия
    private void OnCenterOnPlayerClicked(object sender, EventArgs e)
    {
        if (PageHost.Content is MapView mapView)
        {
            var pos = _playerPositionManager.Position;
            _mapManager.Camera.CenterOn(pos, PageHost.Width, PageHost.Height);
        }
    }
}