public class CameraManager
{
    //масштаб камеры (зум)
    public float Scale { get; private set; } = 1f;
    public const float MinScale = 0.5f;
    public const float MaxScale = 3.0f;

    //смещение камеры относительно мировых координат
    public PointF Offset { get; private set; } = new(0, 0);

    //целевое смещение (для плавности)
    public PointF TargetOffset { get; private set; }

    //обновление Offset'а 
    public void Update(float smoothing = 0.25f)
    {
        Offset = new PointF(
            Offset.X + (TargetOffset.X - Offset.X) * smoothing,
            Offset.Y + (TargetOffset.Y - Offset.Y) * smoothing
        );
    }

    public CameraManager()
    {
        Offset = new PointF(0, 0);
        TargetOffset = Offset; // ⬅️ ВОТ ЭТО МЕСТО
    }

    //установка скейла для отдаления камеры
    public void SetScale(float scale)
    {
        Scale = Math.Clamp(scale, MinScale, MaxScale);
    }

    //устанавливаем новое смещение
    public void SetOffset(PointF offset)
    {
        Offset = offset;
    }


    //перемещение (свдиг) камеры по осям
    public void Translate(float dx, float dy)
    {
        TargetOffset = new PointF(
            TargetOffset.X + dx,
            TargetOffset.Y + dy
        );
    }


    //центрирует камеру на указанной точке в мировых координатах
    public void CenterOn(PointF worldPoint, SizeF viewport)
    {
        //вычисляем смещение так, чтобы worldPoint оказался в центре viewport
        Offset = new PointF(
            viewport.Width / 2f - worldPoint.X * Scale, //по X
            viewport.Height / 2f - worldPoint.Y * Scale //по Y
        );
    }

    //преобразует мировые координаты в экранные
    public PointF WorldToScreen(PointF world)
    {
        return new PointF(
            world.X * Scale + Offset.X, //применяем массштаб и смещение
            world.Y * Scale + Offset.Y
        );
    }

    //преобразует экранные координаты в мировые
    public PointF ScreenToWorld(PointF screen)
    {
        return new PointF(
            (screen.X - Offset.X) / Scale,
            (screen.Y - Offset.Y) / Scale
        );
    }
}