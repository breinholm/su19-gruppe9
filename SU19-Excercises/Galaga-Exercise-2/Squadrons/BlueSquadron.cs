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
    public class BlueSquadron : ISquadron {
        

        
        public int MaxEnemies { get; }
       
        private Game game;


        public BlueSquadron(Game game) {
            MaxEnemies = 5;
            this.game = game; 
            Enemies = new EntityContainer<Enemy>();
            

        }
        public EntityContainer<Enemy> Enemies { get; }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            if (Enemies.CountEntities() < MaxEnemies) {
                for (int i = 0; i < 3; i++) {
                    var tempEnemy = new Enemy(game,
                        new DynamicShape(new Vec2F(i*0.05f+0.3f, -i*0.05f+0.5f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    Enemies.AddDynamicEntity(tempEnemy);
                }

                for (int i = 0; i < 2; i++) {
                    var tempEnemy = new Enemy(game,
                        new DynamicShape(new Vec2F(i*0.05f+0.45f, i*0.05f+0.45f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    Enemies.AddDynamicEntity(tempEnemy);
                    
                }
                
                
            }
            



        }
        
        


    }
}