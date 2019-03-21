using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_3.GalagaEntities {
    public class PlayerShot : Entity {
        

        public PlayerShot(DynamicShape shape, IBaseImage image) : base(shape, image) {
            
            // Shot is instantiated with a fixed direction going straight upwards. 
            Shape.AsDynamicShape().Direction = new Vec2F(0.0f, 0.01f);
        }
    }
}