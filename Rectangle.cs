using System.Drawing;

public class Rectangle : IDrawable
{
    private int _x, _y, _width, _height;
    private Color _color;
    private Point _startPoint;
    private Point _endPoint;
    private IRenderingEngine _renderingEngine;

    public Rectangle(int x, int y, int width, int height, IRenderingEngine renderingEngine)
    {
        _x = x;
        _y = y;
        _width = width;
        _height = height;
        _renderingEngine = renderingEngine;
        _color = Color.Black; // Цвет по умолчанию
        _startPoint = new Point(x, y); // Верхний левый угол
        _endPoint = new Point(x + width, y + height); // Нижний правый угол
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    public Point StartPoint
    {
        get => _startPoint;
        set
        {
            _startPoint = value;
            _x = _startPoint.X;
            _y = _startPoint.Y;
            _width = _endPoint.X - _x;
            _height = _endPoint.Y - _y;
        }
    }

    public Point EndPoint
    {
        get => _endPoint;
        set
        {
            _endPoint = value;
            _width = _endPoint.X - _x;
            _height = _endPoint.Y - _y;
        }
    }

    public void Draw(Graphics g)
    {
        using (var pen = new Pen(_color, 1))
        {
            _renderingEngine.BeginRender(g, false);
            _renderingEngine.DrawRectangle(pen, _x, _y, _width, _height);
            _renderingEngine.EndRender();
        }
    }

    public IDrawable Clone()
    {
        return new Rectangle(_x, _y, _width, _height, _renderingEngine) { Color = this.Color };
    }

    public IDrawable CloneWithOffset()
    {
        return new Rectangle(_x + 10, _y + 10, _width, _height, _renderingEngine) { Color = this.Color };
    }
}
//using System;
//using System.Drawing;

//public class Rectangle : DrawableBase
//{
//    public Rectangle(IRenderingEngine renderingEngine) : base(renderingEngine) { }

//    public override void Draw(Graphics g)
//    {
//        using (var pen = CreatePen())
//        {
//            int x = Math.Min(StartPoint.X, EndPoint.X);
//            int y = Math.Min(StartPoint.Y, EndPoint.Y);
//            int width = Math.Abs(EndPoint.X - StartPoint.X);
//            int height = Math.Abs(EndPoint.Y - StartPoint.Y);
//            RenderingEngine.BeginRender(g);
//            RenderingEngine.DrawRectangle(pen, x, y, width, height);
//            RenderingEngine.EndRender();
//        }
//    }

//    public override IDrawable Clone()
//    {
//        return new Rectangle(RenderingEngine) { StartPoint = StartPoint, EndPoint = EndPoint, Color = Color };
//    }
//}