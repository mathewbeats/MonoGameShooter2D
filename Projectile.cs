using Microsoft.Xna.Framework;

namespace MonoGames;

public class Projectile
{
    public Vector2 Position { get; private set; }
    private Vector2 _velocity;

    public Projectile(Vector2 position, Vector2 velocity)
    {
        Position = position;
        _velocity = velocity;
    }

    public void Update()
    {
        Position += _velocity;
    }
}