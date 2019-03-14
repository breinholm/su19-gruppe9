using System.Collections.Generic;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using Galaga_Excercise_2;
using Galaga_Excercise_2.GalagaEntities;

namespace Galaga_Exercise_2.Squadrons {
    public class LineSquadron : ISquadron {
        
        public int MaxEnemies { get; }
        private int packSize = 8;
        
        private Game game;

        public LineSquadron(Game game) {
            MaxEnemies = 24;
            this.game = game; 
            Enemies = new EntityContainer<Enemy>();
            
            // Adding this squadron to lists of squadrons in game
            this.game.enemySquadrons.Add(this);
        }
        
        public EntityContainer<Enemy> Enemies { get; set; }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            
            if (Enemies.CountEntities() <= MaxEnemies - packSize) {
                
                for (int i = 0; i < 8; i++) {
                    var tempEnemy = new Enemy(game,
                        new DynamicShape(new Vec2F(i*0.1f+0.1f, 0.9f),
                            new Vec2F(0.1f, 0.1f)),
                        new ImageStride(80, enemyStrides));
                    
                    Enemies.AddDynamicEntity(tempEnemy);
                }

            }
        }
    }
}