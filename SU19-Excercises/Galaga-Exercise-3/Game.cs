using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using Galaga_Exercise_3.GalagaState;

namespace Galaga_Exercise_3 {
    public class Game : IGameEventProcessor<object> {
        
        // Gets replaced by enemy lists inside squadron lists
        
        
       
        
        // Add more strides
        
        private GameEventBus<object> eventBus;
        
        private GameTimer gameTimer;
        
        private Window win;
        
        
        private StateMachine stateMachine;
        
       
        
        

        public Game() {
            win = new Window("lol", 500, 500);
            gameTimer = new GameTimer(60, 60);
           

            
            
            
            // eventBus handles events and let's different modules of the program communicate in a
            // streamlined manner through broadcasting of and subscription to events. 
            eventBus = Galaga_Exercise_3.GalagaBus.GetBus();
               
            eventBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window
                GameEventType.PlayerEvent,
                GameEventType.GameStateEvent,
            });

            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            stateMachine = new StateMachine();
            
            
            // Shall be replaced by enemy lists inside squadrons lists
            
            

            
        }

        

        /// <summary>
        ///     Processing events and dividing events between input events and window events.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="gameEvent"></param>
        public void ProcessEvent(GameEventType eventType,
            GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }

        /// <summary>
        ///     Controlling game flow by processing events and updating and rendering entities based on
        ///     a fixed timer to gain independence of platform architecture.
        /// </summary>
        public void GameLoop() {
            
            while (win.IsRunning()) {
                
                gameTimer.MeasureTime();
               

                while (gameTimer.ShouldUpdate()) {
                    eventBus.ProcessEvents();      
                    win.PollEvents();  
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    
                    stateMachine.ActiveState.RenderState();
             
                    
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - displaying last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates +
                                ", FPS: " + gameTimer.CapturedFrames;
                }
            }
        }

        /// <summary>
        ///     Controls user consequences of user input; Closing the game window by registering an
        ///     event; moving the player; shooting.
        /// </summary>
        /// <param name="key"></param>
        private void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.InputEvent, this, "KEY_ESCAPE",
                        "KEY_PRESSED", ""));
                break;
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_LEFT",
                        "KEY_PRESSED", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_RIGHT",
                        "KEY_PRESSED", ""));
                break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_SPACE",
                        "KEY_PRESSED", ""));
                break;

            case "KEY_UP":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.InputEvent, this, "KEY_UP",
                        "KEY_PRESSED", ""));
                break;
            case "KEY_DOWN":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.InputEvent, this, "KEY_DOWN",
                        "KEY_PRESSED", ""));
                break;
            case "KEY_ENTER":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.InputEvent, this, "KEY_ENTER",
                        "KEY_PRESSED", ""));
                break;

            
           

        }
        }

        /// <summary>
        ///     Controls consequences of releasing keys. The direction of the player should become 0, 0,
        ///     causing the player not to move. However, if SPACE-key is released, the Player Entity
        ///     is still be allowed to move, if the user is trying to move.
        /// </summary>
        /// <param name="key"></param>
        public void KeyRelease(string key) {
            switch (key) {
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_LEFT",
                        "KEY_RELEASE", ""));
                break;
            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_RIGHT",
                        "KEY_RELEASE", ""));
                break;
            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.PlayerEvent, this, "KEY_SPACE",
                        "KEY_RELEASE", ""));
                break;
            }
        }
       

        

       

       

        
    }
}