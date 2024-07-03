using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGames;

public class ConsoleMap
{
    private int width;
    private int height;
    private char[,] buffer;
    private List<Projectile> projectiles;
    private List<Enemy> enemies;
    private Vector2 playerPosition;

    public ConsoleMap(int width, int height)
    {
        this.width = width;
        this.height = height;
        buffer = new char[width, height];
        projectiles = new List<Projectile>();
        enemies = new List<Enemy>();
        Clear();
        PlacePlayer();
    }

    private void PlacePlayer()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (buffer[x, y] == ' ')
                {
                    playerPosition = new Vector2(x, y);
                    DrawPlayer();
                    return;
                }
            }
        }

        throw new Exception("No empty space found for player.");
    }

    public void Clear()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                buffer[x, y] = ' ';
            }
        }
    }

    public void DrawRectangle(Rectangle rect, char c)
    {
        int minX = (int)rect.Min.X;
        int maxX = (int)rect.Max.X;
        int minY = (int)rect.Min.Y;
        int maxY = (int)rect.Max.Y;

        // Dibujar paredes verticales con una apertura en el medio
        int midY = (minY + maxY) / 2;
        for (int y = minY; y <= maxY; y++)
        {
            if (y < 0 || y >= height) continue;
            if (minX >= 0 && minX < width)
            {
                buffer[minX, y] = (y == midY) ? ' ' : c;
            }

            if (maxX >= 0 && maxX < width)
            {
                buffer[maxX, y] = (y == midY) ? ' ' : c;
            }
        }

        // Dibujar paredes horizontales con una apertura en el medio
        int midX = (minX + maxX) / 2;
        for (int x = minX; x <= maxX; x++)
        {
            if (x < 0 || x >= width) continue;
            if (minY >= 0 && minY < height)
            {
                buffer[x, minY] = (x == midX) ? ' ' : c;
            }

            if (maxY >= 0 && maxY < height)
            {
                buffer[x, maxY] = (x == midX) ? ' ' : c;
            }
        }
    }

    public void DrawPlayer()
    {
        int x = (int)playerPosition.X;
        int y = (int)playerPosition.Y;
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            buffer[x, y] = 'P';
        }
    }

    public void DrawEnemies()
    {
        foreach (var enemy in enemies)
        {
            int x = (int)enemy.Position.X;
            int y = (int)enemy.Position.Y;
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                buffer[x, y] = 'E';
            }
        }
    }

    public void DrawProjectiles()
    {
        foreach (var projectile in projectiles)
        {
            int x = (int)projectile.Position.X;
            int y = (int)projectile.Position.Y;
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                buffer[x, y] = '*';
            }
        }
    }

    public void ClearPlayer()
    {
        int x = (int)playerPosition.X;
        int y = (int)playerPosition.Y;
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            buffer[x, y] = ' ';
        }
    }

    public bool IsWall(int x, int y)
    {
        return buffer[x, y] == '#';
    }

    public void MovePlayer(int dx, int dy)
    {
        int newX = (int)(playerPosition.X + dx);
        int newY = (int)(playerPosition.Y + dy);

        if (newX >= 0 && newX < width && newY >= 0 && newY < height && !IsWall(newX, newY))
        {
            ClearPlayer(); // Limpia la posición anterior del jugador
            playerPosition.X = newX;
            playerPosition.Y = newY;
            DrawPlayer(); // Dibuja el jugador en la nueva posición
        }
    }

    public void Shoot(Vector2 direction)
    {
        var projectile = new Projectile(playerPosition, direction);
        projectiles.Add(projectile);
    }

    public void UpdateProjectiles()
    {
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            var projectile = projectiles[i];
            projectile.Update();
            int x = (int)projectile.Position.X;
            int y = (int)projectile.Position.Y;

            if (x < 0 || x >= width || y < 0 || y >= height || IsWall(x, y))
            {
                projectiles.RemoveAt(i);
                continue;
            }

            // Colisión con enemigos
            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                var enemy = enemies[j];
                if ((int)enemy.Position.X == x && (int)enemy.Position.Y == y)
                {
                    enemies.RemoveAt(j);
                    projectiles.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void AddEnemy(Vector2 position)
    {
        enemies.Add(new Enemy(position));
    }

    public void UpdateEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.MoveTowards(playerPosition);
        }
    }

    public void Render()
    {
        Console.Clear();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(buffer[x, y]);
            }

            Console.WriteLine();
        }
    }

    public void Update(BSPNode root)
    {
        Clear();
        DrawBSPNode(root);
        DrawPlayer();
        DrawProjectiles();
        DrawEnemies();
        Render();
    }

    private void DrawBSPNode(BSPNode node)
    {
        if (node == null) return;
        DrawRectangle(node.Region, '#');
        DrawBSPNode(node.Left);
        DrawBSPNode(node.Right);
    }
}

public class BSPNode
{
    public Rectangle Region { get; }
    public BSPNode Left { get; set; }
    public BSPNode Right { get; set; }

    public BSPNode(Rectangle region)
    {
        Region = region;
    }
}

public class BSPTree
{
    public BSPNode Root { get; private set; }

    public BSPTree(Rectangle region)
    {
        Root = new BSPNode(region);
        Subdivide(Root, 4); // Número de subdivisiones
    }

    private void Subdivide(BSPNode node, int depth)
    {
        if (depth <= 0) return;

        bool splitVertically = (depth % 2 == 0);
        var region = node.Region;

        if (splitVertically)
        {
            float midX = (region.Min.X + region.Max.X) / 2;
            node.Left = new BSPNode(new Rectangle(region.Min, new Vector2(midX, region.Max.Y)));
            node.Right = new BSPNode(new Rectangle(new Vector2(midX, region.Min.Y), region.Max));
        }
        else
        {
            float midY = (region.Min.Y + region.Max.Y) / 2;
            node.Left = new BSPNode(new Rectangle(region.Min, new Vector2(region.Max.X, midY)));
            node.Right = new BSPNode(new Rectangle(new Vector2(region.Min.X, midY), region.Max));
        }

        Subdivide(node.Left, depth - 1);
        Subdivide(node.Right, depth - 1);
    }

    public void PrintTree(BSPNode node, string indent = "")
    {
        if (node == null) return;
        Console.WriteLine($"{indent}Region: {node.Region}");
        PrintTree(node.Left, indent + "  ");
        PrintTree(node.Right, indent + "  ");
    }
}