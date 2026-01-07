public class CameraManager
{
    //масштаб камеры (зум)
    public float Scale { get; set; } = 1f;

    //смещение камеры относительно мировых координат
    public PointF Offset { get; private set; } = new(0, 0);

    //установка скейла для отдаления камеры
    public void SetScale(float scale)
    {
        Scale = scale;
    }

    //устанавливаем новое смещение
    public void SetOffset(float x, float y)
    {
        Offset = new PointF(x, y);
    }


    //перемещение (свдиг) камеры по осям
    public void Translate(float dx, float dy)
    {
        Offset = new PointF(Offset.X + dx, Offset.Y + dy);
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
            (screen.X - Offset.X) / Scale, //убираем смещение и делим на масштаб
            (screen.Y - Offset.Y) / Scale
        );
    }
}