using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Excercise_2;
using Galaga_Excercise_2.GalagaEntities;

namespace Galaga_Exercise_2.MovementStrategy {
    public class NoMove: IMovementStrategy {
        public NoMove(Game game) {
            game.MovementStrategies.Add(this);
        }
        public void MoveEnemy(Enemy enemy) {
            enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f,0f));
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                enemy.Shape.AsDynamicShape().ChangeDirection(new Vec2F(0f, 0f));
            }
        }
        
    }
}