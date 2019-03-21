using System;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.GalagaStates;


namespace Galaga_Exercise_3.MovementStrategy {
    public class ZigZagDown: IMovementStrategy {
        private static float speed = -0.0003f;
        private static float period = 0.045f;
        private static float amplitude = 0.005f;
        public ZigZagDown() {
            GameRunning.MovementStrategies.Add(this);
        }
        
        public void MoveEnemy(Enemy enemy) {
            float X = enemy.Shape.AsDynamicShape().Position.X;
            float Y = enemy.Shape.AsDynamicShape().Position.Y;
            float newY = Y + ZigZagDown.speed;
            
            enemy.Shape.AsDynamicShape().ChangeDirection(
                new Vec2F(amplitude*(float)(Math.Sin(2.0f*Math.PI*(enemy.StartingPosition.Y-newY)/period)), speed));
            enemy.Shape.AsDynamicShape().Move();
        }

        public void MoveEnemies(EntityContainer<Enemy> enemies) {
            foreach (Enemy enemy in enemies) {
                MoveEnemy(enemy);
            }
        }
        
    }
}