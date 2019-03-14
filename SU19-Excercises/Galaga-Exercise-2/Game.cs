using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Excercise_2.GalagaEntities;
using Galaga_Exercise_2.MovementStrategy;
using Galaga_Exercise_2.Squadrons;

namespace Galaga_Excercise_2 {
    public class Game : IGameEventProcessor<object> {
        
        // Gets replaced by enemy lists inside squadron lists
        
        
        private List<Image> blueMonsterStrides;
        
        // Add more strides
        
        public GameEventBus<object> eventBus;
        private int explosionLength;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private GameTimer gameTimer;
        private Player player;
        private Score score;
        private Window win;
        public List<IMovementStrategy> MovementStrategies;
        
       
        
        public List<ISquadron> enemySquadrons;

        public Game() {
            win = new Window("lol", 500, 500);
            gameTimer = new GameTimer(60, 60);
            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            
            
            // The blueMonsterStrides list includes four different versions of the monster images 
            // allowing for a moving animation of the monster.
            blueMonsterStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images",
                "BlueMonster.png"));
            
            enemySquadrons = new List<ISquadron>();
            new CrossSquadron(this).CreateEnemies(blueMonsterStrides);
            
            new VSquadron(this).CreateEnemies(blueMonsterStrides);
            
            new LineSquadron(this).CreateEnemies(blueMonsterStrides);
            MovementStrategies = new List<IMovementStrategy>();
            
            new Down(this);
            new ZigZagDown(this);
            new NoMove(this);

            

            
            
            
            // eventBus handles events and let's different modules of the program communicate in a
            // streamlined manner through broadcasting of and subscription to events. 
            eventBus = new GameEventBus<object>();
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent
            });
            
            
           
            
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, player);
            
            // Shall be replaced by enemy lists inside squadrons lists
            
            

            PlayerShots = new List<PlayerShot>();

            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);
            explosionLength = 500;

            score = new Score(new Vec2F(0.8f, 0.8f), new Vec2F(0.2f, 0.2f));
        }

        public List<PlayerShot> PlayerShots { get; private set; }

        /// <summary>
        ///     Processing events and dividing events between input events and window events.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
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

        /// <summary>
        ///     Controlling game flow by processing events and updating and rendering entities based on
        ///     a fixed timer to gain independence of platform architecture.
        /// </summary>
        public void GameLoop() {
            
            while (win.IsRunning()) {
                
                gameTimer.MeasureTime();
                eventBus.ProcessEvents();

                while (gameTimer.ShouldUpdate()) {
                    MoveSquadrons();
                    win.PollEvents();
                    player.Move();
                    IterateShots();
                }

                if (gameTimer.ShouldRender()) {
                    
                    win.Clear();
                    player.RenderEntity();

                    foreach (var playerShot in PlayerShots) { playerShot.RenderEntity(); }

                    
                    
                    //VSquadron.Enemies.RenderEntities();
                    foreach (var squad in enemySquadrons) {
                        squad.Enemies.RenderEntities();
                    }
                    
                    
                    explosions.RenderAnimations();
                    score.RenderScore();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - displaying last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        /// <summary>
        ///     Controls user consequences of user input; Closing the game window by registering an
        ///     event; moving the player; shooting.
        /// </summary>
        /// <param name="key"></param>
        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW",
                        "", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_LEFT",
                        "DOWN", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_RIGHT",
                        "DOWN", ""));
                break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_SPACE",
                        "DOWN", ""));
                break;
            }
        }

        /// <summary>
        ///     Controls consequences of releasing keys. The direction of the player should become 0, 0,
        ///     causing the player not to move. However, if SPACE-key is released, the Player Entity
        ///     is still be allowed to move, if the user is trying to move.
        /// </summary>
        /// <param name="key"></param>
        public void KeyRelease(string key) {
            switch (key) {
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_LEFT",
                        "UP", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_RIGHT",
                        "UP", ""));
                break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_SPACE",
                        "UP", ""));
                break;
            }
        }

        

        public void MoveSquadrons() {
            foreach (var squadron in enemySquadrons) {
                MovementStrategies[1].MoveEnemies(squadron.Enemies);
            }
        }

        /// <summary>
        ///     Making shots move upwards. Marks shots and enemies for deletion, if they collide with
        ///     enemy or have moved out of the window scope. Creates explosions and adds points when
        ///     enemies are shot.
        /// </summary>
        public void IterateShots() {
            foreach (var shot in PlayerShots) {
                shot.Shape.Move(shot.Shape.AsDynamicShape().Direction);

                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var squadron in enemySquadrons) {
                    foreach (Enemy enemy in squadron.Enemies) {
                        if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape)
                            .Collision) {
                            enemy.DeleteEntity();
                            shot.DeleteEntity();
                            AddExplosion(enemy.Shape.Position.X - enemy.Shape.Extent.X * 0.5f,
                                enemy.Shape.Position.Y - enemy.Shape.Extent.Y * 0.5f,
                                enemy.Shape.Extent.X * 2.0f,
                                enemy.Shape.Extent.Y * 2.0f);
                            score.AddPoint();
                        }
                    }
                }

                
                
            }
            // Updating list of enemies still alive

            foreach (var squadron in enemySquadrons) {
                var newEnemies = new EntityContainer<Enemy>();
                foreach (Enemy enemy in squadron.Enemies) {
                    if (!enemy.IsDeleted()) {
                        newEnemies.AddDynamicEntity(enemy);
                    }
                }

                squadron.Enemies = newEnemies;


                
            }

            
            

            
           

            var newPlayerShots = new List<PlayerShot>();
            foreach (var shot in PlayerShots) {
                if (!shot.IsDeleted()) {
                    newPlayerShots.Add(shot);
                }
            }

            // Updating list of shots still in game window
            PlayerShots = newPlayerShots;
        }

        /// <summary>
        ///     Creating explosion. Used in iterate shots, when shots hit enemies. Using ImageStride
        ///     instead of static image to create moving animation.
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="extentX"></param>
        /// <param name="extentY"></param>
        public void AddExplosion(float posX, float posY, float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }
    }
}