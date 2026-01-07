using Microsoft.Maui.Graphics;
using SectorMapQuest.Managers;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SectorMapQuest.Graphics;

public class HexMapDrawable : IDrawable
{
    private readonly MapManager _mapManager;
    private readonly PlayerDrawable _playerDrawable;
    private readonly CameraManager _camera;

    //параметры для отрисовки шестиугольника карты
    public float HexSize { get; } = 40f; //размер шестиугольника (радиус описанной окружности)

    public HexMapDrawable(MapManager mapManager, PlayerDrawable playerDrawable, CameraManager camera)
    {
        _mapManager = mapManager;
        _playerDrawable = playerDrawable;
        _camera = camera;
    }

    //метод отрисовки шестиугольников на карте
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        _camera.Update();

        //очитска фона
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(dirtyRect);

        //сохраняем текущее состояние холста для применения трансформаций
        canvas.SaveState();

        //применяем трансформации: смещение и масштаб
        canvas.Translate(_camera.Offset.X, _camera.Offset.Y);
        canvas.Scale(_camera.Scale, _camera.Scale);

        //отрисовываем все секторы из менеджера карты
        foreach (var sector in _mapManager.Sectors)
        {
            var center = AxialToPixel(sector.Q, sector.R);
            DrawHex(canvas, center, sector.IsOpened);
        }

        //игрок поверх карты
        _playerDrawable.Draw(canvas, dirtyRect);

        //восстанавливаем состояние холста
        canvas.RestoreState();
    }

    //преобразует q и r в экранные координаты x и y
    private PointF AxialToPixel(int q, int r)
    {
        float x = HexSize * (float)(Math.Sqrt(3) * q + Math.Sqrt(3) / 2 * r);
        float y = HexSize * (3f / 2f * r);
        return new PointF(x, y);
    }

    //функция рисовки отдельного шестиугольника
    private void DrawHex(ICanvas canvas, PointF center, bool opened)
    {
        var path = new PathF();

        for (int i = 0; i < 6; i++)
        {
            //вычисляем угол для каждой вершины
            double angle = Math.PI / 180 * (60 * i - 30);
            float x = center.X + HexSize * (float)Math.Cos(angle);
            float y = center.Y + HexSize * (float)Math.Sin(angle);

            if (i == 0)
                path.MoveTo(x, y); //первая точка - начало пути
            else
                path.LineTo(x, y); //последующие точки - линии
        }

        path.Close(); //замыкаем путь для создания замкнутой фигуры

        //заливка в зависимости от состояния сектора
        canvas.FillColor = opened ? Colors.ForestGreen : Colors.DarkSlateGray;
        canvas.FillPath(path);

        //обводка гексагона
        canvas.StrokeColor = Colors.Black;
        canvas.StrokeSize = 2;
        canvas.DrawPath(path);
    }
}