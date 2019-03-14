using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Excercise_2;
using Galaga_Excercise_2.GalagaEntities;

namespace Galaga_Exercise_2.MovementStrategy {
    public class Down: IMovementStrategy {
        private static float speed = -0.001f;
        public Down(Game game) {
            game.MovementStrategies.Add(this);
        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f,speed));
            enemy.Shape.AsDynamicShape().Move();
            
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f, speed));
                enemy.Shape.AsDynamicShape().Move();
            }
            
        }
        
    }
}