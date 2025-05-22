using System.Drawing;

public interface IDrawStyleStrategy
{
    Pen CreatePen(Color color);
}

public class SolidLineStrategy : IDrawStyleStrategy
{
    public Pen CreatePen(Color color)
    {
        return new Pen(color, 2);
    }
}

public class DashedLineStrategy : IDrawStyleStrategy
{
    public Pen CreatePen(Color color)
    {
        var pen = new Pen(color, 2);
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
        return pen;
    }
}

public class ThickLineStrategy : IDrawStyleStrategy
{
    public Pen CreatePen(Color color)
    {
        return new Pen(color, 5);
    }
}