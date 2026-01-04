namespace SectorMapQuest.Managers;

public class ScoreManager
{
    //текущий счёт
    public int Score { get; private set; }

    //добавляет очки к текущему счету
    public void AddScore(int value)
    {
        Score += value;
    }
}