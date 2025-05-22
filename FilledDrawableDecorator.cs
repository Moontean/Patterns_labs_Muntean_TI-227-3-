using System.Drawing;
using SimpleGraphicEditor;

namespace SimpleGraphicEditor
{
    public class FilledDrawableDecorator : DrawableDecorator
    {
        private readonly Color _fillColor;

        public FilledDrawableDecorator(IDrawable drawable, Color fillColor)
            : base(drawable)
        {
            _fillColor = fillColor;
        }

        public override void Draw(Graphics g)
        {
            if (_fillColor != Color.Transparent && (_decoratedDrawable is Rectangle || _decoratedDrawable is EllipseAdapter))
            {
                int x = System.Math.Min(StartPoint.X, EndPoint.X);
                int y = System.Math.Min(StartPoint.Y, EndPoint.Y);
                int width = System.Math.Abs(EndPoint.X - StartPoint.X);
                int height = System.Math.Abs(EndPoint.Y - StartPoint.Y);

                using (var brush = new SolidBrush(_fillColor))
                {
                    if (_decoratedDrawable is Rectangle)
                    {
                        g.FillRectangle(brush, x, y, width, height);
                    }
                    else if (_decoratedDrawable is EllipseAdapter)
                    {
                        g.FillEllipse(brush, x, y, width, height);
                    }
                }
            }
            _decoratedDrawable.Draw(g);
        }

        public override IDrawable Clone()
        {
            return new FilledDrawableDecorator(_decoratedDrawable.Clone(), _fillColor);
        }

        public override IDrawable CloneWithOffset()
        {
            return new FilledDrawableDecorator(_decoratedDrawable.CloneWithOffset(), _fillColor);
        }
    }
}