using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

public class CompositeDrawable : IDrawable
{
    private readonly List<IDrawable> _drawables = new List<IDrawable>();
    private static readonly Point DefaultStartPoint = new Point(0, 0);
    private static readonly Point DefaultEndPoint = new Point(0, 0);
    private static readonly Color DefaultColor = Color.Black;

    public Point StartPoint { get; set; } = DefaultStartPoint;
    public Point EndPoint { get; set; } = DefaultEndPoint;
    public Color Color { get; set; } = DefaultColor;

    public void Add(IDrawable drawable)
    {
        if (drawable != null)
        {
            _drawables.Add(drawable);
            Debug.WriteLine($"Добавлена фигура в группу. Всего фигур: {_drawables.Count}");
        }
    }

    public List<IDrawable> GetDrawables()
    {
        return new List<IDrawable>(_drawables);
    }

    public int GetDrawableCount()
    {
        return _drawables.Count;
    }

    public void Draw(Graphics g)
    {
        foreach (var drawable in _drawables)
        {
            if (drawable != null)
            {
                drawable.Draw(g);
            }
        }
    }

    public IDrawable Clone()
    {
        var clone = new CompositeDrawable();
        foreach (var drawable in _drawables)
        {
            if (drawable != null)
            {
                clone.Add(drawable.Clone());
            }
        }
        clone.StartPoint = StartPoint;
        clone.EndPoint = EndPoint;
        clone.Color = Color;
        return clone;
    }

    public IDrawable CloneWithOffset()
    {
        var clone = new CompositeDrawable();
        foreach (var drawable in _drawables)
        {
            if (drawable != null)
            {
                clone.Add(drawable.CloneWithOffset());
            }
        }
        clone.StartPoint = new Point(StartPoint.X + 10, StartPoint.Y + 10);
        clone.EndPoint = new Point(EndPoint.X + 10, EndPoint.Y + 10);
        clone.Color = Color;
        return clone;
    }
}