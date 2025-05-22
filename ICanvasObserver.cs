using System.Drawing;

public interface ICanvasObserver
{
    void OnCanvasStateChanged(Color currentColor, Color currentFillColor, IDrawable lastDrawnObject);
}