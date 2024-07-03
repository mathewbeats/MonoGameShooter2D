using Microsoft.Xna.Framework;

namespace MonoGames;

public struct Rectangle
{
    public Vector2 Min { get; }
    public Vector2 Max { get; }

    public Rectangle(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString() => $"({Min.X}, {Min.Y}) - ({Max.X}, {Max.Y})";
}