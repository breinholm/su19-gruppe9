using System;
using System.Collections.Generic;
using System.Drawing;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Excercise_2;
using Galaga_Excercise_2.GalagaEntities;
using Image = DIKUArcade.Graphics.Image;
using Galaga_Exercise_2;

namespace Galaga_Exercise_2.Squadrons {
    public class VSquadron : ISquadron {
        
        public int MaxEnemies { get; }
        private int packSize = 5;
        
        private Game game;

        public VSquadron(Game game) {
            MaxEnemies = 20;
            this.game = game; 
            Enemies = new EntityContainer<Enemy>();
            
            // Adding this squadron to lists of squadrons in game
            this.game.enemySquadrons.Add(this);
            }
        
        public EntityContainer<Enemy> Enemies { get; set; }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            
            if (Enemies.CountEntities() <= MaxEnemies - packSize) {
                
                for (int i = 0; i < 3; i++) {
                    var tempEnemy = new Enemy(game,
                        new DynamicShape(new Vec2F(i*0.05f+0.3f, -i*0.05f+1.05f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }

                for (int i = 0; i < 2; i++) {
                    
                    var tempEnemy = new Enemy(game,
                        new DynamicShape(new Vec2F(i*0.05f+0.45f, i*0.05f+0.95f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }
            }
        }
    }
}