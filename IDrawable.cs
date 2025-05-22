using System.Drawing;

public interface IDrawable
{
    Color Color { get; set; }
    Point StartPoint { get; set; }
    Point EndPoint { get; set; }
    void Draw(Graphics g);
    IDrawable Clone();
    IDrawable CloneWithOffset();
}
//using System.Drawing;

//public interface IDrawable
//{
//    Point StartPoint { get; set; }
//    Point EndPoint { get; set; }
//    Color Color { get; set; }
//    void Draw(Graphics g);
//    IDrawable Clone();
//    IDrawable CloneWithOffset();
//}