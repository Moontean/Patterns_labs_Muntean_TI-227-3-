using System.Drawing;

namespace SimpleGraphicEditor
{
    public class LegacyEllipseTool
    {
        public void RenderEllipse(Graphics g, int x, int y, int width, int height, Color color)
        {
            using (var pen = new Pen(color, 2))
            {
                g.DrawEllipse(pen, x, y, width, height);
            }
        }

        public LegacyEllipseTool Clone()
        {
            return new LegacyEllipseTool();
        }
    }
}