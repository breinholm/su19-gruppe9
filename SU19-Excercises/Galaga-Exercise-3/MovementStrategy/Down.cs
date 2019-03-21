using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3.MovementStrategy {
    public class Down: IMovementStrategy {
        private static float speed = -0.001f;
        public Down() {
            GameRunning.MovementStrategies.Add(this);
        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f,speed));
            enemy.Shape.AsDynamicShape().Move();
            
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
            
        }
        
    }
}