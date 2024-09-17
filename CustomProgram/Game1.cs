using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceDefenders.Classes;
using System;

namespace SpaceDefenders
{
    public class Game1 : Game
    {
        private static Game1 _instance; // Singleton instance
                                        // Used to allow other classes to access the variables in the game instance
                                        // TODO: Implement James' suggestion into Projectile 
                                        // Can be used by other classes to get instance properties IF needed
        public static Game1 Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Game1();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        // MonoGame object variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Game Engine Variables
        private ObjectPool _objectPool;
        private InputDelayManager _inputManager;
        private Random _randomGenerator;
        private GameState _gameState;
        private Sound _sound;

        // Drawing Variable
        private DrawBackground _drawBackground;
        private DrawEntity _drawEntity;
        private DrawText _drawFontMonospace;

        // Gameplay Variables
        private Earth _earth;
        private Player _player;
        private bool _playerFireOffsetBool;
        private bool _hasPlayedBoostSound;
        private int _gameScore;
        private int _highScore;

        private float _timeSinceLastSpawn;
        private float _initialSpawnInterval; // Initial spawn interval in seconds
        private float _smallMinSpawnInterval;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //Set properties
            _graphics.PreferredBackBufferWidth = GameConstants.ScreenWidth;
            _graphics.PreferredBackBufferHeight = GameConstants.ScreenHeight;
            Window.Title = GameConstants.GameName;
            IsMouseVisible = true;

            //Set framerate to 120Hz
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 120.00);
        }

        protected override void Initialize()
        {
            InitializeGraphics();
            InitializeGameObjects();
            base.Initialize();
        }

        private void InitializeGraphics()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _drawBackground = new DrawBackground(GraphicsDevice, _spriteBatch);
            _drawEntity = new DrawEntity(GraphicsDevice, _spriteBatch);
            _drawFontMonospace = new DrawText("monospace", Content, _spriteBatch, GraphicsDevice);
        }
        private void InitializeGameObjects()
        {
            // Game engine objects
            _objectPool = new ObjectPool();
            _inputManager = new InputDelayManager();
            _randomGenerator = new Random();
            _gameState = new GameState();
            _sound = new Sound();

            // Gameplay objects
            _player = new Player();
            _earth = new Earth();

            // Gameplay variables
            _playerFireOffsetBool = false;
            _gameScore = 0;
            _highScore = SaveAndLoad.LoadHighScore();
            _initialSpawnInterval = 2f;
            _smallMinSpawnInterval = 0.2f;
            _hasPlayedBoostSound = false;
        }
        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update();

            // Variable used to make entity movement frames dependent
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Log key state
            KeyboardState state = Keyboard.GetState();

            // Read Gamestate inputs
            ReadGameStateInput(state);

            // Run methods when game started
            if (_gameState.GameStart == true)
            {
                // Run Test
                if (_gameState.GameTestMode == true && !_gameState.GamePaused)
                {
                    RunTestInputs(state);
                    MovePlayer(state, elapsedSeconds);
                    Shoot(state);
                    Boost(state);
                    UpdateEntities(elapsedSeconds);

                    // Detect Collisions after values are updated
                    DetectCollisions();
                }

                // Run game
                if (_gameState.GamePaused == false && _gameState.GameTestMode == false)
                {
                    // Entity Updates and control reading
                    MovePlayer(state, elapsedSeconds);
                    Shoot(state);
                    Boost(state);
                    UpdateEntities(elapsedSeconds);

                    // Detect Collisions after values are updated
                    DetectCollisions();

                    // Gameplay methods
                    UpdateAndSpawnEnemies(elapsedSeconds);
                }
            }

            if (_gameState.GameMenu == true && !_gameState.GamePaused && !_gameState.GameStart && !_gameState.GameOver)
            {
                // Run methods when game state is game menu
            }
            base.Update(gameTime);

        }

        // All draw functions in here
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            DrawBackground();
            // Draw when game is started/running
            if (_gameState.GameStart == true)
            {
                DrawAnimation();
                DrawEntity(gameTime);
                DrawUI();

                if (_gameState.GamePaused == true)
                {
                    DrawGamePaused();
                }
            }
            // Draw game menu
            if (_gameState.GameMenu == true)
            {
                DrawGameMenu();
            }

            if (_gameState.GameOver == true)
            {
                DrawGameOver();
            }

            _spriteBatch.End();

            base.Draw(gameTime);

        }
        //-------------------------------------------------------//
        // Handles player movement by setting acceleration
        private void MovePlayer(KeyboardState state, float elapsedSeconds)
        {
            Vector2 acceleration = Vector2.Zero;

            if (state.IsKeyDown(Keys.Up))
            {
                acceleration.Y = -400;
            }
            else if (state.IsKeyDown(Keys.Down))
            {
                acceleration.Y = 400;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                acceleration.X = -400;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                acceleration.X = 400;
            }

            _player.Acceleration = acceleration;
            _player.MoveEntity(elapsedSeconds);

            Vector2 newPosition = _player.Position + _player.Velocity * elapsedSeconds;

            if (!CollisionDetection.IsAtEdge(newPosition, _player.Width, _player.Height))
            {
                _player.Position = newPosition;
            }
            else
            {
                _player.Velocity = Vector2.Zero;
            }
        }
        private void Shoot(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.Space) && !_inputManager.KeyDelayed(Keys.Space))
            {
                Vector2 offset = new Vector2(0, 0);
                // Alternate firing position
                if (_playerFireOffsetBool)
                {
                    offset = new Vector2(-11, 8);
                }
                else
                {
                    offset = new Vector2(27, 8);
                }
                // toggle boolean for alternating fire positions
                _playerFireOffsetBool = !_playerFireOffsetBool;

                Vector2 offsetFirePosition = _player.Position + offset;
                // _objectPool.GivePosition(offsetFirePosition);

                Entity activeProjectile = _objectPool.SpawnEntity<Projectile>();
                _sound.PlaySound(SoundType.Fire, 0.1f);
                activeProjectile.Position = offsetFirePosition;
                _inputManager.DelayKey(Keys.Space, _player.FireRateDelay);
            }
        }
        // Boost the player and play boost sound once
        private void Boost(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.LeftShift))
            {
                _player.IsBoosted = true;

                if (!_hasPlayedBoostSound)
                {
                    _sound.PlaySound(SoundType.Boost, 0.1f);
                    _hasPlayedBoostSound = true;
                }
            }
            else
            {
                _player.IsBoosted = false;

                if (_hasPlayedBoostSound && state.IsKeyUp(Keys.LeftShift))
                {
                    _sound.PlaySound(SoundType.NoBoost, 0.25f);
                }

                _hasPlayedBoostSound = false;
            }
        }
        // Run Detection methods
        private void DetectCollisions()
        {
            EnemyCollisions();
            ProjectileCollisions();
            PowerUpCollisions();
        }
        // Enemy Collisions
        private void EnemyCollisions()
        {
            foreach (Enemy enemy in _objectPool.EnemyPool)
            {
                // Enemy to player collision check
                if (enemy.IsActive && CollisionDetection.IsOverlapping(_player, enemy))
                {
                    Entity activeExplosionEntity = _objectPool.SpawnEntity<Explosion>();
                    activeExplosionEntity.Position = enemy.Position;
                    _sound.PlaySound(SoundType.Explosion, 1f);

                    enemy.EntityDie();

                    _player.TakeDamage(1);
                    _sound.PlaySound(SoundType.DamageTaken, 1f);
                }
                // Edge collision checks
                if (enemy.IsActive && CollisionDetection.IsAtEdge(enemy.Position, enemy.Width, enemy.Height) &&
                    enemy.Position.Y > GameConstants.ScreenHeight / 2)
                {
                    enemy.EntityDie();
                }
                // Enemy to Earth collision checks
                if (enemy.IsActive && CollisionDetection.IsOverlappingCircle(_earth, enemy))
                {
                    Entity activeExplosionEntity = _objectPool.SpawnEntity<Explosion>();
                    activeExplosionEntity.Position = enemy.Position;
                    _sound.PlaySound(SoundType.Explosion, 1f);
                    enemy.EntityDie();
                    _earth.TakeDamage(1);
                    _sound.PlaySound(SoundType.DamageTaken, 1f);

                }
            }
        }
        // Projectile Collisions
        private void ProjectileCollisions()
        {
            // Projectile collision detection with enemy
            foreach (Projectile projectile in _objectPool.ProjectilePool)
            {
                foreach (Enemy enemy in _objectPool.EnemyPool)
                {
                    if (enemy.IsActive && CollisionDetection.IsOverlapping(enemy, projectile))
                    {
                        // Enemy takes damage from projectiles damage
                        enemy.TakeDamage(projectile.Damage);
                        // Spawn Hit effect on projectile location and play hit sound
                        Hit hit = _objectPool.SpawnEntity<Hit>();
                        _sound.PlaySound(SoundType.Hit, 0.05f);
                        hit.Position = new Vector2(projectile.Position.X - 50, projectile.Position.Y);
                        // Remove the projectile Entity
                        projectile.DeactivateEntity();
                        {
                            if (enemy.CheckHealthZero())
                            {
                                // Activate Explosion animation on enemy position and play sound effect
                                Entity activeExplosion = _objectPool.SpawnEntity<Explosion>();
                                activeExplosion.Position = enemy.Position;
                                _sound.PlaySound(SoundType.Explosion, 1f);


                                // Randomly spawn a power up on enemy position
                                int randomnumber = _randomGenerator.Next(0, 100);
                                if (randomnumber > 70)  // Adjust to change spawn rates (higher = lower chance)
                                {
                                    Entity activePowerUp = _objectPool.SpawnEntity<PowerUp>();
                                    activePowerUp.Position = enemy.Position;
                                }

                                enemy.EntityDie();
                                // Increase score
                                IncrementScore();
                            }
                        }
                    }
                }

                // Deactivate projectile if projectile is at the edge of the screen
                if (projectile.IsActive == true && CollisionDetection.IsAtEdge(projectile.Position, projectile.Width, projectile.Height))
                {
                    projectile.DeactivateEntity();
                }
            }
        }
        // PowerUp Collisions
        private void PowerUpCollisions()
        {
            foreach (PowerUp powerup in _objectPool.PowerUpsPool)
            {
                if (powerup.IsActive && CollisionDetection.IsOverlapping(_player, powerup))
                {
                    _player.PowerUp(powerup.Type);
                    _sound.PlaySound(SoundType.PowerUp, 0.6f);
                    powerup.DeactivateEntity();
                }

                if (powerup.IsActive && CollisionDetection.IsAtEdge(powerup.Position, powerup.Width, powerup.Height))
                {
                    powerup.DeactivateEntity();
                }
            }
        }
        // Update all entities when IsActive is true
        private void UpdateEntities(float elapsedSeconds)
        {
            // Update projectiles
            foreach (Projectile projectile in _objectPool.ProjectilePool)
            {
                if (projectile.IsActive == true)
                {
                    projectile.Update(elapsedSeconds);
                }
            }
            // Update enemies
            foreach (Enemy enemy in _objectPool.EnemyPool)
            {
                if (enemy.IsActive == true)
                {
                    enemy.Update(elapsedSeconds);
                }
            }
            // Update PowerUps
            foreach (PowerUp powerup in _objectPool.PowerUpsPool)
            {
                if (powerup.IsActive == true)
                {
                    powerup.Update(elapsedSeconds);
                }
            }
            // Update Explosions
            foreach (Explosion explosion in _objectPool.ExplosionPool)
            {
                if (explosion.IsActive == true)
                {
                    explosion.Update(elapsedSeconds);

                    if (explosion.Animation.UpdateFrames() == true)
                    {
                        explosion.DeactivateEntity();
                    }
                }
            }
            // Update Hit
            foreach (Hit hit in _objectPool.HitPool)
            {
                if (hit.IsActive == true)
                {
                    hit.Update(elapsedSeconds);
                    if (hit.Animation.UpdateFrames() == true)
                    {
                        hit.DeactivateEntity();
                    }
                }

            }

            // Update only when boosted
            foreach (Thrusters thrusters in _objectPool.ThrustersPool)
            {
                if (_player.IsBoosted && _player.Fuel > 0)
                {
                    thrusters.Position = new Vector2(_player.Position.X + 60, _player.Position.Y - 10);
                    thrusters.Update(elapsedSeconds);
                }
            }

            _earth.Update(elapsedSeconds);
            _player.Update(elapsedSeconds);

            if (_player.CheckHealthZero() && _gameState.GameStart == true && _gameState.GameTestMode == false)
            {
                _player.EntityDie();
                GameOver();
            }

            if (_earth.CheckHealthZero() && _gameState.GameStart == true && _gameState.GameTestMode == false)
            {
                _player.EntityDie();
                GameOver();
            }
        }
        // Increase game score
        private void IncrementScore()
        {
            _gameScore++;
        }
        // Draw entities and active entities
        private void DrawEntity(GameTime gameTime)
        {
            foreach (Enemy activeEntity in _objectPool.FindActive<Enemy>())
            {
                if (activeEntity.IsBigEnemy == true)
                {
                    _drawEntity.Draw(activeEntity.DrawingType, activeEntity.Position, activeEntity.RotationAngle, 0.25f);
                    DrawHealthBar(activeEntity.Health, activeEntity.Position.X - 10, activeEntity.Position.Y + 80, 15, 5, Color.Red);

                }
                else
                {
                    _drawEntity.Draw(activeEntity.DrawingType, activeEntity.Position, activeEntity.RotationAngle, 0.15f);
                    DrawHealthBar(activeEntity.Health, activeEntity.Position.X + 12, activeEntity.Position.Y + 60, 15, 5, Color.Red);
                }
            }


            foreach (Projectile activeEntity in _objectPool.FindActive<Projectile>())
            {
                _drawEntity.Draw(activeEntity.DrawingType, activeEntity.Position, 0.7f);
            }

            foreach (PowerUp activeEntity in _objectPool.FindActive<PowerUp>())
            {
                _drawEntity.Draw(activeEntity.DrawingType, activeEntity.Position, 1f);
            }

            _drawEntity.Draw(_player.DrawingType, _player.Position, 1.2f);

        }
        // Draw Active animations
        private void DrawAnimation()
        {

            _drawEntity.AnimateEntity(_earth.Animation, _earth.Position, 1f);

            // Find active entities in object pools and draw
            foreach (Explosion activeExplosion in _objectPool.FindActive<Explosion>())
            {
                _drawEntity.AnimateEntity(activeExplosion.Animation, activeExplosion.Position, 1.2f);
            }

            foreach (Hit activeHit in _objectPool.FindActive<Hit>())
            {
                _drawEntity.AnimateEntity(activeHit.Animation, activeHit.Position, 0.20f);
            }

            // Draw only when boosted
            if (_player.IsBoosted && _player.Fuel > 1)
            {
                foreach (Thrusters activeThruster in _objectPool.ThrustersPool)
                {
                    _drawEntity.AnimateEntity(activeThruster.Animation, activeThruster.Position, 2.5f, 1.6f);
                }
            }
        }
        // Draw game UI
        private void DrawUI()
        {
            DrawPlayerUI();
            DrawEarthHealth();
            DrawScore();
        }
        // Draws the player UI
        private void DrawPlayerUI()
        {
            // Draw Health
            _drawFontMonospace.DrawString("HEALTH: ", 60, GameConstants.ScreenHeight - 75, 1.1f);

            int healthOffset = 0;
            for (int i = 0; i < _player.Health; i++)
            {
                _drawEntity.Draw(DrawingType.Health, new Vector2(100 + healthOffset, GameConstants.ScreenHeight - 90), 0.6f);
                healthOffset += 20;
            }

            // Draw Player PowerUps
            _drawFontMonospace.DrawString($"FIRERATE:{_player.FireRateBonusCount}/10", GameConstants.ScreenWidth - 150, GameConstants.ScreenHeight - 20, 1.1f);
            _drawFontMonospace.DrawString($"SPEED:{_player.SpeedBonusCount}/10", GameConstants.ScreenWidth - 172, GameConstants.ScreenHeight - 47, 1.1f);

            // Draw Player Fuel
            _drawFontMonospace.DrawString("FUEL: ", 47, GameConstants.ScreenHeight - 47, 1.1f);
            _drawEntity.DrawRectangle(70, GameConstants.ScreenHeight - 55, _player.Fuel, 15, Color.White);

        }
        // Draws Earth UI (health)
        private void DrawEarthHealth()
        {
            _drawFontMonospace.DrawString("PLANET: ", 60, GameConstants.ScreenHeight - 20, 1.1f);
            int healthOffset = 0;
            for (int i = 0; i < _earth.Health; i++)
            {
                _drawEntity.Draw(DrawingType.EarthHealth, new Vector2(95 + healthOffset, GameConstants.ScreenHeight - 40), 0.15f);
                healthOffset += 20;
            }
        }
        // Used to draw health bars
        private void DrawHealthBar(int health, float xPos, float yPos, float width, float height, Color colour)
        {
            _drawEntity.DrawRectangle(xPos, yPos, (width * health), height, colour);
        }
        // Draw Background (randomises in Background class)
        private void DrawBackground()
        {
            _drawBackground.Draw(Content);
        }
        // Draw the score
        private void DrawScore()
        {
            _drawFontMonospace.DrawString($"SCORE:{_gameScore}", 55, 15, 1.0f);
        }
        // Draw the initial Game menu
        private void DrawGameMenu()
        {
            _drawFontMonospace.DrawString("SPACE DEFENDERS", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 300, 2.0f);
            _drawFontMonospace.DrawString("S - GAME START", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 200, 1.2f);
            _drawFontMonospace.DrawString("T - GAME TEST MODE", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 100, 1.2f);
            _drawFontMonospace.DrawString("ESC - EXIT", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2, 1.2f);
            _drawFontMonospace.DrawString("CURRENT HIGHSCORE", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 + 120, 1.2f);
            _drawFontMonospace.DrawString($"{_highScore}", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 + 200, 2f);

            _drawEntity.DrawRectangle(200, 500, 400, 2, Color.White);
            _drawEntity.DrawSolidRectangle(152, 200, 500, 550, Color.White, 0.2f);
        }
        // Draw menu when paused
        private void DrawGamePaused()
        {
            _drawFontMonospace.DrawString("GAME PAUSED", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 300, 2.0f);
            _drawFontMonospace.DrawString("ESC - RESUME", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 200, 1.2f);
            _drawFontMonospace.DrawString("M - MAIN MENU", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 100, 1.2f);
            _drawFontMonospace.DrawString("Q - QUIT", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2, 1.2f);
            _drawEntity.DrawSolidRectangle(152, 200, 500, 550, Color.White, 0.2f);
        }
        // Draw game over screen
        private void DrawGameOver()
        {
            _drawFontMonospace.DrawString("GAME OVER", GameConstants.ScreenWidth / 2 - 20, GameConstants.ScreenHeight / 2 - 300, 2.0f);
            _drawFontMonospace.DrawString("S - START NEW GAME", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2, 1.2f);
            _drawFontMonospace.DrawString("M - MAIN MENU", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 + 100, 1.2f);
            _drawFontMonospace.DrawString("ESC - EXIT", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 + 200, 1.2f);
            _drawEntity.DrawSolidRectangle(152, 200, 500, 550, Color.White, 0.1f);

            if (_gameScore >= _highScore)
            {
                _drawFontMonospace.DrawString($"NEW HIGHSCORE - {_gameScore} ", GameConstants.ScreenWidth / 2 + 10, GameConstants.ScreenHeight / 2 - 200, 1.2f);
                _drawFontMonospace.DrawString($"YOUR SCORE - {_gameScore} ", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 100, 1.2f);
            }
            else
            {
                _drawFontMonospace.DrawString($"HIGHSCORE - {_highScore} ", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 200, 1.2f);
                _drawFontMonospace.DrawString($"YOUR SCORE - {_gameScore} ", GameConstants.ScreenWidth / 2, GameConstants.ScreenHeight / 2 - 100, 1.2f);
            }
        }
        // Read the players input on menus that change game state
        private void ReadGameStateInput(KeyboardState state)
        {
            // Pause the game if escape is pressed
            if (state.IsKeyDown(Keys.Escape) && !_inputManager.KeyDelayed(Keys.Escape) && _gameState.GameStart == true)
            {
                _gameState.TogglePaused();
                if (_gameState.GamePaused)
                {
                    _sound.PlaySound(SoundType.GameUnpause, 1f);
                }
                else
                {
                    _sound.PlaySound(SoundType.GamePause, 1f);
                }
                _inputManager.DelayKey(Keys.Escape, 30);
            }

            // Return to menu if M is pressed in game paused or game over
            if (state.IsKeyDown(Keys.M) && !_inputManager.KeyDelayed(Keys.M) && (_gameState.GamePaused || _gameState.GameOver))
            {
                _gameState.ReturnToMenu();
                _sound.PlaySound(SoundType.GameEnd, 1f);
                _inputManager.DelayKey(Keys.M, 30);
                ResetGame();
            }

            // Quit if game paused and ESC is pressed
            if (state.IsKeyDown(Keys.Q) && _gameState.GamePaused)
            {
                Exit();
            }

            // Start the game if S is pressed in menu
            if (state.IsKeyDown(Keys.S) && !_inputManager.KeyDelayed(Keys.S) && _gameState.GameStart == false)
            {
                _drawBackground.RandomiseBackground(Content);
                _gameState.StartGame();
                _sound.PlaySound(SoundType.GameStart, 1f);
                _inputManager.DelayKey(Keys.Escape, 30);
                ResetGame();
            }

            // Start Test game if T is pressed in menu
            if (state.IsKeyDown(Keys.T) && !_inputManager.KeyDelayed(Keys.T) && _gameState.GameMenu)
            {
                _drawBackground.RandomiseBackground(Content);
                _gameState.StartTestEnvironment();
                _sound.PlaySound(SoundType.GameStart, 1f);
                _inputManager.DelayKey(Keys.T, 30);
            }

            // Quit game on game menu or game over
            if (state.IsKeyDown(Keys.Escape) && !_inputManager.KeyDelayed(Keys.Escape) && (_gameState.GameMenu || _gameState.GameOver))
            {
                Exit();
            }


        }
        // Update the spawning logic and spawn enemies
        private void UpdateAndSpawnEnemies(float elapsedSeconds)
        {
            _timeSinceLastSpawn += elapsedSeconds;

            if (_timeSinceLastSpawn >= _initialSpawnInterval)
            {
                // Spawn an enemy
                SpawnEnemy();

                // Reset the timer
                _timeSinceLastSpawn = 0;

                // Decrease the spawn interval to make it faster (within a minimum limit)
                _initialSpawnInterval = Math.Max(_initialSpawnInterval - 0.02f, _smallMinSpawnInterval); // You can adjust the decrease rate here
            }

            if (_initialSpawnInterval <= _smallMinSpawnInterval)
            {
                SpawnBigEnemy();
                _initialSpawnInterval = 0.4f;
            }
        }
        // Spawn enemy
        private void SpawnEnemy()
        {
            int amountToSpawn = _randomGenerator.Next(1, 3);
            // Spawn your single enemy type
            for (int i = 0; i < amountToSpawn; i++)
            {
                Enemy enemy = _objectPool.SpawnEntity<Enemy>();
                enemy.IsBigEnemy = false;
            }

        }
        // Spawn big enemy
        private void SpawnBigEnemy()
        {
            Enemy spawnBigEnemy = _objectPool.SpawnEntity<Enemy>();
            if (spawnBigEnemy != null)
            {
                spawnBigEnemy.Health = 6;
                spawnBigEnemy.IsBigEnemy = true;
                spawnBigEnemy.Height = 50;
                spawnBigEnemy.Width = 100;
            }
        }
        // Reset all game values and objects to default and deactivate all objectpool entities
        private void ResetGame()
        {
            _initialSpawnInterval = 2f;
            _player.EntityReset();
            _earth.Reset();
            _objectPool.DeactivateAll();
            _gameScore = 0;
        }
        // Run methods when game ends
        private void GameOver()
        {
            _gameState.StartGameOver();
            _initialSpawnInterval = 2f;
            if (_gameScore > _highScore)
            {
                _sound.PlaySound(SoundType.NewHighScore, 1f);
                SaveAndLoad.SaveHighScore(_gameScore);
                _highScore = _gameScore;
            }
            else
            {
                _sound.PlaySound(SoundType.GameOver, 1f);
            }
        }
        // Run methods when game starts
        private void GameStart()
        {
            ResetGame();
            // If needed
        }
        // Read inputs to run methods for testing
        private void RunTestInputs(KeyboardState state)
        {
            // Ability to spawn entities in at will
            if (state.IsKeyDown(Keys.O) && !_inputManager.KeyDelayed(Keys.O))
            {
                _objectPool.SpawnEntity<Enemy>();
                _inputManager.DelayKey(Keys.S, 5);
            }

            if (state.IsKeyDown(Keys.P) && !_inputManager.KeyDelayed(Keys.P))
            {
                _objectPool.SpawnEntity<PowerUp>();
                _inputManager.DelayKey(Keys.P, 5);
            }
            if (state.IsKeyDown(Keys.I) && !_inputManager.KeyDelayed(Keys.I))
            {
                SpawnBigEnemy();
            }
            // sound test
            if (state.IsKeyDown(Keys.A) && !_inputManager.KeyDelayed(Keys.A))
            {
                _sound.PlaySound(SoundType.Explosion, 1f);
            }
        }
    }
}