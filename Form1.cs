using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleGraphicEditor
{
    public partial class Form1 : Form, ICanvasObserver
    {
        private EditorFacade _editor;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusLabel;
        private CanvasMemento _savedMemento;
        private IRenderingEngine _renderingEngine;

        public Form1()
        {
            InitializeComponent();
            InitializeStatusBar();
            CanvasManager.Instance.AddObserver(this);
            _renderingEngine = new GdiPlusRenderingEngine();
            _editor = new EditorFacade(new BasicToolFactory(_renderingEngine));
            _editor.SetCanvas(new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height));
            canvasPictureBox.Image = CanvasManager.Instance.Canvas;
            renderComboBox.SelectedIndex = 0;
            styleComboBox.Items.AddRange(new object[] { "Solid", "Dashed", "Thick" });
            styleComboBox.SelectedIndex = 0;
        }

        private void InitializeStatusBar()
        {
            _statusStrip = new StatusStrip();
            _statusLabel = new ToolStripStatusLabel("Ready");
            _statusStrip.Items.Add(_statusLabel);
            this.Controls.Add(_statusStrip);
        }

        public void OnCanvasStateChanged(Color currentColor, Color currentFillColor, IDrawable lastDrawnObject)
        {
            _statusLabel.Text = $"Color: {currentColor.Name}, Fill: {(currentFillColor == Color.Transparent ? "None" : currentFillColor.Name)}, Last Object: {(lastDrawnObject != null ? lastDrawnObject.GetType().Name : "None")}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _renderingEngine.Initialize(canvasPictureBox);
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            _editor.SelectTool("Pencil");
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            _editor.SelectTool("Rectangle");
        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            _editor.SelectTool("Ellipse");
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _editor.SetColor(colorDialog.Color);
            }
        }

        private void btnFillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _editor.SetFillColor(colorDialog.Color);
            }
        }

        private void chkFill_CheckedChanged(object sender, EventArgs e)
        {
            _editor.ApplyFillColor(chkFill.Checked);
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            CanvasManager.Instance.AddToComposite();
            canvasPictureBox.Invalidate();
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            if (CanvasManager.Instance.LastDrawnObject != null)
            {
                var clone = CanvasManager.Instance.LastDrawnObject.CloneWithOffset();
                var command = new AddDrawableCommand(CanvasManager.Instance, clone);
                CanvasManager.Instance.ExecuteCommand(command);
                canvasPictureBox.Invalidate();
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            CanvasManager.Instance.Undo();
            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
            {
                CanvasManager.Instance.RedrawAll(g);
            }
            canvasPictureBox.Invalidate();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            CanvasManager.Instance.Redo();
            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
            {
                CanvasManager.Instance.RedrawAll(g);
            }
            canvasPictureBox.Invalidate();
        }

        private void btnSaveState_Click(object sender, EventArgs e)
        {
            _savedMemento = CanvasManager.Instance.CreateMemento();
            MessageBox.Show("Canvas state saved. Undo will only affect actions after this point until Restore is used.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRestoreState_Click(object sender, EventArgs e)
        {
            if (_savedMemento != null)
            {
                CanvasManager.Instance.RestoreMemento(_savedMemento);
                using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
                {
                    CanvasManager.Instance.RedrawAll(g);
                }
                canvasPictureBox.Invalidate();
                MessageBox.Show("Canvas state restored. Undo can now affect actions before the saved state.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No saved state to restore!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void renderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap newCanvas = new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height);
            using (var g = Graphics.FromImage(newCanvas))
            {
                g.Clear(Color.White);
                if (CanvasManager.Instance.Canvas != null)
                {
                    g.DrawImage(CanvasManager.Instance.Canvas, 0, 0);
                }
            }

            if (renderComboBox.SelectedIndex == 0)
            {
                _renderingEngine = new GdiPlusRenderingEngine();
            }
            else
            {
                _renderingEngine = new Direct2DRenderingEngine();
            }
            _renderingEngine.Initialize(canvasPictureBox);
            _editor = new EditorFacade(new BasicToolFactory(_renderingEngine));
            _editor.SetCanvas(newCanvas);
            CanvasManager.Instance.Canvas = newCanvas;
            canvasPictureBox.Image = newCanvas;
            canvasPictureBox.Invalidate();
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (styleComboBox.SelectedIndex)
            {
                case 0:
                    CanvasManager.Instance.DrawStyleStrategy = new SolidLineStrategy();
                    break;
                case 1:
                    CanvasManager.Instance.DrawStyleStrategy = new DashedLineStrategy();
                    break;
                case 2:
                    CanvasManager.Instance.DrawStyleStrategy = new ThickLineStrategy();
                    break;
            }
            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
            {
                CanvasManager.Instance.RedrawAll(g);
            }
            canvasPictureBox.Invalidate();
        }

        private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            _editor.HandleMouseDown(e);
        }

        private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            _editor.HandleMouseMove(e, canvasPictureBox);
        }

        private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _editor.HandleMouseUp(e);
            canvasPictureBox.Invalidate();
        }

        private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (CanvasManager.Instance.Canvas != null)
            {
                e.Graphics.DrawImage(CanvasManager.Instance.Canvas, 0, 0);
            }
        }
    }
}
//using System;
//using System.Drawing;
//using System.Windows.Forms;

//namespace SimpleGraphicEditor
//{
//    public partial class Form1 : Form, ICanvasObserver
//    {
//        private EditorFacade _editor;
//        private StatusStrip _statusStrip;
//        private ToolStripStatusLabel _statusLabel;
//        private CanvasMemento _savedMemento;

//        public Form1()
//        {
//            InitializeComponent();
//            InitializeStatusBar();
//            CanvasManager.Instance.AddObserver(this);
//            _renderingEngine = new GdiPlusRenderingEngine();
//            _editor = new EditorFacade(new BasicToolFactory(_renderingEngine));
//            _editor.SetCanvas(new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height));
//            canvasPictureBox.Image = CanvasManager.Instance.Canvas;
//            renderComboBox.SelectedIndex = 0;
//            styleComboBox.Items.AddRange(new object[] { "Solid", "Dashed", "Thick" });
//            styleComboBox.SelectedIndex = 0;
//        }

//        private void InitializeStatusBar()
//        {
//            _statusStrip = new StatusStrip();
//            _statusLabel = new ToolStripStatusLabel("Ready");
//            _statusStrip.Items.Add(_statusLabel);
//            this.Controls.Add(_statusStrip);
//        }

//        public void OnCanvasStateChanged(Color currentColor, Color currentFillColor, IDrawable lastDrawnObject)
//        {
//            _statusLabel.Text = $"Color: {currentColor.Name}, Fill: {(currentFillColor == Color.Transparent ? "None" : currentFillColor.Name)}, Last Object: {(lastDrawnObject != null ? lastDrawnObject.GetType().Name : "None")}";
//        }

//        private IRenderingEngine _renderingEngine;

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            _renderingEngine.Initialize(canvasPictureBox);
//        }

//        private void btnPencil_Click(object sender, EventArgs e)
//        {
//            _editor.SelectTool("Pencil");
//        }

//        private void btnRectangle_Click(object sender, EventArgs e)
//        {
//            _editor.SelectTool("Rectangle");
//        }

//        private void btnEllipse_Click(object sender, EventArgs e)
//        {
//            _editor.SelectTool("Ellipse");
//        }

//        private void btnColor_Click(object sender, EventArgs e)
//        {
//            if (colorDialog.ShowDialog() == DialogResult.OK)
//            {
//                _editor.SetColor(colorDialog.Color);
//            }
//        }

//        private void btnFillColor_Click(object sender, EventArgs e)
//        {
//            if (colorDialog.ShowDialog() == DialogResult.OK)
//            {
//                _editor.SetFillColor(colorDialog.Color);
//            }
//        }

//        private void chkFill_CheckedChanged(object sender, EventArgs e)
//        {
//            _editor.ApplyFillColor(chkFill.Checked);
//        }

//        private void btnGroup_Click(object sender, EventArgs e)
//        {
//            CanvasManager.Instance.AddToComposite();
//            canvasPictureBox.Invalidate();
//        }

//        private void btnClone_Click(object sender, EventArgs e)
//        {
//            if (CanvasManager.Instance.LastDrawnObject != null)
//            {
//                var clone = CanvasManager.Instance.LastDrawnObject.CloneWithOffset();
//                var command = new AddDrawableCommand(CanvasManager.Instance, clone);
//                CanvasManager.Instance.ExecuteCommand(command);
//                canvasPictureBox.Invalidate();
//            }
//        }

//        private void btnUndo_Click(object sender, EventArgs e)
//        {
//            CanvasManager.Instance.Undo();
//            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//            {
//                CanvasManager.Instance.RedrawAll(g);
//            }
//            canvasPictureBox.Invalidate();
//        }

//        private void btnRedo_Click(object sender, EventArgs e)
//        {
//            CanvasManager.Instance.Redo();
//            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//            {
//                CanvasManager.Instance.RedrawAll(g);
//            }
//            canvasPictureBox.Invalidate();
//        }

//        private void btnSaveState_Click(object sender, EventArgs e)
//        {
//            _savedMemento = CanvasManager.Instance.CreateMemento();
//            MessageBox.Show("Canvas state saved. Undo will only affect actions after this point until Restore is used.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void btnRestoreState_Click(object sender, EventArgs e)
//        {
//            if (_savedMemento != null)
//            {
//                CanvasManager.Instance.RestoreMemento(_savedMemento);
//                using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//                {
//                    CanvasManager.Instance.RedrawAll(g);
//                }
//                canvasPictureBox.Invalidate();
//                MessageBox.Show("Canvas state restored. Undo can now affect actions before the saved state.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            else
//            {
//                MessageBox.Show("No saved state to restore!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        private void renderComboBox_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (renderComboBox.SelectedIndex == 0)
//            {
//                _renderingEngine = new GdiPlusRenderingEngine();
//            }
//            else
//            {
//                _renderingEngine = new Direct2DRenderingEngine();
//            }
//            _renderingEngine.Initialize(canvasPictureBox);
//            _editor = new EditorFacade(new BasicToolFactory(_renderingEngine));
//            _editor.SetCanvas(new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height));
//            canvasPictureBox.Invalidate();
//        }

//        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            switch (styleComboBox.SelectedIndex)
//            {
//                case 0:
//                    CanvasManager.Instance.DrawStyleStrategy = new SolidLineStrategy();
//                    break;
//                case 1:
//                    CanvasManager.Instance.DrawStyleStrategy = new DashedLineStrategy();
//                    break;
//                case 2:
//                    CanvasManager.Instance.DrawStyleStrategy = new ThickLineStrategy();
//                    break;
//            }
//            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//            {
//                CanvasManager.Instance.RedrawAll(g);
//            }
//            canvasPictureBox.Invalidate();
//        }

//        private void canvasPictureBox_MouseDown(object sender, MouseEventArgs e)
//        {
//            _editor.HandleMouseDown(e);
//        }

//        private void canvasPictureBox_MouseMove(object sender, MouseEventArgs e)
//        {
//            _editor.HandleMouseMove(e, canvasPictureBox);
//        }

//        private void canvasPictureBox_MouseUp(object sender, MouseEventArgs e)
//        {
//            _editor.HandleMouseUp(e);
//            canvasPictureBox.Invalidate();
//        }

//        private void canvasPictureBox_Paint(object sender, PaintEventArgs e)
//        {
//            if (CanvasManager.Instance.Canvas != null)
//            {
//                e.Graphics.DrawImage(CanvasManager.Instance.Canvas, 0, 0);
//            }
//        }
//    }
//}
