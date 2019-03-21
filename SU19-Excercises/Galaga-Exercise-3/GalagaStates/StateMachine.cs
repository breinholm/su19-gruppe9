using DIKUArcade.EventBus;
using DIKUArcade.State;
using Galaga_Exercise_3.GalagaStates;

namespace Galaga_Exercise_3.GalagaState {
    public class StateMachine : IGameEventProcessor<object> {
        public StateMachine() {
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        public IGameState ActiveState { get; private set; }

        /// <summary>
        ///     Controlling the game messages in order to maintain the correct ActiveState
        /// </summary>
        /// <param name="eventType">The event type contained in the message</param>
        /// <param name="gameEvent">The GameEvent object holding message and parameters</param>
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                if (gameEvent.Message == "CHANGE_STATE") {
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                }

                if (gameEvent.Parameter2 == "NEW_GAME") {
                    ActiveState.InitializeGameState();
                }
            } else if (eventType == GameEventType.InputEvent) {
                ActiveState.HandleKeyEvent(gameEvent.Message, gameEvent.Parameter1);
            }
        }

        /// <summary>
        ///     Switches states according to the given argument
        /// </summary>
        /// <param name="stateType">The state that has to be switched to</param>
        private void SwitchState(GameStateType stateType) {
            switch (stateType) {
            case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                break;
            case GameStateType.GamePaused:
                ActiveState = GamePaused.GetInstance();
                break;
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                break;
            }
        }
    }
}