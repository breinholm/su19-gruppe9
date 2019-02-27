using System;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Timers;

namespace Galaga_Excercise_1 {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private DIKUArcade.Timers.GameTimer gameTimer;
        private Player player;

        public Game() {
// TODO: Choose some reasonable values for the window and timer constructor.
// For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            this.player = new Player(this, new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), new Image(Path.Combine
                ("Assets", "Images", "Player.png")));
            
            win = new Window("motherfucker", 500, 500);
            gameTimer = new GameTimer(60, 144);
        }

        public void GameLoop() {
            
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
// Update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
// Render gameplay entities here
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
// 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        public void KeyPress(string key) {
            throw new NotImplementedException();
        }

        public void KeyRelease(string key) {
            throw new NotImplementedException();
        }

        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            throw new NotImplementedException();
        }
    }
}