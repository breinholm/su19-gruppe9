using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Excercise_1 {
    public class Player : Entity {
        
        // Loading image once instead of every time a shot is added
        private Image bullet = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
        private Game game;
        private Vec2F testVec;

        public Player(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
        }

        /// <summary>
        /// Changing direction of the player. Called in move method. Downcasting the Shape to
        /// DynamicShape in order to get access to more properties, including Direction.  
        /// </summary>
        /// <param name="dir"></param>
        public void Direction(Vec2F dir) { Shape.AsDynamicShape().Direction = dir; }

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
        public void Shoot() {
            
            // Placing shot in the middle on top of the player 
            var tempShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + Shape.Extent.X / 2.0f - 0.004f,
                        Shape.Position.Y + 0.1f),
                    new Vec2F(0.008f, 0.027f)), bullet);
            
            game.PlayerShots.Add(tempShot);
        }
    }
}