using System.Security.Policy;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Excercise_1 {
    public class Player : Entity {
        private Game game;
        private Vec2F testVec;
       

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
    }
}