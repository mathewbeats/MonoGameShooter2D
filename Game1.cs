using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGames;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _playerTexture;
    private Texture2D _enemyTexture;
    private Texture2D _projectileTexture;
    private Texture2D _wallTexture;

    private Vector2 _playerPosition;
    private List<Projectile> _projectiles;
    private List<Enemy> _enemies;
    private List<Wall> _walls;

    private Random _random;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _playerPosition = new Vector2(400, 240);
        _projectiles = new List<Projectile>();
        _enemies = new List<Enemy>();
        _walls = new List<Wall>();

        _random = new Random();
        for (int i = 0; i < 5; i++)
        {
            _enemies.Add(new Enemy(new Vector2(_random.Next(800), _random.Next(480))));
        }
        
        
        // Añadir algunas paredes en posiciones aleatorias
        for (int i = 0; i < 5; i++)
        {
            _walls.Add(new Wall(new Vector2(_random.Next(800), _random.Next(480))));
        }
        base.Initialize();
    }

    // protected override void LoadContent()
    // {
    //     _spriteBatch = new SpriteBatch(GraphicsDevice);
    //
    //     string[] textureFiles = { "player", "Projectile", "enemy", "wall" };
    //     foreach (string file in textureFiles)
    //     {
    //         string path = Path.Combine(Content.RootDirectory, file + ".png");
    //         if (!File.Exists(path))
    //         {
    //             Console.WriteLine($"Texture file not found: {path}");
    //         }
    //     }
    //
    //     try
    //     {
    //         _playerTexture = Content.Load<Texture2D>("player");
    //         _projectileTexture = Content.Load<Texture2D>("Projectile");
    //         _enemyTexture = Content.Load<Texture2D>("enemy");
    //         _wallTexture = Content.Load<Texture2D>("wall");
    //        
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Error loading textures: " + ex.Message);
    //         throw;
    //     }
    // }


    private Texture2D CreateRectangleTexture(GraphicsDevice graphicsDevice, int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(graphicsDevice, width, height);
        Color[] data = new Color[width * height];
        for (int i = 0; i < data.Length; ++i) data[i] = color;
        texture.SetData(data);
        return texture;
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // try
        // {
        //     // Intenta cargar solo una textura para depuración
        //     _projectileTexture = Content.Load<Texture2D>("Projectile");
        //     Console.WriteLine("Projectile texture loaded successfully.");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("Error loading Projectile texture: " + ex.Message);
        //     throw;
        // }

        // Crear una textura básica para el proyectil
        _projectileTexture = CreateRectangleTexture(GraphicsDevice, 5, 5, Color.Yellow);
        Console.WriteLine("Projectile texture created successfully.");
        
        
        // Crear una textura básica para las paredes
        _wallTexture = CreateRectangleTexture(GraphicsDevice, 32, 32, Color.Gray);
        Console.WriteLine("Wall texture created successfully.");

        // Intenta cargar otras texturas de manera similar
        try
        {
            _playerTexture = Content.Load<Texture2D>("player");
            Console.WriteLine("Player texture loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading Player texture: " + ex.Message);
            throw;
        }
   

        try
        {
            _enemyTexture = Content.Load<Texture2D>("enemy");
            Console.WriteLine("Enemy texture loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading Enemy texture: " + ex.Message);
            throw;
        }


        // try
        // {
        //     _wallTexture = Content.Load<Texture2D>("wall");
        //     Console.WriteLine("Wall texture loaded successfully.");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("Error loading Wall texture: " + ex.Message);
        //     throw;
        // }


        // Continúa con las demás texturas
    }

    // protected override void Update(GameTime gameTime)
    // {
    //     if (Keyboard.GetState().IsKeyDown(Keys.Escape))
    //         Exit();
    //
    //     // Movimiento del jugador
    //     var keyboardState = Keyboard.GetState();
    //     Vector2 previousPlayerPosition = _playerPosition; // Definir la posición anterior del jugador
    //     if (keyboardState.IsKeyDown(Keys.W))
    //         _playerPosition.Y -= 2;
    //     if (keyboardState.IsKeyDown(Keys.S))
    //         _playerPosition.Y += 2;
    //     if (keyboardState.IsKeyDown(Keys.A))
    //         _playerPosition.X -= 2;
    //     if (keyboardState.IsKeyDown(Keys.D))
    //         _playerPosition.X += 2;
    //     
    //     
    //     // Detección de colisiones del jugador con las paredes
    //     var playerRectangle = new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32);
    //     foreach (var wall in _walls)
    //     {
    //         var wallRectangle = new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
    //         if (playerRectangle.Intersects(wallRectangle))
    //         {
    //             _playerPosition = previousPlayerPosition;
    //             break;
    //         }
    //     }
    //
    //     // Disparos
    //     if (keyboardState.IsKeyDown(Keys.Up))
    //         _projectiles.Add(new Projectile(_playerPosition, new Vector2(0, -5)));
    //     if (keyboardState.IsKeyDown(Keys.Down))
    //         _projectiles.Add(new Projectile(_playerPosition, new Vector2(0, 5)));
    //     if (keyboardState.IsKeyDown(Keys.Left))
    //         _projectiles.Add(new Projectile(_playerPosition, new Vector2(-5, 0)));
    //     if (keyboardState.IsKeyDown(Keys.Right))
    //         _projectiles.Add(new Projectile(_playerPosition, new Vector2(5, 0)));
    //
    //     // // Actualizar posición de los disparos
    //     // for (int i = _projectiles.Count - 1; i >= 0; i--)
    //     // {
    //     //     _projectiles[i].Update();
    //     //     if (_projectiles[i].Position.X < 0 || _projectiles[i].Position.X > 800 ||
    //     //         _projectiles[i].Position.Y < 0 || _projectiles[i].Position.Y > 480)
    //     //     {
    //     //         _projectiles.RemoveAt(i);
    //     //     }
    //     // }
    //     
    //     // Actualizar posición de los disparos y detectar colisiones con las paredes
    //     
    //     
    //     for (int i = _projectiles.Count - 1; i >= 0; i--)
    //     {
    //         _projectiles[i].Update();
    //         var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[i].Position.X, (int)_projectiles[i].Position.Y, 5, 5);
    //
    //         // Verificar colisión con paredes
    //         bool collidedWithWall = false;
    //         foreach (var wall in _walls)
    //         {
    //             var wallRectangle = new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
    //             if (projectileRectangle.Intersects(wallRectangle))
    //             {
    //                 collidedWithWall = true;
    //                 break;
    //             }
    //         }
    //
    //         if (collidedWithWall || 
    //             _projectiles[i].Position.X < 0 || _projectiles[i].Position.X > 800 || 
    //             _projectiles[i].Position.Y < 0 || _projectiles[i].Position.Y > 480)
    //         {
    //             _projectiles.RemoveAt(i);
    //         }
    //     }
    //
    //     // Detección de colisiones y actualización de enemigos
    //     for (int i = _enemies.Count - 1; i >= 0; i--)
    //     {
    //         var enemyRectangle =
    //             new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X, (int)_enemies[i].Position.Y, 32, 32);
    //         bool isHit = false;
    //         for (int j = _projectiles.Count - 1; j >= 0; j--)
    //         {
    //             var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[j].Position.X,
    //                 (int)_projectiles[j].Position.Y, 5, 5);
    //             if (enemyRectangle.Intersects(projectileRectangle))
    //             {
    //                 _projectiles.RemoveAt(j);
    //                 isHit = true;
    //             }
    //         }
    //
    //         if (isHit)
    //         {
    //             _enemies.RemoveAt(i);
    //         }
    //         else
    //         {
    //             _enemies[i].MoveTowards(_playerPosition);
    //         }
    //     }
    //
    //     base.Update(gameTime);
    // }

protected override void Update(GameTime gameTime)
{
    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();

    // Movimiento del jugador
    var keyboardState = Keyboard.GetState();
    Vector2 previousPlayerPosition = _playerPosition; // Definir la posición anterior del jugador
    if (keyboardState.IsKeyDown(Keys.W))
        _playerPosition.Y -= 2;
    if (keyboardState.IsKeyDown(Keys.S))
        _playerPosition.Y += 2;
    if (keyboardState.IsKeyDown(Keys.A))
        _playerPosition.X -= 2;
    if (keyboardState.IsKeyDown(Keys.D))
        _playerPosition.X += 2;

    // Detección de colisiones del jugador con las paredes
    var playerRectangle = new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32);
    foreach (var wall in _walls)
    {
        var wallRectangle = new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
        if (playerRectangle.Intersects(wallRectangle))
        {
            _playerPosition = previousPlayerPosition;
            break;
        }
    }

    // Disparos
    if (keyboardState.IsKeyDown(Keys.Up))
        _projectiles.Add(new Projectile(_playerPosition, new Vector2(0, -5)));
    if (keyboardState.IsKeyDown(Keys.Down))
        _projectiles.Add(new Projectile(_playerPosition, new Vector2(0, 5)));
    if (keyboardState.IsKeyDown(Keys.Left))
        _projectiles.Add(new Projectile(_playerPosition, new Vector2(-5, 0)));
    if (keyboardState.IsKeyDown(Keys.Right))
        _projectiles.Add(new Projectile(_playerPosition, new Vector2(5, 0)));

    // Actualizar posición de los disparos y detectar colisiones con las paredes
    for (int i = _projectiles.Count - 1; i >= 0; i--)
    {
        _projectiles[i].Update();
        var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[i].Position.X, (int)_projectiles[i].Position.Y, 5, 5);

        // Verificar colisión con paredes
        bool collidedWithWall = false;
        foreach (var wall in _walls)
        {
            var wallRectangle = new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
            if (projectileRectangle.Intersects(wallRectangle))
            {
                collidedWithWall = true;
                break;
            }
        }

        if (collidedWithWall || 
            _projectiles[i].Position.X < 0 || _projectiles[i].Position.X > 800 || 
            _projectiles[i].Position.Y < 0 || _projectiles[i].Position.Y > 480)
        {
            _projectiles.RemoveAt(i);
        }
    }

    // Detección de colisiones y actualización de enemigos
    for (int i = _enemies.Count - 1; i >= 0; i--)
    {
        Vector2 previousEnemyPosition = _enemies[i].Position; // Guardar la posición anterior del enemigo
        var enemyRectangle = new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X, (int)_enemies[i].Position.Y, 32, 32);
        bool isHit = false;
        for (int j = _projectiles.Count - 1; j >= 0; j--)
        {
            var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[j].Position.X, (int)_projectiles[j].Position.Y, 5, 5);
            if (enemyRectangle.Intersects(projectileRectangle))
            {
                _projectiles.RemoveAt(j);
                isHit = true;
                break;
            }
        }

        if (isHit)
        {
            _enemies.RemoveAt(i);
        }
        else
        {
            _enemies[i].MoveTowards(_playerPosition);

            // Detección de colisiones del enemigo con las paredes
            enemyRectangle = new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X, (int)_enemies[i].Position.Y, 32, 32);
            foreach (var wall in _walls)
            {
                var wallRectangle = new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
                if (enemyRectangle.Intersects(wallRectangle))
                {
                    _enemies[i].Position = previousEnemyPosition;
                    break;
                }
            }
        }
    }

    base.Update(gameTime);
}

    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Dibujar jugador
        _spriteBatch.Draw(_playerTexture,
            new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32), Color.White);

        // Dibujar disparos
        foreach (var projectile in _projectiles)
        {
            _spriteBatch.Draw(_projectileTexture,
                new Microsoft.Xna.Framework.Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, 5, 5),
                Color.Yellow);
        }

        // Dibujar enemigos
        foreach (var enemy in _enemies)
        {
            _spriteBatch.Draw(_enemyTexture,
                new Microsoft.Xna.Framework.Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 32, 32), Color.Red);
        }
        
        // Dibujar paredes
        foreach (var wall in _walls)
        {
            _spriteBatch.Draw(_wallTexture, new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32), Color.Brown);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}


public class Wall
{
    
    public Vector2 Position { get; private set; }

    public Wall(Vector2 position)
    {

        Position = position;

    }
}