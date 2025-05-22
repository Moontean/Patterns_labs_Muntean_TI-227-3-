using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class CanvasMemento
{
    public List<IDrawable> AllDrawables { get; }
    public Color CurrentColor { get; }
    public Color CurrentFillColor { get; }
    public IDrawable LastDrawnObject { get; }
    public IDrawStyleStrategy DrawStyleStrategy { get; }
    public List<ICommand> UndoStack { get; }

    public CanvasMemento(
        List<IDrawable> allDrawables,
        Color currentColor,
        Color currentFillColor,
        IDrawable lastDrawnObject,
        IDrawStyleStrategy drawStyleStrategy)
    {
        AllDrawables = allDrawables.ConvertAll(d => d.Clone());
        CurrentColor = currentColor;
        CurrentFillColor = currentFillColor;
        LastDrawnObject = lastDrawnObject?.Clone();
        DrawStyleStrategy = drawStyleStrategy;
        UndoStack = new List<ICommand>();
    }

    public CanvasMemento(
        List<IDrawable> allDrawables,
        Color currentColor,
        Color currentFillColor,
        IDrawable lastDrawnObject,
        IDrawStyleStrategy drawStyleStrategy,
        Stack<ICommand> undoStack)
    {
        AllDrawables = allDrawables.ConvertAll(d => d.Clone());
        CurrentColor = currentColor;
        CurrentFillColor = currentFillColor;
        LastDrawnObject = lastDrawnObject?.Clone();
        DrawStyleStrategy = drawStyleStrategy;
        UndoStack = undoStack.Reverse().Select(cmd => (ICommand)cmd.Clone()).ToList();
    }
}
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;

//public class CanvasMemento
//{
//    public List<IDrawable> AllDrawables { get; }
//    public Color CurrentColor { get; }
//    public Color CurrentFillColor { get; }
//    public IDrawable LastDrawnObject { get; }
//    public IDrawStyleStrategy DrawStyleStrategy { get; }
//    public List<ICommand> UndoStack { get; }

//    public CanvasMemento(
//        List<IDrawable> allDrawables,
//        Color currentColor,
//        Color currentFillColor,
//        IDrawable lastDrawnObject,
//        IDrawStyleStrategy drawStyleStrategy)
//    {
//        AllDrawables = allDrawables.ConvertAll(d => d.Clone());
//        CurrentColor = currentColor;
//        CurrentFillColor = currentFillColor;
//        LastDrawnObject = lastDrawnObject?.Clone();
//        DrawStyleStrategy = drawStyleStrategy;
//        UndoStack = new List<ICommand>();
//    }

//    public CanvasMemento(
//        List<IDrawable> allDrawables,
//        Color currentColor,
//        Color currentFillColor,
//        IDrawable lastDrawnObject,
//        IDrawStyleStrategy drawStyleStrategy,
//        Stack<ICommand> undoStack)
//    {
//        AllDrawables = allDrawables.ConvertAll(d => d.Clone());
//        CurrentColor = currentColor;
//        CurrentFillColor = currentFillColor;
//        LastDrawnObject = lastDrawnObject?.Clone();
//        DrawStyleStrategy = drawStyleStrategy;
//        UndoStack = new List<ICommand>(undoStack.Reverse());
//    }
//}
