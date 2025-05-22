using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System;

public interface IRenderingEngine : IDisposable
{
    void Initialize(Control control);
    void SetCanvas(Bitmap canvas);
    void BeginRender(Graphics g, bool isPreviewMode);
    void EndRender();
    void DrawLine(Pen pen, Point start, Point end);
    void DrawRectangle(Pen pen, int x, int y, int width, int height);
    void DrawEllipse(Pen pen, int x, int y, int width, int height);
    void FillRectangle(Brush brush, int x, int y, int width, int height);
    void FillEllipse(Brush brush, int x, int y, int width, int height);
}
//using System.Drawing;
//using System.Windows.Forms;
//// фасад, адаптер , в принципе можно считать за bridge
//public interface IRenderingEngine
//{
//    void Initialize(Control control);
//    void SetCanvas(Bitmap bitmap);
//    void BeginRender(Graphics g, bool isPreviewMode = false);
//    void DrawLine(Pen pen, Point start, Point end);
//    void DrawRectangle(Pen pen, int x, int y, int width, int height);
//    void DrawEllipse(Pen pen, int x, int y, int width, int height);
//    void FillRectangle(Brush brush, int x, int y, int width, int height);
//    void FillEllipse(Brush brush, int x, int y, int width, int height);
//    void EndRender();
//}