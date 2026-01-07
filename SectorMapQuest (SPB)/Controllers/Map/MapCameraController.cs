namespace SectorMapQuest.Controller;

public class MapCameraController
{
    public bool IsDraggingPlayer { get; private set; }

    //переменные для pan
    private double _lastPanX;
    private double _lastPanY;

    //переменная для pinch
    private double _lastPinchScale = 1.0;

    //карта к которой применяется трансформация и камера хранящая текущее состояние параметров смещения
    private readonly Action _invalidate;
    private readonly CameraManager _camera;

    //начальные масштаб и смещение в момент начала жеста Pinch 
    private float _startScale;
    private PointF _startOffset;

    //мин/макс допустимый масштабы
    private const float MinScale = 0.5f;
    private const float MaxScale = 3.0f;

    //конструктор контроллера камеры карты
    public MapCameraController (Action invalidate, CameraManager camera)
    {
        _invalidate = invalidate;
        _camera = camera;
    }

    //обработка жеста "pinch" (масштабирование двумя пальцами)
    public void OnPinchUpdated(PinchGestureUpdatedEventArgs e)
    {
        //начало жеста — запоминаем исходное состояние камеры
        if (e.Status == GestureStatus.Started)
        {
            _lastPinchScale = 1.0;
            return;
        }

        //во время жеста вычисляем новый масштаб
        if (e.Status != GestureStatus.Running)
            return;

        var delta = e.Scale / _lastPinchScale;
        _lastPinchScale = e.Scale;

        _camera.SetScale((float)delta);
        _invalidate();
    }

    //обработка жеста "pan" (перетаскивание одним пальцем)
    public void OnPanUpdated(PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
        {
            _lastPanX = e.TotalX;
            _lastPanY = e.TotalY;
            return;
        }

        if (e.StatusType == GestureStatus.Running)
        {
            var dx = e.TotalX - _lastPanX;
            var dy = e.TotalY - _lastPanY;

            _lastPanX = e.TotalX;
            _lastPanY = e.TotalY;

            _camera.Translate((float)dx, (float)dy);
            _invalidate();
        }
    }

}