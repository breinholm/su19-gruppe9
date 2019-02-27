using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Galaga_Excercise_1 {
    public class Player : Entity {
        private Game game;
        
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
            
        }
    }

}