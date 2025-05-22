using System;
using System.Drawing;
using System.Linq;

public interface ICommand : ICloneable
{
    void Execute();
    void Undo();
}

public class AddDrawableCommand : ICommand
{
    private readonly CanvasManager _manager;
    private readonly IDrawable _drawable;

    public AddDrawableCommand(CanvasManager manager, IDrawable drawable)
    {
        _manager = manager;
        _drawable = drawable;
    }

    public void Execute()
    {
        _manager.AddDrawable(_drawable);
    }

    public void Undo()
    {
        _manager.AllDrawables.Remove(_drawable);
        _manager.LastDrawnObject = _manager.AllDrawables.LastOrDefault();
        _manager.NotifyObservers();
    }

    public object Clone()
    {
        return new AddDrawableCommand(_manager, _drawable.Clone());
    }
}

public class SetColorCommand : ICommand
{
    private readonly CanvasManager _manager;
    private readonly Color _newColor;
    private readonly Color _previousColor;

    public SetColorCommand(CanvasManager manager, Color newColor)
    {
        _manager = manager;
        _newColor = newColor;
        _previousColor = _manager.CurrentColor;
    }

    public void Execute()
    {
        _manager.CurrentColor = _newColor;
    }

    public void Undo()
    {
        _manager.CurrentColor = _previousColor;
        _manager.NotifyObservers();
    }

    public object Clone()
    {
        return new SetColorCommand(_manager, _newColor);
    }
}
//using System.Drawing;
//using System.Linq;

//public interface ICommand
//{
//    void Execute();
//    void Undo();
//}

//public class AddDrawableCommand : ICommand
//{
//    private readonly CanvasManager _manager;
//    private readonly IDrawable _drawable;

//    public AddDrawableCommand(CanvasManager manager, IDrawable drawable)
//    {
//        _manager = manager;
//        _drawable = drawable;
//    }

//    public void Execute()
//    {
//        _manager.AddDrawable(_drawable);
//    }

//    public void Undo()
//    {
//        _manager.AllDrawables.Remove(_drawable);
//        _manager.LastDrawnObject = _manager.AllDrawables.LastOrDefault();
//        _manager.NotifyObservers();
//    }
//}

//public class SetColorCommand : ICommand
//{
//    private readonly CanvasManager _manager;
//    private readonly Color _newColor;
//    private readonly Color _previousColor;

//    public SetColorCommand(CanvasManager manager, Color newColor)
//    {
//        _manager = manager;
//        _newColor = newColor;
//        _previousColor = _manager.CurrentColor;
//    }

//    public void Execute()
//    {
//        _manager.CurrentColor = _newColor;
//    }

//    public void Undo()
//    {
//        _manager.CurrentColor = _previousColor;
//        _manager.NotifyObservers();
//    }
//}
