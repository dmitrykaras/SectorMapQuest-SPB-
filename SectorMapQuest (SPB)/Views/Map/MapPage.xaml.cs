using SectorMapQuest.Graphics;
using SectorMapQuest.Managers;
using SectorMapQuest.Controller;

namespace SectorMapQuest.Views.Map;

public enum MapInputMode
{
    Camera,   // pan + pinch двигают карту
    Player    // касание двигает игрока
}


public partial class MapView : ContentView
{
    //logic managers
    private readonly MapManager _mapManager;
    private readonly ProgressManager _progressManager;
    private readonly PlayerPositionManager _playerPosition;

    private MapInputMode _inputMode = MapInputMode.Camera; // по умолчанию карта

    private PanGestureRecognizer _panGesture;
    private PinchGestureRecognizer _pinchGesture;

    //отрисовка
    private readonly PlayerDrawable _playerDrawable;
    private readonly HexMapDrawable _drawable;

    //камера и управление ей
    private CameraManager _camera;
    private MapCameraController _cameraController;

    //состояния
    private bool _centered; // флаг: карта уже была центрирована
    private bool _draggingPlayer; // флаг: сейчас перетаскиваем игрока

    public MapView(
        MapManager mapManager,
        ProgressManager progressManager,
        PlayerPositionManager playerPosition)
    {
        InitializeComponent();

        _mapManager = mapManager;
        _progressManager = progressManager;
        _playerPosition = playerPosition;

        //создаём drawable игрока
        _playerDrawable = new PlayerDrawable(_playerPosition);

        //генерация карты радиусом 3 гекса от центра
        _mapManager.Generate(3);

        _camera = new CameraManager();

        //создание и настройка отрисовщика карты
        _drawable = new HexMapDrawable(_mapManager, _playerDrawable, _camera);
        MapCanvas.Drawable = _drawable;

        MapCanvas.SizeChanged += OnSizeChanged; //для центрирования карты
        //для обработки касаний
        MapCanvas.StartInteraction += OnTouchStart;
        MapCanvas.DragInteraction += OnTouchMove;
        MapCanvas.EndInteraction += OnTouchEnd;

        //подписка на событие открытия сектора
        _progressManager.SectorOpened += OnSectorOpened;

        
        _cameraController = new MapCameraController(() => MapCanvas.Invalidate(), _camera);

        _panGesture = new PanGestureRecognizer();
        _panGesture.PanUpdated += OnPanUpdated;

        _pinchGesture = new PinchGestureRecognizer();
        _pinchGesture.PinchUpdated += OnPinchUpdated;

        // Изначально включаем только потому, что стартовый режим CAMERA
        MapCanvas.GestureRecognizers.Add(_panGesture);
        MapCanvas.GestureRecognizers.Add(_pinchGesture);
    }

    //отладка: если режим камеры, то перетаскивание работает
    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (_inputMode == MapInputMode.Camera)
            _cameraController.OnPanUpdated(e);
    }

    //отладка: если режим камера, то масштабирование работает
    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (_inputMode == MapInputMode.Camera)
            _cameraController.OnPinchUpdated(e);
    }


    //обработчик изменения размера холста
    private void OnSizeChanged(object? sender, EventArgs e)
    {
        if (_centered || MapCanvas.Width <= 0 || MapCanvas.Height <= 0)
            return;

        CenterMap();
        _centered = true;
    }

    //центрирует карту на экране
    private void CenterMap()
    {
        //получаем границы карты в пикселях
        var bounds = _mapManager.GetWorldBounds(_drawable.HexSize);

        //центр экрана
        var screenCenter = new PointF(
            (float)(MapCanvas.Width / 2),
            (float)(MapCanvas.Height / 2));

        //центр карты в мировых координатах
        var mapCenter = new PointF(
            bounds.X + bounds.Width / 2,
            bounds.Y + bounds.Height / 2);

        //запрашиваем перерисовку
        MapCanvas.Invalidate();
    }

    //обработчик касания карты
    private void OnTouchStart(object sender, TouchEventArgs e)
    {
        if (_inputMode != MapInputMode.Player || e.Touches.Length == 0)
            return;

        var screen = e.Touches[0];
        var world = _camera.ScreenToWorld(screen);

        //проверяем, попали ли по игроку
        if (Distance(world, _playerPosition.Position) < 15)
            _draggingPlayer = true;
    }

    //перетаскиывание player
    private void OnTouchMove(object sender, TouchEventArgs e)
    {
        if (_inputMode != MapInputMode.Player || !_draggingPlayer || e.Touches.Length == 0)
            return;

        var world = _camera.ScreenToWorld(e.Touches[0]);

        _playerPosition.MoveTo(world);

        _progressManager.TryOpenSectorAtPlayerPosition(
            _playerPosition,
            _mapManager
        );

        MapCanvas.Invalidate();
    }

    //заканчиваем перетаскивание
    private void OnTouchEnd(object sender, TouchEventArgs e)
    {
        _draggingPlayer = false;
    }

    //перевод из экранных координат в мировые
    private PointF ScreenToWorld(PointF screen)
    {
        return _camera.ScreenToWorld(screen);
    }

    //расстояние между двумя точками
    private static float Distance(PointF a, PointF b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    //вызывается при открытии нового сектора.
    private void OnSectorOpened(Sector sector)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SectorPopup.IsVisible = true;
        });
    }

    //закрывает overlay-окно "Новый сектор".
    private void OnPopupClosed(object sender, EventArgs e)
    {
        SectorPopup.IsVisible = false;
    }

    //обработчик для кнопки дебага
    private void OnDebugModeClicked(object sender, EventArgs e)
    {
        _inputMode = _inputMode == MapInputMode.Camera
            ? MapInputMode.Player
            : MapInputMode.Camera;

        //сначала убираем любые жесты
        MapCanvas.GestureRecognizers.Remove(_panGesture);
        MapCanvas.GestureRecognizers.Remove(_pinchGesture);

        //если теперь режим Camera — добавляем их обратно
        if (_inputMode == MapInputMode.Camera)
        {
            MapCanvas.GestureRecognizers.Add(_panGesture);
            MapCanvas.GestureRecognizers.Add(_pinchGesture);
        }

        DebugModeButton.Text = _inputMode == MapInputMode.Camera
            ? "MODE: CAMERA"
            : "MODE: PLAYER";
    }



}