namespace SectorMapQuest.Managers;

public class PlayerPositionManager
{
    public PointF Position { get; private set; }

    public event Action<PointF>? PositionChanged;

    public PlayerPositionManager()
    {
        Position = new PointF(0, 0);
    }

    //перемещение игрока в указанные координаты
    public void MoveTo(PointF newPosition)
    {
        Position = newPosition;
        PositionChanged?.Invoke(Position);
    }
}