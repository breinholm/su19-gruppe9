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
        
        
 
        public CrossSquadron() {
            MaxEnemies = 20;
            
            Enemies = new EntityContainer<Enemy>();
            
            
            // Adding this squadron to lists of squadrons in game
            GameRunning.enemySquadrons.Add(this);
        }
        
        public EntityContainer<Enemy> Enemies { get; set; }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            
            if (Enemies.CountEntities() <= MaxEnemies - packSize) {
                
                for (int i = 0; i < 3; i++) {
                    var tempEnemy = new Enemy(
                        new DynamicShape(new Vec2F(i*0.05f+0.3f, -i*0.05f+0.7f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }

                for (int i = 0; i < 2; i++) {
                    
                    var tempEnemy = new Enemy(
                        new DynamicShape(new Vec2F(-0.1f*i+0.4f, -0.1f*i+0.7f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }
            }
        }
    }
}