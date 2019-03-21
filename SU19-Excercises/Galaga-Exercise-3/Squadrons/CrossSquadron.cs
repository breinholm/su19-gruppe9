using System;
using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Exercise_3.GalagaEntities;
using Galaga_Exercise_3.GalagaStates;


namespace Galaga_Exercise_3.Squadrons {
    public class CrossSquadron : ISquadron {
        
        public int MaxEnemies { get; }
        private int packSize = 5;
        private Random rand;
        
        
 
        public CrossSquadron() {
            MaxEnemies = 20;
            
            Enemies = new EntityContainer<Enemy>();
            rand = new Random();
            
            
            // Adding this squadron to lists of squadrons in game
            GameRunning.enemySquadrons.Add(this);
        }
        
        public EntityContainer<Enemy> Enemies { get; set; }
        
        public void CreateEnemies(List<Image> enemyStrides) {

            float randX = 2.0f;
            while (randX < 0.2f || randX > 0.8f) {
                randX = (float) rand.NextDouble();
            }

            float randY = -1.0f;
            while (randY < 0.4f) {
                randY = (float) rand.NextDouble();
            }
            
            if (Enemies.CountEntities() <= MaxEnemies - packSize) {
                
                for (int i = 0; i < 3; i++) {
                    var tempEnemy = new Enemy(
                        new DynamicShape(new Vec2F(i*0.05f+randX, -i*0.05f+randY),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }

                for (int i = 0; i < 2; i++) {
                    
                    var tempEnemy = new Enemy(
                        new DynamicShape(new Vec2F(-0.1f*i+randX+0.1f, -0.1f*i+randY),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }
            }
        }
    }
}