using SharpDX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;

public class EllipseToolAdapter : ITool
{
    private IRenderingEngine _renderingEngine;
    private Ellipse _ellipse;
    private Point _startPoint;

    public EllipseToolAdapter(IRenderingEngine renderingEngine)
    {
        _renderingEngine = renderingEngine;
    }

    public void HandleMouseDown(MouseEventArgs e)
    {
        _startPoint = e.Location;
        _ellipse = new Ellipse(_startPoint.X, _startPoint.Y, 0, 0, _renderingEngine);
    }

    public void HandleMouseMove(MouseEventArgs e, Control canvas)
    {
        if (_ellipse != null)
        {
            int width = e.X - _startPoint.X;
            int height = e.Y - _startPoint.Y;
            _ellipse = new Ellipse(_startPoint.X, _startPoint.Y, width, height, _renderingEngine);
            using (var g = canvas.CreateGraphics())
            {
                _renderingEngine.BeginRender(g, false); // Добавлен параметр isPreviewMode = false
                _ellipse.Draw(g);
                _renderingEngine.EndRender();
            }
            canvas.Invalidate();
        }
    }

    public void HandleMouseUp(MouseEventArgs e)
    {
        if (_ellipse != null)
        {
            int width = e.X - _startPoint.X;
            int height = e.Y - _startPoint.Y;
            _ellipse = new Ellipse(_startPoint.X, _startPoint.Y, width, height, _renderingEngine);
            CanvasManager.Instance.AddDrawable(_ellipse);
        }
    }
}
//using SimpleGraphicEditor;
//using System;
//using System.Drawing;

//public class EllipseToolAdapter : Tool
//{
//    private readonly LegacyEllipseTool _legacyTool;
//    private readonly IRenderingEngine _renderingEngine;

//    public EllipseToolAdapter(LegacyEllipseTool legacyTool, IRenderingEngine renderingEngine)
//    {
//        _legacyTool = legacyTool;
//        _renderingEngine = renderingEngine;
//    }

//    public override IDrawable CreateDrawable(Point start, Point end, Color color)
//    {
//        return new EllipseAdapter(start, end, color, _legacyTool, _renderingEngine);
//    }
//}

//public class EllipseAdapter : DrawableBase
//{
//    private readonly LegacyEllipseTool _legacyTool;

//    public EllipseAdapter(Point start, Point end, Color color, LegacyEllipseTool legacyTool, IRenderingEngine renderingEngine)
//        : base(renderingEngine)
//    {
//        StartPoint = start;
//        EndPoint = end;
//        Color = color;
//        _legacyTool = legacyTool;
//    }

//    public override void Draw(Graphics g)
//    {
//        int x = Math.Min(StartPoint.X, EndPoint.X);
//        int y = Math.Min(StartPoint.Y, EndPoint.Y);
//        int width = Math.Abs(EndPoint.X - StartPoint.X);
//        int height = Math.Abs(EndPoint.Y - StartPoint.Y);
//        RenderingEngine.BeginRender(g);
//        using (var pen = CreatePen())
//        {
//            RenderingEngine.DrawEllipse(pen, x, y, width, height);
//        }
//        RenderingEngine.EndRender();
//    }

//    public override IDrawable Clone()
//    {
//        return new EllipseAdapter(StartPoint, EndPoint, Color, _legacyTool.Clone(), RenderingEngine);
//    }
//}