using System.Drawing;
using SimpleGraphicEditor;

namespace SimpleGraphicEditor
{
    public abstract class DrawableDecorator : IDrawable
    {
        protected IDrawable _decoratedDrawable;

        protected DrawableDecorator(IDrawable drawable)
        {
            _decoratedDrawable = drawable;
        }

        public Point StartPoint
        {
            get => _decoratedDrawable.StartPoint;
            set => _decoratedDrawable.StartPoint = value;
        }

        public Point EndPoint
        {
            get => _decoratedDrawable.EndPoint;
            set => _decoratedDrawable.EndPoint = value;
        }

        public Color Color
        {
            get => _decoratedDrawable.Color;
            set => _decoratedDrawable.Color = value;
        }

        public virtual void Draw(Graphics g)
        {
            _decoratedDrawable.Draw(g);
        }

        public virtual IDrawable Clone()
        {
            return _decoratedDrawable.Clone();
        }

        public virtual IDrawable CloneWithOffset()
        {
            return _decoratedDrawable.CloneWithOffset();
        }

        public IDrawable GetDecoratedDrawable()
        {
            return _decoratedDrawable;
        }
    }
}