using System.Drawing;

public class Line : IDrawable
{
    private Point _startPoint;
    private Point _endPoint;
    private Color _color;
    private IRenderingEngine _renderingEngine;

    public Line(Point start, Point end, IRenderingEngine renderingEngine)
    {
        _startPoint = start;
        _endPoint = end;
        _renderingEngine = renderingEngine;
        _color = Color.Black;
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public Point StartPoint
    {
        get => _startPoint;
        set => _startPoint = value;
    }

    public Point EndPoint
    {
        get => _endPoint;
        set => _endPoint = value;
    }

    public void Draw(Graphics g)
    {
        using (var pen = new Pen(_color, 1))
        {
            _renderingEngine.BeginRender(g, false); // Добавлен параметр isPreviewMode = false
            _renderingEngine.DrawLine(pen, _startPoint, _endPoint);
            _renderingEngine.EndRender();
        }
    }

    public IDrawable Clone()
    {
        return new Line(_startPoint, _endPoint, _renderingEngine) { Color = this.Color };
    }

    public IDrawable CloneWithOffset()
    {
        return new Line(new Point(_startPoint.X + 10, _startPoint.Y + 10), new Point(_endPoint.X + 10, _endPoint.Y + 10), _renderingEngine) { Color = this.Color };
    }
}
//using System.Drawing;

//public class Line : DrawableBase
//{
//    public Line(IRenderingEngine renderingEngine) : base(renderingEngine) { }

//    public override void Draw(Graphics g)
//    {
//        using (var pen = CreatePen())
//        {
//            RenderingEngine.BeginRender(g);
//            RenderingEngine.DrawLine(pen, StartPoint, EndPoint);
//            RenderingEngine.EndRender();
//        }
//    }

//    public override IDrawable Clone()
//    {
//        return new Line(RenderingEngine) { StartPoint = StartPoint, EndPoint = EndPoint, Color = Color };
//    }
//}