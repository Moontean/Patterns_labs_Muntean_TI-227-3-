using System.Collections.Generic;
using System.Drawing;
using SimpleGraphicEditor;

namespace SimpleGraphicEditor
{
    public class BasicToolFactory : IToolFactory
    {
        private readonly IRenderingEngine _renderingEngine;

        public BasicToolFactory(IRenderingEngine renderingEngine)
        {
            _renderingEngine = renderingEngine;
        }

        public Tool CreatePencilTool()
        {
            return new PencilTool(_renderingEngine);
        }

        public Tool CreateRectangleTool()
        {
            return new RectangleTool(_renderingEngine);
        }

        public Tool CreateEllipseTool()
        {
            return new EllipseToolAdapter(new LegacyEllipseTool(), _renderingEngine);
        }
    }
}