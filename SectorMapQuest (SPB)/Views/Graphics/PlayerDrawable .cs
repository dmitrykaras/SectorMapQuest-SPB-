using Microsoft.Maui.Graphics;
using SectorMapQuest.Managers;

namespace SectorMapQuest.Graphics;

public class PlayerDrawable : IDrawable
{
    private readonly PlayerPositionManager _player;

    public float Radius { get; } = 10f;

    public PlayerDrawable(PlayerPositionManager player)
    {
        _player = player;
    }

    //рисуем player 
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var p = _player.Position;

        canvas.FillColor = Colors.Red;
        canvas.FillCircle(p.X, p.Y, Radius);

        canvas.StrokeColor = Colors.White;
        canvas.StrokeSize = 2;
        canvas.DrawCircle(p.X, p.Y, Radius);
    }
}