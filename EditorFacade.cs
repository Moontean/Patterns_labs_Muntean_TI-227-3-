using System.Drawing;
using System.Windows.Forms;

public class EditorFacade
{
    private readonly IToolFactory _toolFactory;
    private Tool _currentTool;
    private IEditorState _currentState;

    public EditorFacade(IToolFactory toolFactory)
    {
        _toolFactory = toolFactory;
        _currentState = new IdleState();
    }

    public Tool CurrentTool
    {
        get => _currentTool;
        set => _currentTool = value;
    }

    public void SetState(IEditorState state)
    {
        _currentState = state;
    }

    public void SetCanvas(Bitmap canvas)
    {
        CanvasManager.Instance.Canvas = canvas;
    }

    public void SelectPencilTool()
    {
        _currentTool = _toolFactory.CreatePencilTool();
    }

    public void SelectRectangleTool()
    {
        _currentTool = _toolFactory.CreateRectangleTool();
    }

    public void SelectEllipseTool()
    {
        _currentTool = _toolFactory.CreateEllipseTool();
    }

    public void SelectTool(string tool)
    {
        _currentState.SelectTool(this, tool);
    }

    public void SetColor(Color color)
    {
        var command = new SetColorCommand(CanvasManager.Instance, color);
        CanvasManager.Instance.ExecuteCommand(command);
    }

    public void SetFillColor(Color fillColor)
    {
        CanvasManager.Instance.SelectedFillColor = fillColor;
    }

    public void ApplyFillColor(bool apply)
    {
        CanvasManager.Instance.CurrentFillColor = apply ? CanvasManager.Instance.SelectedFillColor : Color.Transparent;
        using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
        {
            CanvasManager.Instance.RedrawAll(g);
        }
    }

    public void Draw(Point start, Point end)
    {
        if (_currentTool != null)
        {
            var drawable = _currentTool.CreateDrawable(start, end, CanvasManager.Instance.CurrentColor);
            var command = new AddDrawableCommand(CanvasManager.Instance, drawable);
            CanvasManager.Instance.ExecuteCommand(command);
            if (CanvasManager.Instance.CurrentFillColor != Color.Transparent)
            {
                CanvasManager.Instance.ApplyFillDecorator();
            }
            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
            {
                CanvasManager.Instance.RedrawAll(g);
            }
        }
    }

    public IDrawable CreatePreviewDrawable(Point start, Point end)
    {
        return _currentTool?.CreateDrawable(start, end, CanvasManager.Instance.CurrentColor);
    }

    public void HandleMouseDown(MouseEventArgs e)
    {
        _currentState.HandleMouseDown(this, e);
    }

    public void HandleMouseMove(MouseEventArgs e, PictureBox canvas)
    {
        _currentState.HandleMouseMove(this, e, canvas);
    }

    public void HandleMouseUp(MouseEventArgs e)
    {
        _currentState.HandleMouseUp(this, e);
    }
}
//using System.Drawing;
//using System.Windows.Forms;

//public class EditorFacade
//{
//    private readonly IToolFactory _toolFactory;
//    private Tool _currentTool;
//    private IEditorState _currentState;

//    public EditorFacade(IToolFactory toolFactory)
//    {
//        _toolFactory = toolFactory;
//        _currentState = new IdleState();
//    }

//    public Tool CurrentTool
//    {
//        get => _currentTool;
//        set => _currentTool = value;
//    }

//    public void SetState(IEditorState state)
//    {
//        _currentState = state;
//    }

//    public void SetCanvas(Bitmap canvas)
//    {
//        CanvasManager.Instance.Canvas = canvas;
//    }

//    public void SelectPencilTool()
//    {
//        _currentTool = _toolFactory.CreatePencilTool();
//    }

//    public void SelectRectangleTool()
//    {
//        _currentTool = _toolFactory.CreateRectangleTool();
//    }

//    public void SelectEllipseTool()
//    {
//        _currentTool = _toolFactory.CreateEllipseTool();
//    }

//    public void SelectTool(string tool)
//    {
//        _currentState.SelectTool(this, tool);
//    }

//    public void SetColor(Color color)
//    {
//        var command = new SetColorCommand(CanvasManager.Instance, color);
//        CanvasManager.Instance.ExecuteCommand(command);
//    }

//    public void SetFillColor(Color fillColor)
//    {
//        CanvasManager.Instance.CurrentFillColor = fillColor;
//        using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//        {
//            g.Clear(Color.White);
//            CanvasManager.Instance.RedrawAll(g);
//        }
//    }

//    public void Draw(Point start, Point end)
//    {
//        if (_currentTool != null)
//        {
//            var drawable = _currentTool.CreateDrawable(start, end, CanvasManager.Instance.CurrentColor);
//            var command = new AddDrawableCommand(CanvasManager.Instance, drawable);
//            CanvasManager.Instance.ExecuteCommand(command);
//            using (var g = Graphics.FromImage(CanvasManager.Instance.Canvas))
//            {
//                g.Clear(Color.White);
//                CanvasManager.Instance.RedrawAll(g);
//            }
//        }
//    }

//    public IDrawable CreatePreviewDrawable(Point start, Point end)
//    {
//        return _currentTool?.CreateDrawable(start, end, CanvasManager.Instance.CurrentColor);
//    }

//    public void HandleMouseDown(MouseEventArgs e)
//    {
//        _currentState.HandleMouseDown(this, e);
//   }

//    public void HandleMouseMove(MouseEventArgs e, PictureBox canvas)
//    {
//        _currentState.HandleMouseMove(this, e, canvas);
//    }

//    public void HandleMouseUp(MouseEventArgs e)
//    {
//        _currentState.HandleMouseUp(this, e);
//    }
//}