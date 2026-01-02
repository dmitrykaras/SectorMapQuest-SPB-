namespace SectorMapQuest.Managers;

public class PlayerPositionManager
{
    public int Q { get; private set; }
    public int R { get; private set; }

    public void MoveTo(int q, int r)
    {
        Q = q;
        R = r;
    }
}