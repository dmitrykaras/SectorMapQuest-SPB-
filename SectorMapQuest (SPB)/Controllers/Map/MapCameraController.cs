namespace SectorMapQuest.Controller;

public class MapCameraController
{
    public bool IsDraggingPlayer { get; private set; }


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
            _startScale = _camera.Scale;
            _startOffset = _camera.Offset;
        }

        //во время жеста вычисляем новый масштаб
        if (e.Status == GestureStatus.Running)
        {
            var newScale = Math.Clamp(
                _startScale * (float)e.Scale,
                MinScale,
                MaxScale
            );

            _camera.SetScale(newScale);
            _invalidate();
        }
    }

    //обработка жеста "pan" (перетаскивание одним пальцем)
    public void OnPanUpdated(PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Started)
            _startOffset = _camera.Offset;

        //реакция только во время движения пальца
        if (e.StatusType == GestureStatus.Running)
        {
            //сдвигаем камеру на величину смещения жеста
            _camera.Translate(
                (float)e.TotalX,
                (float)e.TotalY
            );

            _invalidate();
        }
    }
}