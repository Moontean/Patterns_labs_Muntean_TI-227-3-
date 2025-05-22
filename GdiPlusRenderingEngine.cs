using System;
using System.Drawing;
using System.Windows.Forms;

public class GdiPlusRenderingEngine : IRenderingEngine
{
    private Bitmap _canvas;

    public void Initialize(Control control)
    {
        _canvas = new Bitmap(control.Width, control.Height);
        control.BackgroundImage = _canvas;
    }

    public void SetCanvas(Bitmap canvas)
    {
        _canvas?.Dispose();
        _canvas = canvas;
    }

    public void BeginRender(Graphics g, bool isPreviewMode)
    {
        if (_canvas != null)
        {
            g.Clear(Color.White);
        }
    }

    public void EndRender()
    {
        // Нет необходимости в дополнительных действиях для GDI+
    }

    public void DrawLine(Pen pen, Point start, Point end)
    {
        if (_canvas != null)
        {
            using (var g = Graphics.FromImage(_canvas))
            {
                g.DrawLine(pen, start, end);
            }
        }
    }

    public void DrawRectangle(Pen pen, int x, int y, int width, int height)
    {
        if (_canvas != null)
        {
            using (var g = Graphics.FromImage(_canvas))
            {
                g.DrawRectangle(pen, x, y, width, height);
            }
        }
    }

    public void DrawEllipse(Pen pen, int x, int y, int width, int height)
    {
        if (_canvas != null)
        {
            using (var g = Graphics.FromImage(_canvas))
            {
                g.DrawEllipse(pen, x, y, width, height);
            }
        }
    }

    public void FillRectangle(Brush brush, int x, int y, int width, int height)
    {
        if (_canvas != null)
        {
            using (var g = Graphics.FromImage(_canvas))
            {
                g.FillRectangle(brush, x, y, width, height);
            }
        }
    }

    public void FillEllipse(Brush brush, int x, int y, int width, int height)
    {
        if (_canvas != null)
        {
            using (var g = Graphics.FromImage(_canvas))
            {
                g.FillEllipse(brush, x, y, width, height);
            }
        }
    }

    public void Dispose()
    {
        _canvas?.Dispose();
    }
}
//using System.Drawing;
//using System.Windows.Forms;

//public class GdiPlusRenderingEngine : IRenderingEngine
//{
//    private Graphics _graphics;

//    public void Initialize(Control control) { }


//    public void SetCanvas(Bitmap bitmap) => _graphics = Graphics.FromImage(bitmap);

//    public void BeginRender(Graphics g, bool isPreviewMode = false) => _graphics = g;

//    public void DrawLine(Pen pen, Point start, Point end)
//    {
//        _graphics.DrawLine(pen, start, end);
//    }

//    public void DrawRectangle(Pen pen, int x, int y, int width, int height)
//    {
//        _graphics.DrawRectangle(pen, x, y, width, height);
//    }

//    public void DrawEllipse(Pen pen, int x, int y, int width, int height)
//    {
//        _graphics.DrawEllipse(pen, x, y, width, height);
//    }

//    public void FillRectangle(Brush brush, int x, int y, int width, int height)
//    {
//        _graphics.FillRectangle(brush, x, y, width, height);
//    }

//    public void FillEllipse(Brush brush, int x, int y, int width, int height)
//    {
//        _graphics.FillEllipse(brush, x, y, width, height);
//    }

//    public void EndRender() { }
//}