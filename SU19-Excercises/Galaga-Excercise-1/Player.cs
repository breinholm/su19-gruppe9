using System.CodeDom.Compiler;
using System.Security.Policy;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Excercise_1 {
    public class Player : Entity {
        private Game game;
        private Vec2F testVec;
        // Loading image once instead of every time a shot is added
        private Image bullet = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));
       
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
        }

        public void Direction(Vec2F dir) {
            Shape.AsDynamicShape().Direction = dir;
        }

        public void Move() {
            testVec = Shape.Position + Shape.AsDynamicShape().Direction;
            if (testVec.X >= 0.0f && testVec.X <= 0.9f) {
                Shape.Move(Shape.AsDynamicShape().Direction);
            }
        }

        public void Shoot() {
            
            // Placing shot in the middle on top of the player 
            PlayerShot tempShot = new PlayerShot(game,
                new DynamicShape(new Vec2F(Shape.Position.X + Shape.Extent.X / 2.0f
                        - 0.004f, 
                        Shape.Position.Y + 0.1f),
                    new Vec2F(0.008f, 0.027f)), bullet);
            game.PlayerShots.Add(tempShot);
        }
    }
}