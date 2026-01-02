public class CameraManager
{
    public float Scale { get; set; } = 1f;

    public PointF Offset { get; private set; } = new(0, 0);

    public void CenterOn(PointF worldPoint, SizeF viewport)
    {
        Offset = new PointF(
            viewport.Width / 2f - worldPoint.X * Scale,
            viewport.Height / 2f - worldPoint.Y * Scale
        );
    }

    public PointF WorldToScreen(PointF world)
    {
        return new PointF(
            world.X * Scale + Offset.X,
            world.Y * Scale + Offset.Y
        );
    }

    public PointF ScreenToWorld(PointF screen)
    {
        return new PointF(
            (screen.X - Offset.X) / Scale,
            (screen.Y - Offset.Y) / Scale
        );
    }
}