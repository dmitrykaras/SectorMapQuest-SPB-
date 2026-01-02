namespace SectorMapQuest.Managers;

public class ScoreManager
{
    public int Score { get; private set; }

    public void AddScore(int value)
    {
        Score += value;
    }
}