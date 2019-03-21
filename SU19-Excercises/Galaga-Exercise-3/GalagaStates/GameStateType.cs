using System;

namespace Galaga_Exercise_3.GalagaStates {
    public enum GameStateType{
        GameRunning,
        GamePaused,
        MainMenu,
        TestState,
    }

    public class StateTransformer {
        public static GameStateType TransformStringToState(string state) {
            switch(state)
            { 
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;     
            }

            throw new ArgumentException("Arg was not a state");

        }

        public static string TransformStateToString(GameStateType state) {
            switch(state)
            { 
            case GameStateType.GameRunning:
                return "GAME_RUNNING";
            case GameStateType.GamePaused:
                return "GAME_PAUSED";
            case GameStateType.MainMenu:
                return "MAIN_MENU";     
            }

            throw new ArgumentException("Arg was not a state");

        }
    }
}