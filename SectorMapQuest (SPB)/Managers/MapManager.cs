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

    public RectF GetWorldBounds(float hexSize)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (var s in Sectors)
        {
            float x = hexSize * (float)(Math.Sqrt(3) * s.Q + Math.Sqrt(3) / 2 * s.R);
            float y = hexSize * (3f / 2f * s.R);

            minX = Math.Min(minX, x);
            minY = Math.Min(minY, y);
            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        return new RectF(minX, minY, maxX - minX, maxY - minY);
    }

}