using Microsoft.Maui.Graphics;
using SectorMapQuest.Managers;

namespace SectorMapQuest.Graphics;

public class HexMapDrawable : IDrawable
{
    private readonly MapManager _mapManager;

    public float HexSize { get; } = 40f;
    public float Scale { get; set; } = 1f;
    public PointF Offset { get; set; }

    public HexMapDrawable(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        canvas.SaveState();

        canvas.Translate(Offset.X, Offset.Y);
        canvas.Scale(Scale, Scale);

        foreach (var sector in _mapManager.Sectors)
        {
            var center = AxialToPixel(sector.Q, sector.R);
            DrawHex(canvas, center, sector.IsOpened);
        }

        canvas.RestoreState();
    }

    private PointF AxialToPixel(int q, int r)
    {
        float x = HexSize * (float)(Math.Sqrt(3) * q + Math.Sqrt(3) / 2 * r);
        float y = HexSize * (3f / 2f * r);
        return new PointF(x, y);
    }

    private void DrawHex(ICanvas canvas, PointF center, bool opened)
    {
        var path = new PathF();

        for (int i = 0; i < 6; i++)
        {
            double angle = Math.PI / 180 * (60 * i - 30);
            float x = center.X + HexSize * (float)Math.Cos(angle);
            float y = center.Y + HexSize * (float)Math.Sin(angle);

            if (i == 0)
                path.MoveTo(x, y);
            else
                path.LineTo(x, y);
        }

        path.Close();

        canvas.FillColor = opened ? Colors.ForestGreen : Colors.DarkSlateGray;
        canvas.FillPath(path);

        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;
        canvas.DrawPath(path);
    }
}