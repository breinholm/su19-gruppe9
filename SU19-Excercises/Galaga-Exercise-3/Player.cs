using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaStates;



namespace Galaga_Exercise_3 {
    public class Player : Entity, IGameEventProcessor<object>{
        // Loading image once instead of every time a shot is added
        private Image bullet = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
 
        private Vec2F testVec;

        public Player(DynamicShape shape, IBaseImage image) : base(shape, image) {    
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {

                switch (gameEvent.Parameter1) {
                case "KEY_RELEASE":
                    ProcessKeyRelease(gameEvent.Message);
                    break;
                case "KEY_PRESSED":
                    ProcessKeyPress(gameEvent.Message);
                    break;
                }
            }
        }

        private void ProcessKeyRelease(string message) {
            if (message == "KEY_RIGHT" && Shape.AsDynamicShape().Direction.X > 0) {
                    Direction(new Vec2F(0f, 0f));
                }
            else if (message == "KEY_LEFT" && Shape.AsDynamicShape().Direction.X < 0) {
                Direction(new Vec2F(0f, 0f));
            }
        }

        private void ProcessKeyPress(string message) {
            switch (message) {
            case "KEY_LEFT":
                Direction(new Vec2F(-0.01f, 0f));
                break;
            case "KEY_RIGHT":
                Direction(new Vec2F(0.01f, 0f));
                break;
            case "KEY_SPACE":
                Shoot();
                break;
            }
            
        }
        
            


        /// <summary>
            /// Changing direction of the player. Called in move method. Downcasting the Shape to
            /// DynamicShape in order to get access to more properties, including Direction.  
            /// </summary>
            /// <param name="dir"></param>
            private void Direction(Vec2F dir) {
                Shape.AsDynamicShape().Direction = dir;
            }

            /// <summary>
            /// Moves player, if movement will not cause player to move out of game window. 
            /// </summary>
            public void Move() {
                testVec = Shape.Position + Shape.AsDynamicShape().Direction;

                if (testVec.X >= 0.0f && testVec.X <= 0.9f) {
                    Shape.Move(Shape.AsDynamicShape().Direction);
                }
            }

            /// <summary>
            /// Creating shots by instantiating PlayerShots and adding them to PlayerShots list. 
            /// </summary>
            private void Shoot() {
                // Placing shot in the middle on top of the player 
                var tempShot = new PlayerShot(
                    new DynamicShape(new Vec2F(Shape.Position.X + Shape.Extent.X / 2.0f - 0.004f,
                            Shape.Position.Y + 0.1f),
                        new Vec2F(0.008f, 0.027f)), bullet);

                GameRunning.getPlayerShots().Add(tempShot);
            }
        }
    }

    