using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;

namespace Galaga_Excercise_1 {
    public class Game : IGameEventProcessor<object> {
        private List<Enemy> enemies;
        private List<Image> enemyStrides;
        private GameEventBus<object> eventBus;
        private int explosionLength;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private GameTimer gameTimer;
        private Player player;
        private Window win;
        private Score score;


        public Game() {
// TODO: Choose some reasonable values for the window and timer constructor.
// For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            win = new Window("lol", 500, 500);
            gameTimer = new GameTimer(60, 60);

            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));

            enemyStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images",
                "BlueMonster.png"));
            enemies = new List<Enemy>();
            AddEnemies();

            PlayerShots = new List<PlayerShot>();

            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent // messages to the window
            });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);
            explosionLength = 500;
            score = new Score(new Vec2F(0.8f,0.8f),new Vec2F(0.2f,0.2f));
        }

        // List of playershots
        public List<PlayerShot> PlayerShots { get; private set; }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                eventBus.ProcessEvents();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    player.Move();
                    IterateShots();

// Update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.RenderEntity();

                    // Create playerShots
                    foreach (var playerShot in PlayerShots) {
                        playerShot.RenderEntity();
                    }

                    // Trying to render enemy entities
                    foreach (var enemy in enemies) {
                        enemy.RenderEntity();
                    }

                    explosions.RenderAnimations();
                    score.RenderScore();


// Render gameplay entities here
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
// 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
            case "KEY_LEFT":
                player.Direction(new Vec2F(-0.01f, 0f));
                break;
            case "KEY_RIGHT":
                player.Direction(new Vec2F(0.01f, 0f));
                break;
            case "KEY_SPACE":
                player.Shoot();
                break;
            }
        }

        public void KeyRelease(string key) {
            if (key != "KEY_SPACE") {
                player.Direction(new Vec2F(0f, 0f));
            }
        }

        /// <summary>
        ///     Adds enemies to the enemies-list.
        /// </summary>
        public void AddEnemies() {
            for (var i = 0; i < 4; i++) {
                var tempEnemy = new Enemy(this,
                    new DynamicShape(new Vec2F(i * 0.3f, 0.8f),
                        new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80,
                        enemyStrides)); //Enemy strides is the list of pictures that 
                enemies.Add(tempEnemy);
            }
        }

        public void IterateShots() {
            foreach (var shot in PlayerShots) {
                shot.Shape.Move(shot.Shape.AsDynamicShape().Direction);
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var enemy in enemies) {
                    if (CollisionDetection
                        .Aabb(shot.Shape.AsDynamicShape(), enemy.Shape).Collision) {
                        enemy.DeleteEntity();
                        AddExplosion(enemy.Shape.Position.X - enemy.Shape.Extent.X * 0.5f,
                            enemy.Shape.Position.Y - enemy.Shape.Extent.Y * 0.5f,
                            enemy.Shape.Extent.X * 2.0f, enemy.Shape.Extent.Y * 2.0f);
                        score.AddPoint();
                        
                    }

// TODO: perform collision detection
// (hint: Physics.CollisionDetection.Aabb)
                }
            }

            var newEnemies = new List<Enemy>();
            foreach (var enemy in enemies) {
                if (!enemy.IsDeleted()) {
                    newEnemies.Add(enemy);
                }
            }

            enemies = newEnemies;

            var newPlayerShots = new List<PlayerShot>();
            foreach (var shot in PlayerShots) {
                if (!shot.IsDeleted()) { //Marks the entity for deletion
                    newPlayerShots.Add(shot);
                }
            }

            PlayerShots = newPlayerShots;
        }

        public void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}