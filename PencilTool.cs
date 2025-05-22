using System.Drawing;
using SimpleGraphicEditor;

namespace SimpleGraphicEditor
{
    public class PencilTool : Tool
    {
        private readonly IRenderingEngine _renderingEngine;

        public PencilTool(IRenderingEngine renderingEngine)
        {
            _renderingEngine = renderingEngine;
        }

        public override IDrawable CreateDrawable(Point start, Point end, Color color)
        {
            var line = new Line(_renderingEngine)
            {
                StartPoint = start,
                EndPoint = end,
                Color = color
            };
            return line;
        }
    }
}