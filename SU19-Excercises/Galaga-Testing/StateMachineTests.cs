using System.Collections.Generic;
using DIKUArcade.EventBus;
using Galaga_Exercise_3;
using Galaga_Exercise_3.GalagaState;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;
        [SetUp]
        public void InitiateStateMachine() {
            
            DIKUArcade.Window.CreateOpenGLContext();
            
            GalagaBus.GetBus();
            stateMachine = new StateMachine();
            GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.GameStateEvent,
                GameEventType.InputEvent,
                GameEventType.WindowEvent,
                GameEventType.PlayerEvent,
               
                
            });
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        }
        
        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        [Test]
        public void TestEventGamePaused() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_PAUSED", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
        [Test]
        public void TestEventGameRunning() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
        [Test]
        public void TestEventMainMenu() {
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "GAME_RUNNING", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            GalagaBus.GetBus().RegisterEvent(
                GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.GameStateEvent,
                    this,
                    "CHANGE_STATE",
                    "MAIN_MENU", ""));
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
    }
}