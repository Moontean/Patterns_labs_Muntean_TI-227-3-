using System.Drawing;

public abstract class DrawableBase : IDrawable
{
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }
    public Color Color { get; set; }
    public IRenderingEngine RenderingEngine { get; protected set; }

    protected DrawableBase(IRenderingEngine renderingEngine)
    {
        RenderingEngine = renderingEngine;
    }

    protected Pen CreatePen()
    {
        return CanvasManager.Instance.DrawStyleStrategy.CreatePen(Color);
    }

    public abstract void Draw(Graphics g);
    public abstract IDrawable Clone();

    public virtual IDrawable CloneWithOffset()
    {
        var clone = Clone();
        clone.StartPoint = new Point(StartPoint.X + 10, StartPoint.Y + 10);
        clone.EndPoint = new Point(EndPoint.X + 10, EndPoint.Y + 10);
        return clone;
    }
}