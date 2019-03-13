using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using System.Drawing;

namespace Galaga_Excercise_2 {
    public class Score {
        private int score;
        private Text display;
        
        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }
        
        /// <summary>
        /// Adds points to score. The longer the time played, the less points is given for killing
        /// an enemy. 
        /// </summary>
        public void AddPoint() {
            score += 1000/(int) StaticTimer.GetElapsedSeconds();
        }
        
        /// <summary>
        /// Displaying score in top right corner of game window. 
        /// </summary>
        public void RenderScore() {
            
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }


        
    }
 
}