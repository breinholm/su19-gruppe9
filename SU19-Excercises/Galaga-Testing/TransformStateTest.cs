using System;
using System.Reflection.Emit;
using Galaga_Exercise_3.GalagaStates;
using NUnit.Framework;

namespace Galaga_Testing {
    [TestFixture]
    public class TransformStateTests {
        [Test]
        public void TestRunningStateToString() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GameRunning), "GAME_RUNNING");
        }
        [Test]
        public void TestPausedStateToString() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.GamePaused), "GAME_PAUSED");
        }
        [Test]
        public void TestMainMenuStateToString() {
            Assert.AreEqual(StateTransformer.TransformStateToString(GameStateType.MainMenu), "MAIN_MENU");
        }
        [Test]
        public void TestExceptionStateToString() {
            
            Assert.Throws<ArgumentException>(() => StateTransformer.TransformStateToString(GameStateType.TestState));
        }
        
        [Test]
        public void TestMainMenuStringToState() {
            Assert.AreEqual(StateTransformer.TransformStringToState("MAIN_MENU"), GameStateType.MainMenu);
        }
        [Test]
        public void TestPausedStringToState() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_PAUSED"), GameStateType.GamePaused);
        }
        [Test]
        public void TestRunningStringToState() {
            Assert.AreEqual(StateTransformer.TransformStringToState("GAME_RUNNING"), GameStateType.GameRunning);
        }
        [Test]
        public void TestExceptionStringToState() {
            
            Assert.Throws<ArgumentException>(() => StateTransformer.TransformStringToState("TEST_STRING"));
        }
    }
}