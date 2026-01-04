namespace SectorMapQuest.Managers;

public class MapManager
{
    //список всех секторов на карте
    public List<Sector> Sectors { get; } = new();

    //генерирует гексагональную карту заданного радиуса
    public void Generate(int radius)
    {
        //очищаем предыдущую карту
        Sectors.Clear();

        int id = 0;

        //проходим по всем возможным координатам в квадратной области
        for (int q = -radius; q <= radius; q++)
        {
            for (int r = -radius; r <= radius; r++)
            {
                //фильтруем только те координаты, которые попадают в гексагональную область
                if (Math.Abs(q + r) > radius)
                    continue;

                //создаем новый сектор
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

    //получает сектор по осевым координатам (q, r)
    public Sector GetAt(int q, int r)
        => Sectors.FirstOrDefault(s => s.Q == q && s.R == r);

    //вычисляет границы мира в пикселях для всех секторов
    public RectF GetWorldBounds(float hexSize)
    {
        //инициализируем границы максимальными/минимальными значениями
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        foreach (var s in Sectors)
        {
            //преобразуем осевые координаты в пиксельные (та же формула, что и в HexMapDrawable)
            float x = hexSize * (float)(Math.Sqrt(3) * s.Q + Math.Sqrt(3) / 2 * s.R);
            float y = hexSize * (3f / 2f * s.R);

            //обновляем границы
            minX = Math.Min(minX, x);
            minY = Math.Min(minY, y);
            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        //возвращаем прямоугольник с вычисленными границами
        return new RectF(minX, minY, maxX - minX, maxY - minY);
    }
}