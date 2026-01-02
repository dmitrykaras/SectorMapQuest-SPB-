namespace SectorMapQuest.Managers;

public class MapManager
{
    public List<Sector> Sectors { get; } = new();

    public void Generate(int radius)
    {
        Sectors.Clear();

        int id = 0;

        for (int q = -radius; q <= radius; q++)
        {
            for (int r = -radius; r <= radius; r++)
            {
                if (Math.Abs(q + r) > radius)
                    continue;

                Sectors.Add(new Sector
                {
                    Id = id++,
                    Q = q,
                    R = r,
                    IsOpened = false
                });
            }
        }
    }

    public Sector GetAt(int q, int r)
        => Sectors.FirstOrDefault(s => s.Q == q && s.R == r);
}