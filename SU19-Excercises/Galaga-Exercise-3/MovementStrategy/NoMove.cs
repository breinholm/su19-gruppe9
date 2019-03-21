using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.GalagaStates;


namespace Galaga_Exercise_3.MovementStrategy {
    public class NoMove: IMovementStrategy {
        public NoMove() {
            GameRunning.MovementStrategies.Add(this);
        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f,0f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
        
    }
}