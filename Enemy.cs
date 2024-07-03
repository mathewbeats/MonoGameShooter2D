using System;
using Microsoft.Xna.Framework;

namespace MonoGames;

public class Enemy
{
    public Vector2 Position { get; set; }
    private int moveCounter = 0;

    public Enemy(Vector2 position)
    {
        Position = position;
    }

    public void MoveTowards(Vector2 target)
    {
        if (moveCounter % 3 == 0)
        {
            Vector2 direction = new Vector2(target.X - Position.X, target.Y - Position.Y);
            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                direction.X = Math.Sign(direction.X);
                direction.Y = 0;
            }
            else
            {
                direction.X = 0;
                direction.Y = Math.Sign(direction.Y);
            }

            Position += direction;
        }
        moveCounter++;
    }
}