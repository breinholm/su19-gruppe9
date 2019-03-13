using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Excercise_2.GalagaEntities {
    public class Enemy : Entity {
        private Game game;
        
        

        public Enemy(Game game, DynamicShape shape, IBaseImage image) : base(shape, image) {
            this.game = game;
           
            StartingPosition = shape.Direction.Copy();
        }
        public Vec2F StartingPosition { get; private set; } 
    }
}