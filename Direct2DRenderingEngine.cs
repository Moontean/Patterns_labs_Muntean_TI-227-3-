using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;

public class Direct2DRenderingEngine : IRenderingEngine
{
    private WindowRenderTarget _renderTarget;
    private System.Drawing.Bitmap _bitmap;
    private Control _control;
    private SharpDX.Direct2D1.Factory _factory;

    public Direct2DRenderingEngine()
    {
        _factory = new SharpDX.Direct2D1.Factory();
    }

    public void Initialize(Control control)
    {
        _control = control ?? throw new ArgumentNullException(nameof(control));

        if (_control.Width <= 0 || _control.Height <= 0)
        {
            throw new ArgumentException("Control size must be greater than zero.");
        }

        var hwndProps = new HwndRenderTargetProperties
        {
            Hwnd = _control.Handle,
            PixelSize = new Size2(_control.Width, _control.Height),
            PresentOptions = PresentOptions.None
        };
        var renderProps = new RenderTargetProperties
        {
            Type = RenderTargetType.Default,
            PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)
        };
        _renderTarget = new WindowRenderTarget(_factory, renderProps, hwndProps);

        _bitmap = new System.Drawing.Bitmap(_control.Width, _control.Height);
        _control.BackgroundImage = _bitmap;
    }

    public void SetCanvas(System.Drawing.Bitmap canvas)
    {
        _bitmap?.Dispose();
        _bitmap = canvas;
        if (_renderTarget != null && _bitmap != null)
        {
            _renderTarget.Dispose();
            var hwndProps = new HwndRenderTargetProperties
            {
                Hwnd = _control.Handle,
                PixelSize = new Size2(_bitmap.Width, _bitmap.Height),
                PresentOptions = PresentOptions.None
            };
            var renderProps = new RenderTargetProperties();
            _renderTarget = new WindowRenderTarget(_factory, renderProps, hwndProps);
        }
    }

    public void BeginRender(Graphics g, bool isPreviewMode)
    {
        if (_renderTarget == null)
        {
            throw new InvalidOperationException("RenderTarget is not initialized.");
        }
        _renderTarget.BeginDraw();
        _renderTarget.Clear(new RawColor4(1f, 1f, 1f, 1f)); // Очистка белым цветом
    }

    public void EndRender()
    {
        if (_renderTarget != null)
        {
            _renderTarget.EndDraw();
            _control.Invalidate();
        }
    }

    public void DrawLine(System.Drawing.Pen pen, Point start, Point end)
    {
        if (_renderTarget != null)
        {
            var color = new RawColor4(pen.Color.R / 255.0f, pen.Color.G / 255.0f, pen.Color.B / 255.0f, pen.Color.A / 255.0f);
            using (var brush = new SolidColorBrush(_renderTarget, color))
            {
                _renderTarget.DrawLine(new RawVector2(start.X, start.Y), new RawVector2(end.X, end.Y), brush, pen.Width);
            }
        }
    }

    public void DrawRectangle(System.Drawing.Pen pen, int x, int y, int width, int height)
    {
        if (_renderTarget != null)
        {
            var color = new RawColor4(pen.Color.R / 255.0f, pen.Color.G / 255.0f, pen.Color.B / 255.0f, pen.Color.A / 255.0f);
            using (var brush = new SolidColorBrush(_renderTarget, color))
            {
                var rect = new RawRectangleF(x, y, x + width, y + height);
                _renderTarget.DrawRectangle(rect, brush, pen.Width);
            }
        }
    }

    public void DrawEllipse(System.Drawing.Pen pen, int x, int y, int width, int height)
    {
        if (_renderTarget != null)
        {
            var color = new RawColor4(pen.Color.R / 255.0f, pen.Color.G / 255.0f, pen.Color.B / 255.0f, pen.Color.A / 255.0f);
            using (var brush = new SolidColorBrush(_renderTarget, color))
            {
                var ellipse = new Ellipse(new RawVector2(x + width / 2.0f, y + height / 2.0f), width / 2.0f, height / 2.0f);
                _renderTarget.DrawEllipse(ellipse, brush, pen.Width);
            }
        }
    }

    public void FillRectangle(System.Drawing.Brush brush, int x, int y, int width, int height)
    {
        if (_renderTarget != null && brush is System.Drawing.SolidBrush solidBrush)
        {
            var color = new RawColor4(solidBrush.Color.R / 255.0f, solidBrush.Color.G / 255.0f, solidBrush.Color.B / 255.0f, solidBrush.Color.A / 255.0f);
            using (var fillBrush = new SolidColorBrush(_renderTarget, color))
            {
                var rect = new RawRectangleF(x, y, x + width, y + height);
                _renderTarget.FillRectangle(rect, fillBrush);
            }
        }
    }

    public void FillEllipse(System.Drawing.Brush brush, int x, int y, int width, int height)
    {
        if (_renderTarget != null && brush is System.Drawing.SolidBrush solidBrush)
        {
            var color = new RawColor4(solidBrush.Color.R / 255.0f, solidBrush.Color.G / 255.0f, solidBrush.Color.B / 255.0f, solidBrush.Color.A / 255.0f);
            using (var fillBrush = new SolidColorBrush(_renderTarget, color))
            {
                var ellipse = new Ellipse(new RawVector2(x + width / 2.0f, y + height / 2.0f), width / 2.0f, height / 2.0f);
                _renderTarget.FillEllipse(ellipse, fillBrush);
            }
        }
    }

    public void Dispose()
    {
        _renderTarget?.Dispose();
        _bitmap?.Dispose();
        _factory?.Dispose();
    }
}
//using System.Drawing;
//using System.Windows.Forms;
//using SharpDX;
//using SharpDX.Direct2D1;
//using SharpDX.DXGI;
//using SharpDX.Mathematics.Interop;
//using SharpDX.WIC;
//using System.Drawing.Imaging;
//using System;

//public class Direct2DRenderingEngine : IRenderingEngine
//{
//    private SharpDX.Direct2D1.Factory _d2dFactory;
//    private WindowRenderTarget _renderTarget;
//    private ImagingFactory _wicFactory;
//    private SharpDX.WIC.Bitmap _wicBitmap;
//    private WicRenderTarget _wicRenderTarget;
//    private System.Drawing.Bitmap _bitmap;
//    private bool _isPreviewMode; // Флаг для режима предпросмотра
//    private bool _isRendering; // Флаг для отслеживания состояния рендеринга

//    public void Initialize(Control control)
//    {
//        _d2dFactory = new SharpDX.Direct2D1.Factory();
//        _wicFactory = new ImagingFactory();

//        var renderTargetProperties = new HwndRenderTargetProperties
//        {
//            Hwnd = control.Handle,
//            PixelSize = new Size2(control.ClientSize.Width, control.ClientSize.Height),
//            PresentOptions = SharpDX.Direct2D1.PresentOptions.Immediately
//        };
//        _renderTarget = new WindowRenderTarget(_d2dFactory, new RenderTargetProperties(), renderTargetProperties);

//        // Создаём WIC Bitmap для рендеринга
//        _wicBitmap = new SharpDX.WIC.Bitmap(_wicFactory, control.ClientSize.Width, control.ClientSize.Height, SharpDX.WIC.PixelFormat.Format32bppPBGRA, BitmapCreateCacheOption.CacheOnDemand);
//        _wicRenderTarget = new WicRenderTarget(_d2dFactory, _wicBitmap, new RenderTargetProperties());

//        // Очищаем холст при инициализации
//        _wicRenderTarget.BeginDraw();
//        _wicRenderTarget.Clear(new RawColor4(1f, 1f, 1f, 1f)); // Белый фон
//        _wicRenderTarget.EndDraw();
//    }

//    public void SetCanvas(System.Drawing.Bitmap bitmap)
//    {
//        _bitmap = bitmap;
//        _wicRenderTarget?.Dispose();
//        _wicBitmap?.Dispose();
//        _wicBitmap = new SharpDX.WIC.Bitmap(_wicFactory, _bitmap.Width, _bitmap.Height, SharpDX.WIC.PixelFormat.Format32bppPBGRA, BitmapCreateCacheOption.CacheOnDemand);
//        _wicRenderTarget = new WicRenderTarget(_d2dFactory, _wicBitmap, new RenderTargetProperties());

//        // Очищаем холст при установке нового битмапа
//        _wicRenderTarget.BeginDraw();
//        _wicRenderTarget.Clear(new RawColor4(1f, 1f, 1f, 1f)); // Белый фон
//        _wicRenderTarget.EndDraw();
//    }

//    public void BeginRender(Graphics g, bool isPreviewMode = false)
//    {
//        if (_isRendering)
//        {
//            // Если уже в процессе рендеринга, пропускаем вызов
//            return;
//        }

//        _isRendering = true;
//        _isPreviewMode = isPreviewMode;
//        _wicRenderTarget.BeginDraw();

//        // Очищаем холст только в режиме предпросмотра
//        if (_isPreviewMode)
//        {
//            _wicRenderTarget.Clear(new RawColor4(1f, 1f, 1f, 1f)); // Белый фон
//        }
//    }

//    public void DrawLine(Pen pen, Point start, Point end)
//    {
//        using (var brush = new SharpDX.Direct2D1.SolidColorBrush(_wicRenderTarget, new RawColor4(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f, pen.Color.A / 255f)))
//        {
//            _wicRenderTarget.DrawLine(new RawVector2(start.X, start.Y), new RawVector2(end.X, end.Y), brush, pen.Width);
//        }
//    }

//    public void DrawRectangle(Pen pen, int x, int y, int width, int height)
//    {
//        using (var brush = new SharpDX.Direct2D1.SolidColorBrush(_wicRenderTarget, new RawColor4(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f, pen.Color.A / 255f)))
//        {
//            _wicRenderTarget.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), brush, pen.Width);
//        }
//    }

//    public void DrawEllipse(Pen pen, int x, int y, int width, int height)
//    {
//        using (var brush = new SharpDX.Direct2D1.SolidColorBrush(_wicRenderTarget, new RawColor4(pen.Color.R / 255f, pen.Color.G / 255f, pen.Color.B / 255f, pen.Color.A / 255f)))
//        {
//            var ellipse = new Ellipse(new RawVector2(x + width / 2f, y + height / 2f), width / 2f, height / 2f);
//            _wicRenderTarget.DrawEllipse(ellipse, brush, pen.Width);
//        }
//    }

//    public void FillRectangle(System.Drawing.Brush brush, int x, int y, int width, int height)
//    {
//        if (brush is SolidBrush solidBrush)
//        {
//            using (var d2dBrush = new SharpDX.Direct2D1.SolidColorBrush(_wicRenderTarget, new RawColor4(solidBrush.Color.R / 255f, solidBrush.Color.G / 255f, solidBrush.Color.B / 255f, solidBrush.Color.A / 255f)))
//            {
//                _wicRenderTarget.FillRectangle(new RawRectangleF(x, y, x + width, y + height), d2dBrush);
//            }
//        }
//    }

//    public void FillEllipse(System.Drawing.Brush brush, int x, int y, int width, int height)
//    {
//        if (brush is SolidBrush solidBrush)
//        {
//            using (var d2dBrush = new SharpDX.Direct2D1.SolidColorBrush(_wicRenderTarget, new RawColor4(solidBrush.Color.R / 255f, solidBrush.Color.G / 255f, solidBrush.Color.B / 255f, solidBrush.Color.A / 255f)))
//            {
//                var ellipse = new Ellipse(new RawVector2(x + width / 2f, y + height / 2f), width / 2f, height / 2f);
//                _wicRenderTarget.FillEllipse(ellipse, d2dBrush);
//            }
//        }
//    }

//    public void EndRender()
//    {
//        if (!_isRendering)
//        {
//            // Если рендеринг не начат, пропускаем вызов
//            return;
//        }

//        _wicRenderTarget.EndDraw();

//        // Копируем данные в System.Drawing.Bitmap только если не в режиме предпросмотра
//        if (!_isPreviewMode)
//        {
//            var bitmapData = _bitmap.LockBits(
//                new System.Drawing.Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
//                System.Drawing.Imaging.ImageLockMode.WriteOnly,
//                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

//            using (var wicLock = _wicBitmap.Lock(BitmapLockFlags.Read))
//            {
//                int bytesPerRow = _bitmap.Width * 4;
//                int srcStride = wicLock.Data.Pitch;
//                int destStride = bitmapData.Stride;
//                IntPtr sourcePtr = wicLock.Data.DataPointer;
//                IntPtr destPtr = bitmapData.Scan0;

//                byte[] rowBuffer = new byte[bytesPerRow];
//                for (int y = 0; y < _bitmap.Height; y++)
//                {
//                    System.Runtime.InteropServices.Marshal.Copy(sourcePtr, rowBuffer, 0, bytesPerRow);
//                    System.Runtime.InteropServices.Marshal.Copy(rowBuffer, 0, destPtr, bytesPerRow);
//                    sourcePtr = IntPtr.Add(sourcePtr, srcStride);
//                    destPtr = IntPtr.Add(destPtr, destStride);
//                }
//            }

//            _bitmap.UnlockBits(bitmapData);
//        }

//        _isRendering = false;
//    }
//}