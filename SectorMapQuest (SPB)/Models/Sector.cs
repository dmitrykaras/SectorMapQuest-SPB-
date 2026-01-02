public class Sector
{
    public int Id { get; set; }

    // координаты логической сетки
    public int Q { get; set; }
    public int R { get; set; }

    public bool IsOpened { get; set; }

    public List<PointOfInterest> Points { get; set; } = new();
}