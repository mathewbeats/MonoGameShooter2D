using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoGames;

public enum GameState
{
    Outside,
    InsideCave
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //private Texture2D _playerTexture;
    private Texture2D _enemyTexture;
    private Texture2D _projectileTexture;
    private Texture2D _wallTexture;
    private Texture2D _treeTexture;
    private Texture2D _backgroundTexture;
    private Texture2D _caveTexture;
    private Texture2D _caveInteriorTexture;

    private Texture2D _enemySpriteSheet;

    private Texture2D _playerSpriteSheet;


    private Vector2 _playerPosition;
    private List<Projectile> _projectiles;
    private List<Enemy> _enemies;
    private List<Wall> _walls;
    private List<Tree2> _tree;

    private AnimatedSprite _playerAnimatedSprite;

    private Random _random;

    private Song _backgroundMusic;

    private GameState _currentGameState;

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
        _tree = new List<Tree2>();

        _random = new Random();
        // for (int i = 0; i < 20; i++)
        // {
        //     _enemies.Add(new Enemy(new Vector2(_random.Next(800), _random.Next(480))));
        // }

        // Agregar algunas paredes en posiciones aleatorias
        for (int i = 0; i < 8; i++)
        {
            _walls.Add(new Wall(new Vector2(_random.Next(800), _random.Next(480))));
        }

        for (int i = 0; i < 10; i++)
        {
            _tree.Add(new Tree2(new Vector2(_random.Next(800), _random.Next(480))));
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
        //     _playerTexture = Content.Load<Texture2D>("player");
        //     Console.WriteLine("Player texture loaded successfully.");
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("Error loading Player texture: " + ex.Message);
        //     throw;
        // }

        try
        {
            _playerSpriteSheet = Content.Load<Texture2D>("walk - sword shield");
            _playerAnimatedSprite = new AnimatedSprite(_playerSpriteSheet, 4, 4, 10); // 4 rows, 4 columns, 10 fps
            Console.WriteLine("Player sprite sheet loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading player sprite sheet: " + ex.Message);
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

        //new enemy
        _enemySpriteSheet = Content.Load<Texture2D>("attack - pitchfork shield");

        for (int i = 0; i < 5; i++)
        {
            var animatedSprite = new AnimatedSprite(_enemySpriteSheet, 4, 4, 10); // 4 rows, 4 columns, 10 fps
            _enemies.Add(new Enemy(new Vector2(_random.Next(800), _random.Next(480)), animatedSprite));
        }


        //-----------------------


        try
        {
            _treeTexture = Content.Load<Texture2D>("tree2");
            Console.WriteLine("Tree2 texture loaded successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading tree texture" + e.Message);
            throw;
        }

        try
        {
            _projectileTexture = Content.Load<Texture2D>("flames");

            Console.WriteLine("projectile was loaded succesfully");
        }
        catch (Exception e)
        {
            Console.WriteLine("projectile cant be loaded" + e.Message);
            throw;
        }

        // Cargar la música de fondo
        try
        {
            _backgroundMusic = Content.Load<Song>("17");
            MediaPlayer.IsRepeating = true; // Repetir la música automáticamente
            MediaPlayer.Play(_backgroundMusic);
            Console.WriteLine("Background music loaded and playing.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading background music: " + ex.Message);
            throw;
        }


        try
        {
            _backgroundTexture = Content.Load<Texture2D>("PC Computer - Railroad Tycoon 3 - Ground Texture 2");
            Console.WriteLine("Background texture loaded");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading background texture: " + e.Message);
            throw;
        }


        try
        {
            _caveTexture = Content.Load<Texture2D>("cave (1)");
            Console.WriteLine("Cave texture loaded");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading cave texture: " + e.Message);
            throw;
        }


        try
        {
            _caveInteriorTexture = Content.Load<Texture2D>("map_2");
            Console.WriteLine("Cave interior texture loaded");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading cave interior texture: " + e.Message);
            throw;
        }

        // Crear una textura básica para las paredes
        _wallTexture = CreateRectangleTexture(GraphicsDevice, 32, 32, Color.Gray);
        Console.WriteLine("Wall texture created successfully.");

        // // Crear una textura básica para el proyectil
        // _projectileTexture = CreateRectangleTexture(GraphicsDevice, 5, 5, Color.Yellow);
        // Console.WriteLine("Projectile texture created successfully.");
    }


    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Movimiento del jugador
        var keyboardState = Keyboard.GetState();


        if (_currentGameState == GameState.Outside)
        {
            Vector2 previousPlayerPosition = _playerPosition; // Definir la posición anterior del jugador
            bool isMoving = false;


            if (keyboardState.IsKeyDown(Keys.W))
            {
                _playerPosition.Y -= 2;
                _playerAnimatedSprite.SetRow(0); // Fila 1 (de espaldas)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                _playerPosition.Y += 2;
                _playerAnimatedSprite.SetRow(2); // Fila 3 (de frente)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                _playerPosition.X -= 2;
                _playerAnimatedSprite.SetRow(1); // Fila 2 (hacia la izquierda)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                _playerPosition.X += 2;
                _playerAnimatedSprite.SetRow(3); // Fila 4 (hacia la derecha)
                isMoving = true;
            }

            if (isMoving)
            {
                _playerAnimatedSprite.Update(gameTime);
            }


            // _playerAnimatedSprite.Update(gameTime);

            // Detección de colisiones del jugador con las paredes
            var playerRectangle =
                new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32);
            foreach (var wall in _walls)
            {
                var wallRectangle =
                    new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
                if (playerRectangle.Intersects(wallRectangle))
                {
                    _playerPosition = previousPlayerPosition;
                    break;
                }
            }

            // Detección de colisiones del jugador con la entrada de la cueva
            var caveEntranceRectangle =
                new Microsoft.Xna.Framework.Rectangle(200, 100, 32,
                    32); // Actualiza esto con la posición correcta de la entrada de la cueva
            if (playerRectangle.Intersects(caveEntranceRectangle))
            {
                _currentGameState = GameState.InsideCave;
                _playerPosition = new Vector2(400, 240); // Nueva posición del jugador dentro de la cueva
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
                var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[i].Position.X,
                    (int)_projectiles[i].Position.Y, 5, 5);

                // Verificar colisión con paredes
                bool collidedWithWall = false;
                foreach (var wall in _walls)
                {
                    var wallRectangle =
                        new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
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
                var enemyRectangle =
                    new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X, (int)_enemies[i].Position.Y, 32,
                        32);
                bool isHit = false;
                for (int j = _projectiles.Count - 1; j >= 0; j--)
                {
                    var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[j].Position.X,
                        (int)_projectiles[j].Position.Y, 5, 5);
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
                    _enemies[i].Update(gameTime, _playerPosition);

                    // _enemies[i].MoveTowards(_playerPosition);

                    // Detección de colisiones del enemigo con las paredes
                    enemyRectangle = new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X,
                        (int)_enemies[i].Position.Y, 32, 32);
                    foreach (var wall in _walls)
                    {
                        var wallRectangle =
                            new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
                        if (enemyRectangle.Intersects(wallRectangle))
                        {
                            _enemies[i].Position = previousEnemyPosition;
                            break;
                        }
                    }
                }
            }
        }
        else if (_currentGameState == GameState.InsideCave)
        {
             Vector2 previousPlayerPosition = _playerPosition; // Definir la posición anterior del jugador
            bool isMoving = false;


            if (keyboardState.IsKeyDown(Keys.W))
            {
                _playerPosition.Y -= 2;
                _playerAnimatedSprite.SetRow(0); // Fila 1 (de espaldas)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                _playerPosition.Y += 2;
                _playerAnimatedSprite.SetRow(2); // Fila 3 (de frente)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                _playerPosition.X -= 2;
                _playerAnimatedSprite.SetRow(1); // Fila 2 (hacia la izquierda)
                isMoving = true;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                _playerPosition.X += 2;
                _playerAnimatedSprite.SetRow(3); // Fila 4 (hacia la derecha)
                isMoving = true;
            }

            if (isMoving)
            {
                _playerAnimatedSprite.Update(gameTime);
            }


            // _playerAnimatedSprite.Update(gameTime);

            // Detección de colisiones del jugador con las paredes
            var playerRectangle =
                new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32);
            foreach (var wall in _walls)
            {
                var wallRectangle =
                    new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
                if (playerRectangle.Intersects(wallRectangle))
                {
                    _playerPosition = previousPlayerPosition;
                    break;
                }
            }

            // Detección de colisiones del jugador con la entrada de la cueva
            var caveEntranceRectangle =
                new Microsoft.Xna.Framework.Rectangle(200, 100, 32,
                    32); // Actualiza esto con la posición correcta de la entrada de la cueva
            if (playerRectangle.Intersects(caveEntranceRectangle))
            {
                _currentGameState = GameState.InsideCave;
                _playerPosition = new Vector2(400, 240); // Nueva posición del jugador dentro de la cueva
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
                var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[i].Position.X,
                    (int)_projectiles[i].Position.Y, 5, 5);

                // Verificar colisión con paredes
                bool collidedWithWall = false;
                foreach (var wall in _walls)
                {
                    var wallRectangle =
                        new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
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
                var enemyRectangle =
                    new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X, (int)_enemies[i].Position.Y, 32,
                        32);
                bool isHit = false;
                for (int j = _projectiles.Count - 1; j >= 0; j--)
                {
                    var projectileRectangle = new Microsoft.Xna.Framework.Rectangle((int)_projectiles[j].Position.X,
                        (int)_projectiles[j].Position.Y, 5, 5);
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
                    _enemies[i].Update(gameTime, _playerPosition);

                    // _enemies[i].MoveTowards(_playerPosition);

                    // Detección de colisiones del enemigo con las paredes
                    enemyRectangle = new Microsoft.Xna.Framework.Rectangle((int)_enemies[i].Position.X,
                        (int)_enemies[i].Position.Y, 32, 32);
                    foreach (var wall in _walls)
                    {
                        var wallRectangle =
                            new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32);
                        if (enemyRectangle.Intersects(wallRectangle))
                        {
                            _enemies[i].Position = previousEnemyPosition;
                            break;
                        }
                    }
                }
            }

        }


        base.Update(gameTime);
    }


    // protected override void Draw(GameTime gameTime)
    // {
    //     GraphicsDevice.Clear(Color.CornflowerBlue);
    //
    //     _spriteBatch.Begin();
    //
    //
    //     //dibujar fondo
    //
    //     _spriteBatch.Draw(_backgroundTexture,
    //         new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480), Color.White);
    //
    //
    //     // Dibujar jugador
    //     _playerAnimatedSprite.Draw(_spriteBatch, _playerPosition);
    //     // // Dibujar jugador
    //     // _spriteBatch.Draw(_playerTexture,
    //     //     new Microsoft.Xna.Framework.Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, 32, 32), Color.White);
    //
    //     // Dibujar disparos
    //     foreach (var projectile in _projectiles)
    //     {
    //         _spriteBatch.Draw(_projectileTexture,
    //             new Microsoft.Xna.Framework.Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, 5, 5),
    //             Color.Yellow);
    //     }
    //
    //     // Dibujar enemigos
    //     // foreach (var enemy in _enemies)
    //     // {
    //     //     _spriteBatch.Draw(_enemyTexture,
    //     //         new Microsoft.Xna.Framework.Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 32, 32), Color.Red);
    //     // }
    //
    //
    //     foreach (var enemy in _enemies)
    //     {
    //         enemy.Draw(_spriteBatch);
    //     }
    //
    //     // Dibujar paredes
    //     foreach (var wall in _walls)
    //     {
    //         _spriteBatch.Draw(_wallTexture,
    //             new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32), Color.Gray);
    //     }
    //
    //     //bibujar arboles
    //
    //     foreach (var tree in _tree)
    //     {
    //         _spriteBatch.Draw(_treeTexture,
    //             new Microsoft.Xna.Framework.Rectangle((int)tree.Position.X, (int)tree.Position.Y, 32, 32), Color.Green);
    //     }
    //
    //     _spriteBatch.End();
    //
    //     base.Draw(gameTime);
    // }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        if (_currentGameState == GameState.Outside)
        {
            // Dibujar fondo
            _spriteBatch.Draw(_backgroundTexture,
                new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480), Color.White);

            // Dibujar jugador
            _playerAnimatedSprite.Draw(_spriteBatch, _playerPosition);

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
                enemy.Draw(_spriteBatch);
            }

            // Dibujar paredes
            foreach (var wall in _walls)
            {
                _spriteBatch.Draw(_wallTexture,
                    new Microsoft.Xna.Framework.Rectangle((int)wall.Position.X, (int)wall.Position.Y, 32, 32),
                    Color.Gray);
            }

            // Dibujar arboles
            foreach (var tree in _tree)
            {
                _spriteBatch.Draw(_treeTexture,
                    new Microsoft.Xna.Framework.Rectangle((int)tree.Position.X, (int)tree.Position.Y, 32, 32),
                    Color.Green);
            }

            // Dibujar entrada de la cueva
            _spriteBatch.Draw(_caveTexture,
                new Microsoft.Xna.Framework.Rectangle(200, 100, 32, 32),
                new Microsoft.Xna.Framework.Rectangle(32, 0, 32, 32), Color.White);
        }
        else if (_currentGameState == GameState.InsideCave)
        {
            // Dibujar el interior de la cueva
            GraphicsDevice.Clear(Color.Black); // Puedes cambiar esto para que sea el fondo de la cueva
            _spriteBatch.Draw(_caveInteriorTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480), Color.White);

            // Dibujar jugador dentro de la cueva
            _playerAnimatedSprite.Draw(_spriteBatch, _playerPosition);

            // Dibujar disparos dentro de la cueva
            foreach (var projectile in _projectiles)
            {
                _spriteBatch.Draw(_projectileTexture,
                    new Microsoft.Xna.Framework.Rectangle((int)projectile.Position.X, (int)projectile.Position.Y, 5, 5),
                    Color.Yellow);
            }

            // Aquí puedes dibujar otros elementos dentro de la cueva
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

public class Tree2
{
    public Vector2 Position { get; private set; }

    public Tree2(Vector2 position)
    {
        Position = position;
    }
}

public class AnimatedSprite
{
    private Texture2D _texture;
    private int _rows;
    private int _columns;
    private int _currentFrame;
    private int _totalFrames;
    private double _timeSinceLastFrame;
    private double _timePerFrame;
    private int _currentRow;

    public AnimatedSprite(Texture2D texture, int rows, int columns, double frameRate)
    {
        _texture = texture;
        _rows = rows;
        _columns = columns;
        _currentFrame = 0;
        _currentRow = 0;
        _totalFrames = rows * columns;
        _timeSinceLastFrame = 0;
        _timePerFrame = 1 / frameRate;
    }

    public void SetRow(int row)
    {
        if (row < 0 || row >= _rows)
        {
            throw new ArgumentOutOfRangeException(nameof(row), "Row is out of range");
        }

        _currentRow = row;

        _currentFrame = _currentRow * _columns;
    }

    public void Update(GameTime gameTime)
    {
        _timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
        if (_timeSinceLastFrame > _timePerFrame)
        {
            _currentFrame++;
            _currentFrame %= _totalFrames;
            _timeSinceLastFrame -= _timePerFrame;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
        int width = _texture.Width / _columns;
        int height = _texture.Height / _rows;
        int row = (int)((float)_currentFrame / _columns);
        int column = _currentFrame % _columns;

        Microsoft.Xna.Framework.Rectangle sourceRectangle =
            new Microsoft.Xna.Framework.Rectangle(width * column, height * row, width, height);
        Microsoft.Xna.Framework.Rectangle destinationRectangle =
            new Microsoft.Xna.Framework.Rectangle((int)location.X, (int)location.Y, width, height);

        spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
    }
}