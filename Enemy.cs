using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGames;
//
// public class Enemy
// {
//     public Vector2 Position { get; set; }
//     private int moveCounter = 0;
//
//     public Enemy(Vector2 position)
//     {
//         Position = position;
//     }
//
//     public void MoveTowards(Vector2 target)
//     {
//         if (moveCounter % 3 == 0)
//         {
//             Vector2 direction = new Vector2(target.X - Position.X, target.Y - Position.Y);
//             if (Math.Abs(direction.X) > Math.Abs(direction.Y))
//             {
//                 direction.X = Math.Sign(direction.X);
//                 direction.Y = 0;
//             }
//             else
//             {
//                 direction.X = 0;
//                 direction.Y = Math.Sign(direction.Y);
//             }
//
//             Position += direction;
//         }
//         moveCounter++;
//     }
// }

public class Enemy
{
    public Vector2 Position { get; set; }
    private AnimatedSprite _animatedSprite;
    
    private int moveCounter = 0;

    public Enemy(Vector2 position, AnimatedSprite animatedSprite)
    {
        Position = position;
        _animatedSprite = animatedSprite;
    }

    public void Update(GameTime gameTime, Vector2 playerPosition)
    {
        _animatedSprite.Update(gameTime);
        MoveTowards(playerPosition);
    }

    // public void MoveTowards(Vector2 targetPosition)
    // {
    //     var direction = Vector2.Normalize(targetPosition - Position);
    //     Position += direction * 1f; // Adjust the speed as needed
    // }
    
    // public void MoveTowards(Vector2 target)
    //  {
    //      if (moveCounter % 3 == 0)
    //      {
    //          Vector2 direction = new Vector2(target.X - Position.X, target.Y - Position.Y);
    //          if (Math.Abs(direction.X) > Math.Abs(direction.Y))
    //          {
    //              direction.X = Math.Sign(direction.X);
    //              direction.Y = 0;
    //          }
    //          else
    //          {
    //              direction.X = 0;
    //              direction.Y = Math.Sign(direction.Y);
    //          }
    //
    //          Position += direction * 1f;
    //      }
    //      moveCounter++;
    //  }
    
    
    public void MoveTowards(Vector2 target)
    {
        if (moveCounter % 3 == 0)
        {
            Vector2 direction = new Vector2(target.X - Position.X, target.Y - Position.Y);
            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                if (direction.X > 0)
                {
                    _animatedSprite.SetRow(2); // Moviendo a la derecha
                }
                else
                {
                    _animatedSprite.SetRow(3); // Moviendo a la izquierda
                }
                direction.X = Math.Sign(direction.X);
                direction.Y = 0;
            }
            else
            {
                if (direction.Y > 0)
                {
                    _animatedSprite.SetRow(0); // Moviendo hacia abajo
                }
                else
                {
                    _animatedSprite.SetRow(1); // Moviendo hacia arriba
                }
                direction.X = 0;
                direction.Y = Math.Sign(direction.Y);
            }

            Position += direction * 1f;
        }
        moveCounter++;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _animatedSprite.Draw(spriteBatch, Position);
    }
}
