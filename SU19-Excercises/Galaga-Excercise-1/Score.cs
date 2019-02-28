using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;
using System.Drawing;

namespace Galaga_Excercise_1 {
    public class Score {
        private int score;
        private Text display;   
        
        
        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(score.ToString(), position, extent);
        }
        public void AddPoint() {
            score += 1000/(int) StaticTimer.GetElapsedSeconds();
        }
        
        public void RenderScore() {
            display.SetText(string.Format("Score: {0}", score.ToString()));
            display.SetColor(new Vec3I(255, 0, 0));
            display.RenderText();
        }


        
    }
 
}