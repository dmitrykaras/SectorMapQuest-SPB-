namespace SectorMapQuest.Managers;

public class PlayerPositionManager
{
    public int Q { get; private set; }
    public int R { get; private set; }

    //перемещает игрока в указанные координаты
    //в будущем можно добавить проверки на валидность координат....
    public void MoveTo(int q, int r)
    {
        Q = q;
        R = r;
    }
}