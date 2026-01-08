public class CameraManager
{
    //перерисовка для центрирования
    public event Action RequestInvalidate;

    //масштаб камеры (зум)
    public float Scale { get; private set; } = 1f;
    public const float MinScale = 0.5f;
    public const float MaxScale = 3.0f;

    //смещение камеры относительно мировых координат
    public PointF Offset { get; private set; } = new(0, 0);

    //целевое смещение (для плавности)
    public PointF TargetOffset { get; private set; }

    //плавная анимация перемещения
    public void Update(float smoothing = 0.25f)
    {
        float dx = TargetOffset.X - Offset.X;
        float dy = TargetOffset.Y - Offset.Y;

        // Если разница больше пикселя, продолжаем движение
        if (Math.Abs(dx) > 0.5f || Math.Abs(dy) > 0.5f)
        {
            Offset = new PointF(
                Offset.X + dx * smoothing,
                Offset.Y + dy * smoothing
            );

            RequestInvalidate?.Invoke();
        }
        else
        {
            // Принудительно ставим в финальную точку для идеальной точности
            Offset = TargetOffset;
        }
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

    //рассчитываем смещение так, чтобы точка игрока оказалась в центре экрана
    public void CenterOn(PointF worldPosition, double screenWidth, double screenHeight)
    {
        // Расчет строгого центра: Offset = Экран/2 - (Мир * Зум)
        float targetX = (float)(screenWidth / 2f - (worldPosition.X * Scale));
        float targetY = (float)(screenHeight / 2f - (worldPosition.Y * Scale));

        TargetOffset = new PointF(targetX, targetY);

        // Даем первый толчок для начала цикла Invalidate
        RequestInvalidate?.Invoke();
    }
}