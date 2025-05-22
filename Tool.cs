using System.Drawing;

public abstract class Tool
{
    public abstract IDrawable CreateDrawable(Point start, Point end, Color color);
}