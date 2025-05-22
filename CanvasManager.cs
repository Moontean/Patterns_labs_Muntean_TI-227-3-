using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using SimpleGraphicEditor;

public class CanvasManager
{
    private static CanvasManager _instance;
    private readonly List<ICanvasObserver> _observers = new List<ICanvasObserver>();
    private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
    private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
    private IDrawStyleStrategy _drawStyleStrategy = new SolidLineStrategy();
    private int _savedCommandIndex = -1; // Индекс последней команды на момент Save State
    private CanvasManager() { }

    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CanvasManager();
            return _instance;
        }
    }

    public Bitmap Canvas { get; set; }
    private Color _currentColor = Color.Black;
    private Color _currentFillColor = Color.Transparent;
    public Color SelectedFillColor { get; set; } = Color.Yellow;
    public IDrawable LastDrawnObject { get; set; }
    public List<IDrawable> AllDrawables { get; set; } = new List<IDrawable>();
    private CompositeDrawable _compositeDrawable;

    public Color CurrentColor
    {
        get => _currentColor;
        set
        {
            _currentColor = value;
            NotifyObservers();
        }
    }

    public Color CurrentFillColor
    {
        get => _currentFillColor;
        set
        {
            _currentFillColor = value;
            NotifyObservers();
        }
    }

    public IDrawStyleStrategy DrawStyleStrategy
    {
        get => _drawStyleStrategy;
        set
        {
            _drawStyleStrategy = value ?? new SolidLineStrategy();
            NotifyObservers();
        }
    }

    public void AddObserver(ICanvasObserver observer)
    {
        if (observer != null && !_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void RemoveObserver(ICanvasObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.OnCanvasStateChanged(CurrentColor, CurrentFillColor, LastDrawnObject);
        }
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
        NotifyObservers();
    }

    public void Undo()
    {
        if (_undoStack.Count > 0 && _undoStack.Count > _savedCommandIndex + 1)
        {
            Debug.WriteLine($"Performing Undo: Stack size={_undoStack.Count}, SavedCommandIndex={_savedCommandIndex}");
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            NotifyObservers();
        }
        else
        {
            Debug.WriteLine($"Undo blocked: Stack size={_undoStack.Count}, SavedCommandIndex={_savedCommandIndex}");
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            Debug.WriteLine($"Performing Redo: Stack size={_redoStack.Count}");
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            NotifyObservers();
        }
    }

    public CanvasMemento CreateMemento()
    {
        _savedCommandIndex = _undoStack.Count - 1;
        Debug.WriteLine($"Creating Memento: UndoStack size={_undoStack.Count}, SavedCommandIndex={_savedCommandIndex}");
        return new CanvasMemento(AllDrawables, CurrentColor, CurrentFillColor, LastDrawnObject, DrawStyleStrategy, _undoStack);
    }

    public void RestoreMemento(CanvasMemento memento)
    {
        if (memento == null) return;
        AllDrawables = new List<IDrawable>(memento.AllDrawables.ConvertAll(d => d.Clone()));
        CurrentColor = memento.CurrentColor;
        CurrentFillColor = memento.CurrentFillColor;
        LastDrawnObject = memento.LastDrawnObject?.Clone();
        DrawStyleStrategy = memento.DrawStyleStrategy;
        _compositeDrawable = LastDrawnObject as CompositeDrawable ?? new CompositeDrawable();
        _undoStack.Clear();
        Debug.WriteLine($"Restoring Memento: UndoStack size={memento.UndoStack.Count}");
        foreach (var command in memento.UndoStack)
        {
            _undoStack.Push(command);
        }
        _redoStack.Clear();
        _savedCommandIndex = -1; // Сбрасываем, чтобы Undo мог отменять все команды
        Debug.WriteLine($"Memento Restored: New UndoStack size={_undoStack.Count}, SavedCommandIndex={_savedCommandIndex}");
        NotifyObservers();
    }

    public void AddDrawable(IDrawable drawable)
    {
        if (drawable != null)
        {
            AllDrawables.Add(drawable);
            LastDrawnObject = drawable;
            if (_currentFillColor != Color.Transparent)
            {
                ApplyFillDecorator();
            }
            NotifyObservers();
        }
    }

    public void ApplyFillDecorator()
    {
        if (LastDrawnObject == null) return;

        AllDrawables.Remove(LastDrawnObject);

        var originalDrawable = LastDrawnObject;
        while (originalDrawable is FilledDrawableDecorator filledDecorator)
        {
            originalDrawable = filledDecorator.GetDecoratedDrawable();
        }

        if (CurrentFillColor != Color.Transparent)
        {
            LastDrawnObject = new FilledDrawableDecorator(originalDrawable, CurrentFillColor);
        }
        else
        {
            LastDrawnObject = originalDrawable;
        }

        AllDrawables.Add(LastDrawnObject);
        NotifyObservers();
    }

    public void RedrawAll(Graphics g)
    {
        g.Clear(Color.White);
        foreach (var drawable in AllDrawables)
        {
            if (drawable != null)
            {
                drawable.Draw(g);
            }
        }
    }

    public void AddToComposite()
    {
        if (AllDrawables.Count > 0)
        {
            Debug.WriteLine($"Перед группировкой: {AllDrawables.Count} фигур");

            var newComposite = new CompositeDrawable();
            var allIndividualDrawables = new List<IDrawable>();

            foreach (var drawable in AllDrawables.ToList())
            {
                if (drawable != null)
                {
                    if (drawable is CompositeDrawable composite)
                    {
                        Debug.WriteLine($"Найдена группа с {composite.GetDrawableCount()} фигурами");
                        allIndividualDrawables.AddRange(composite.GetDrawables());
                    }
                    else
                    {
                        Debug.WriteLine("Найдена одиночная фигура");
                        allIndividualDrawables.Add(drawable);
                    }
                }
            }

            foreach (var drawable in allIndividualDrawables)
            {
                if (drawable != null)
                {
                    Debug.WriteLine("Добавляем фигуру в новую группу");
                    newComposite.Add(drawable);
                }
            }

            AllDrawables.Clear();
            _compositeDrawable = newComposite;
            LastDrawnObject = _compositeDrawable;
            AllDrawables.Add(LastDrawnObject);
            Debug.WriteLine($"После группировки: {AllDrawables.Count} фигур, в группе: {_compositeDrawable.GetDrawableCount()}");

            if (_currentFillColor != Color.Transparent)
            {
                ApplyFillDecorator();
            }
            NotifyObservers();
        }
        else
        {
            Debug.WriteLine("Нет фигур для группировки");
        }
    }

    public void ClearComposite()
    {
        _compositeDrawable = new CompositeDrawable();
        LastDrawnObject = null;
        AllDrawables.Clear();
        NotifyObservers();
    }

    public void ClearAll()
    {
        AllDrawables.Clear();
        LastDrawnObject = null;
        NotifyObservers();
    }
}
//using System.Collections.Generic;
//using System.Drawing;
//using System.Diagnostics;
//using System.Linq;
//using SimpleGraphicEditor;

//public class CanvasManager
//{
//    private static CanvasManager _instance;
//    private readonly List<ICanvasObserver> _observers = new List<ICanvasObserver>();
//    private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
//    private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
//    private IDrawStyleStrategy _drawStyleStrategy = new SolidLineStrategy();
//    private int _savedCommandIndex = -1; // Индекс последней команды на момент Save State
//    private CanvasManager() { }

//    public static CanvasManager Instance
//    {
//        get
//        {
//            if (_instance == null)
//                _instance = new CanvasManager();
//            return _instance;
//        }
//    }

//    public Bitmap Canvas { get; set; }
//    private Color _currentColor = Color.Black;
//    private Color _currentFillColor = Color.Transparent;
//    public Color SelectedFillColor { get; set; } = Color.Yellow;
//    public IDrawable LastDrawnObject { get; set; }
//    public List<IDrawable> AllDrawables { get; set; } = new List<IDrawable>();
//    private CompositeDrawable _compositeDrawable;

//    public Color CurrentColor
//    {
//        get => _currentColor;
//        set
//        {
//            _currentColor = value;
//            NotifyObservers();
//        }
//    }

//    public Color CurrentFillColor
//    {
//        get => _currentFillColor;
//        set
//        {
//            _currentFillColor = value;
//            NotifyObservers();
//        }
//    }

//    public IDrawStyleStrategy DrawStyleStrategy
//    {
//        get => _drawStyleStrategy;
//        set
//        {
//            _drawStyleStrategy = value ?? new SolidLineStrategy();
//            NotifyObservers();
//        }
//    }

//    public void AddObserver(ICanvasObserver observer)
//    {
//        if (observer != null && !_observers.Contains(observer))
//            _observers.Add(observer);
//    }

//    public void RemoveObserver(ICanvasObserver observer)
//    {
//        _observers.Remove(observer);
//    }

//    public void NotifyObservers()
//    {
//        foreach (var observer in _observers)
//        {
//            observer.OnCanvasStateChanged(CurrentColor, CurrentFillColor, LastDrawnObject);
//        }
//    }

//    public void ExecuteCommand(ICommand command)
//    {
//        command.Execute();
//        _undoStack.Push(command);
//        _redoStack.Clear();
//        NotifyObservers();
//    }

//    public void Undo()
//    {
//        if (_undoStack.Count > 0 && _undoStack.Count > _savedCommandIndex + 1)
//        {
//            var command = _undoStack.Pop();
//            command.Undo();
//            _redoStack.Push(command);
//            NotifyObservers();
//        }
//    }

//    public void Redo()
//    {
//        if (_redoStack.Count > 0)
//        {
//            var command = _redoStack.Pop();
//            command.Execute();
//            _undoStack.Push(command);
//            NotifyObservers();
//        }
//    }

//    public CanvasMemento CreateMemento()
//    {
//        _savedCommandIndex = _undoStack.Count - 1;
//        return new CanvasMemento(AllDrawables, CurrentColor, CurrentFillColor, LastDrawnObject, DrawStyleStrategy, _undoStack);
//    }

//    public void RestoreMemento(CanvasMemento memento)
//    {
//        if (memento == null) return;
//        AllDrawables = new List<IDrawable>(memento.AllDrawables.ConvertAll(d => d.Clone()));
//        CurrentColor = memento.CurrentColor;
//        CurrentFillColor = memento.CurrentFillColor;
//        LastDrawnObject = memento.LastDrawnObject?.Clone();
//        DrawStyleStrategy = memento.DrawStyleStrategy;
//        _compositeDrawable = LastDrawnObject as CompositeDrawable ?? new CompositeDrawable();
//        _undoStack.Clear();
//        foreach (var command in memento.UndoStack)
//        {
//            _undoStack.Push(command);
//        }
//        _redoStack.Clear();
//        _savedCommandIndex = -1; // Сбрасываем, чтобы Undo мог отменять все команды
//        NotifyObservers();
//    }

//    public void AddDrawable(IDrawable drawable)
//    {
//        if (drawable != null)
//        {
//            AllDrawables.Add(drawable);
//            LastDrawnObject = drawable;
//            if (_currentFillColor != Color.Transparent)
//            {
//                ApplyFillDecorator();
//            }
//            NotifyObservers();
//        }
//    }

//    public void ApplyFillDecorator()
//    {
//        if (LastDrawnObject == null) return;

//        AllDrawables.Remove(LastDrawnObject);

//        var originalDrawable = LastDrawnObject;
//        while (originalDrawable is FilledDrawableDecorator filledDecorator)
//        {
//            originalDrawable = filledDecorator.GetDecoratedDrawable();
//        }

//        if (CurrentFillColor != Color.Transparent)
//        {
//            LastDrawnObject = new FilledDrawableDecorator(originalDrawable, CurrentFillColor);
//        }
//        else
//        {
//            LastDrawnObject = originalDrawable;
//        }

//        AllDrawables.Add(LastDrawnObject);
//        NotifyObservers();
//    }

//    public void RedrawAll(Graphics g)
//    {
//        g.Clear(Color.White);
//        foreach (var drawable in AllDrawables)
//        {
//            if (drawable != null)
//            {
//                drawable.Draw(g);
//            }
//        }
//    }

//    public void AddToComposite()
//    {
//        if (AllDrawables.Count > 0)
//        {
//            Debug.WriteLine($"Перед группировкой: {AllDrawables.Count} фигур");

//            var newComposite = new CompositeDrawable();
//            var allIndividualDrawables = new List<IDrawable>();

//            foreach (var drawable in AllDrawables.ToList())
//            {
//                if (drawable != null)
//                {
//                    if (drawable is CompositeDrawable composite)
//                    {
//                        Debug.WriteLine($"Найдена группа с {composite.GetDrawableCount()} фигурами");
//                        allIndividualDrawables.AddRange(composite.GetDrawables());
//                    }
//                    else
//                    {
//                        Debug.WriteLine("Найдена одиночная фигура");
//                        allIndividualDrawables.Add(drawable);
//                    }
//                }
//            }

//            foreach (var drawable in allIndividualDrawables)
//            {
//                if (drawable != null)
//                {
//                    Debug.WriteLine("Добавляем фигуру в новую группу");
//                    newComposite.Add(drawable);
//                }
//            }

//            AllDrawables.Clear();
//            _compositeDrawable = newComposite;
//            LastDrawnObject = _compositeDrawable;
//            AllDrawables.Add(LastDrawnObject);
//            Debug.WriteLine($"После группировки: {AllDrawables.Count} фигур, в группе: {_compositeDrawable.GetDrawableCount()}");

//            if (_currentFillColor != Color.Transparent)
//            {
//                ApplyFillDecorator();
//            }
//            NotifyObservers();
//        }
//        else
//        {
//            Debug.WriteLine("Нет фигур для группировки");
//        }
//    }

//    public void ClearComposite()
//    {
//        _compositeDrawable = new CompositeDrawable();
//        LastDrawnObject = null;
//        AllDrawables.Clear();
//        NotifyObservers();
//    }

//    public void ClearAll()
//    {
//        AllDrawables.Clear();
//        LastDrawnObject = null;
//        NotifyObservers();
//    }
//}
