using System.Drawing;
using System.Windows.Forms;

public interface IEditorState
{
    void HandleMouseDown(EditorFacade editor, MouseEventArgs e);
    void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas);
    void HandleMouseUp(EditorFacade editor, MouseEventArgs e);
    void SelectTool(EditorFacade editor, string tool);
}

public class IdleState : IEditorState
{
    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && editor.CurrentTool != null)
        {
            editor.SetState(new DrawingState(e.Location));
            editor.HandleMouseDown(e);
        }
    }

    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas) { }

    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e) { }

    public void SelectTool(EditorFacade editor, string tool)
    {
        editor.SetState(new SelectingToolState());
        editor.SelectTool(tool);
        editor.SetState(new IdleState());
    }
}

public class DrawingState : IEditorState
{
    private readonly Point _startPoint;

    public DrawingState(Point startPoint)
    {
        _startPoint = startPoint;
    }

    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e) { }

    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas)
    {
        using (var g = canvas.CreateGraphics())
        {
            g.Clear(Color.White);
            CanvasManager.Instance.RedrawAll(g);
            var preview = editor.CreatePreviewDrawable(_startPoint, e.Location);
            if (preview != null)
            {
                preview.Draw(g);
            }
        }
    }

    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e)
    {
        editor.Draw(_startPoint, e.Location);
        editor.SetState(new IdleState());
    }

    public void SelectTool(EditorFacade editor, string tool) { }
}

public class SelectingToolState : IEditorState
{
    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e) { }

    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas) { }

    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e) { }

    public void SelectTool(EditorFacade editor, string tool)
    {
        switch (tool)
        {
            case "Pencil":
                editor.SelectPencilTool();
                break;
            case "Rectangle":
                editor.SelectRectangleTool();
                break;
            case "Ellipse":
                editor.SelectEllipseTool();
                break;
        }
    }
}
//using System.Drawing;
//using System.Windows.Forms;

//public interface IEditorState
//{
//    void HandleMouseDown(EditorFacade editor, MouseEventArgs e);
//    void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas);
//    void HandleMouseUp(EditorFacade editor, MouseEventArgs e);
//    void SelectTool(EditorFacade editor, string tool);
//}

//public class IdleState : IEditorState
//{
//    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e)
//    {
//        if (e.Button == MouseButtons.Left && editor.CurrentTool != null)
//        {
//            editor.SetState(new DrawingState(e.Location));
//            editor.HandleMouseDown(e);
//        }
//    }

//    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas) { }

//    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e) { }

//    public void SelectTool(EditorFacade editor, string tool)
//    {
//        editor.SetState(new SelectingToolState());
//        editor.SelectTool(tool);
//        editor.SetState(new IdleState());
//    }
//}

//public class DrawingState : IEditorState
//{
//    private readonly Point _startPoint;

//    public DrawingState(Point startPoint)
//    {
//        _startPoint = startPoint;
//    }

//    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e) { }

//    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas)
//    {
//        using (var g = canvas.CreateGraphics())
//        {
//            g.Clear(Color.White);
//            CanvasManager.Instance.RedrawAll(g);
//            var preview = editor.CreatePreviewDrawable(_startPoint, e.Location);
//            if (preview != null)
//            {
//                preview.Draw(g);
//            }
//        }
//    }

//    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e)
//    {
//        editor.Draw(_startPoint, e.Location);
//        editor.SetState(new IdleState());
//    }

//    public void SelectTool(EditorFacade editor, string tool) { }
//}

//public class SelectingToolState : IEditorState
//{
//    public void HandleMouseDown(EditorFacade editor, MouseEventArgs e) { }

//    public void HandleMouseMove(EditorFacade editor, MouseEventArgs e, PictureBox canvas) { }

//    public void HandleMouseUp(EditorFacade editor, MouseEventArgs e) { }

//    public void SelectTool(EditorFacade editor, string tool)
//    {
//        switch (tool)
//        {
//            case "Pencil":
//                editor.SelectPencilTool();
//                break;
//            case "Rectangle":
//                editor.SelectRectangleTool();
//                break;
//            case "Ellipse":
//                editor.SelectEllipseTool();
//                break;
//        }
//    }
//}