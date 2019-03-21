using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.MovementStrategy;
using Galaga_Exercise_3.Squadrons;

namespace Galaga_Exercise_3.GalagaStates {
    public class GameRunning : IGameState {
        private static GameRunning instance;
        private int explosionLength;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private List<Image> blueMonsterStrides;

        private Player player;
        private Score score;

        public static List<PlayerShot> getPlayerShots() {
            return GameRunning.PlayerShots;
        }

        public static List<PlayerShot> PlayerShots { get; private set; }
        public static List<IMovementStrategy> MovementStrategies { get; private set; }
        public static List<ISquadron> enemySquadrons { get; private set; }


        private GameRunning() {
            player = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            NewGame();
            
            
        }

        public void NewGame() {
            player.Shape.AsDynamicShape().SetPosition(new Vec2F(0.45f, 0.1f));
            


            // The blueMonsterStrides list includes four different versions of the monster images 
            // allowing for a moving animation of the monster.
            blueMonsterStrides = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images",
                "BlueMonster.png"));

            GameRunning.enemySquadrons = new List<ISquadron>();
            new CrossSquadron().CreateEnemies(blueMonsterStrides);

            new VSquadron().CreateEnemies(blueMonsterStrides);

            new LineSquadron().CreateEnemies(blueMonsterStrides);

            GameRunning.MovementStrategies = new List<IMovementStrategy>();

            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, player);

            new Down();
            new ZigZagDown();
            new NoMove();
            GameRunning.PlayerShots = new List<PlayerShot>();


            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(8);
            explosionLength = 500;

            score = new Score(new Vec2F(0.8f, 0.8f), new Vec2F(0.2f, 0.2f));
        }


        public static GameRunning GetInstance() {
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
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

        /// <summary>
        ///     Making shots move upwards. Marks shots and enemies for deletion, if they collide with
        ///     enemy or have moved out of the window scope. Creates explosions and adds points when
        ///     enemies are shot.
        /// </summary>
        public void IterateShots() {
            foreach (var shot in GameRunning.PlayerShots) {
                shot.Shape.Move(shot.Shape.AsDynamicShape().Direction);

                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var squadron in GameRunning.enemySquadrons) {
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
            // Removing dead enemies

            foreach (var squadron in GameRunning.enemySquadrons) {
                var newEnemies = new EntityContainer<Enemy>();
                foreach (Enemy enemy in squadron.Enemies) {
                    if (!enemy.IsDeleted()) {
                        newEnemies.AddDynamicEntity(enemy);
                    }
                }

                squadron.Enemies = newEnemies;
            }

            var newPlayerShots = new List<PlayerShot>();
            foreach (var shot in GameRunning.PlayerShots) {
                if (!shot.IsDeleted()) {
                    newPlayerShots.Add(shot);
                }
            }

            // Updating list of shots still in game window
            GameRunning.PlayerShots = newPlayerShots;
        }

        /// <summary>
        ///     Respawns squadron when it has been killed
        /// </summary>
        public void RespawnSquadrons() {
            foreach (var squadron in GameRunning.enemySquadrons) {
                if (squadron.Enemies.CountEntities() == 0) {
                    squadron.CreateEnemies(blueMonsterStrides);
                }
            }
        }


        /// <summary>
        ///     Moves every enemy according to a movementstrategy
        /// </summary>
        public void MoveSquadrons() {
            var i = 0;
            foreach (var squadron in GameRunning.enemySquadrons) {
                GameRunning.MovementStrategies[i].MoveEnemies(squadron.Enemies);
                i++;
                foreach (Enemy enemy in squadron.Enemies) {
                    //Marking enemies when they walk out of screen
                    if (enemy.Shape.Position.Y < 0.0f) {
                        enemy.DeleteEntity();
                    }
                }
            }
        }


        public void UpdateGameLogic() {
            MoveSquadrons();
            player.Move();
            IterateShots();
            RespawnSquadrons();
        }

        public void RenderState() {
            player.RenderEntity();
            foreach (var playerShot in GameRunning.PlayerShots) {
                playerShot.RenderEntity();
            }

            //VSquadron.Enemies.RenderEntities();
            foreach (var squad in GameRunning.enemySquadrons) {
                squad.Enemies.RenderEntities();
            }

            explosions.RenderAnimations();
            score.RenderScore();
        }

        public void GameLoop() { }
        public void InitializeGameState() {
            
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESSED") {
                switch (keyValue) {
                case "KEY_ESCAPE":
                    GalagaBus.GetBus().RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent,
                            this,
                            "CHANGE_STATE",
                            "GAME_PAUSED", ""));
                    break;
                }


            }

        }
    }
}