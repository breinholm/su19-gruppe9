using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.GalagaEntities {
    public class Enemy : Entity {
        
        

        public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image) {
            
           
            StartingPosition = shape.Direction.Copy();
        }
        public Vec2F StartingPosition { get; private set; } 
    }
}