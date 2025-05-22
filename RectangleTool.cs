using System.Drawing;
using System.Windows.Forms;

public class RectangleTool : ITool
{
    private IRenderingEngine _renderingEngine;
    private Rectangle _rectangle;
    private Point _startPoint;

    public RectangleTool(IRenderingEngine renderingEngine)
    {
        _renderingEngine = renderingEngine;
    }

    public void HandleMouseDown(MouseEventArgs e)
    {
        _startPoint = e.Location;
        _rectangle = new Rectangle(_startPoint.X, _startPoint.Y, 0, 0, _renderingEngine); // Добавлен параметр y = _startPoint.Y
    }

    public void HandleMouseMove(MouseEventArgs e, Control canvas)
    {
        if (_rectangle != null)
        {
            int width = e.X - _startPoint.X;
            int height = e.Y - _startPoint.Y;
            _rectangle = new Rectangle(_startPoint.X, _startPoint.Y, width, height, _renderingEngine);
            using (var g = canvas.CreateGraphics())
            {
                _renderingEngine.BeginRender(g, false); // Добавлен параметр isPreviewMode = false
                _rectangle.Draw(g);
                _renderingEngine.EndRender();
            }
            canvas.Invalidate();
        }
    }

    public void HandleMouseUp(MouseEventArgs e)
    {
        if (_rectangle != null)
        {
            int width = e.X - _startPoint.X;
            int height = e.Y - _startPoint.Y;
            _rectangle = new Rectangle(_startPoint.X, _startPoint.Y, width, height, _renderingEngine);
            CanvasManager.Instance.AddDrawable(_rectangle);
        }
    }
}
//using System.Drawing;
//using SimpleGraphicEditor;

//namespace SimpleGraphicEditor
//{
//    public class RectangleTool : Tool
//    {
//        private readonly IRenderingEngine _renderingEngine;

//        public RectangleTool(IRenderingEngine renderingEngine)
//        {
//            _renderingEngine = renderingEngine;
//        }

//        public override IDrawable CreateDrawable(Point start, Point end, Color color)
//        {
//            var rectangle = new Rectangle(_renderingEngine)
//            {
//                StartPoint = start,
//                EndPoint = end,
//                Color = color
//            };
//            return rectangle;
//        }
//    }
//}