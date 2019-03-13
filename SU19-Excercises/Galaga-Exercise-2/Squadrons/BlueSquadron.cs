using System;
using System.Collections.Generic;
using System.Drawing;
using DIKUArcade.Entities;
using Galaga_Excercise_2.GalagaEntities;
using Image = DIKUArcade.Graphics.Image;

namespace Galaga_Exercise_2.Squadrons {
    public class BlueSquadron : ISquadron {
        

        public EntityContainer<Enemy> Enemies { get; }
        public int MaxEnemies { get; }
        
        public void CreateEnemies(List<Image> enemyStrides) {
            throw new NotImplementedException();
        }


    }
}