using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.State;

namespace Galaga_Exercise_3.GalagaStates {
    public class GamePaused : IGameState {
        private static GamePaused instance;
        private int activeMenuButton;

        private Entity backGroundImage;
        private Vec2F buttonSize = new Vec2F(0.20f, 0.20f);
        private int mainMenu = 0;
        private Vec2F mainMenuPos;
        private int maxMenuButtons;
        private Text[] menuButtons;
        private Entity positionMarker;
        private int resumeGame = 1;
        private Vec2F resumeGamePos;

        private GamePaused() {
            resumeGamePos = new Vec2F(0.55f, 0.627f);
            mainMenuPos = new Vec2F(0.55f, 0.477f);
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1f, 1f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));

            positionMarker =
                new Entity(new DynamicShape(resumeGamePos, new Vec2F(0.05f, 0.01f)),
                    new Image(Path.Combine("Assets", "Images", "BulletRedTurned.png")));

            menuButtons = new[] {
                new Text("Main Menu", new Vec2F(0.40f, 0.30f), buttonSize),
                new Text("Resume", new Vec2F(0.40f, 0.45f), buttonSize)
            };
            activeMenuButton = 1;
            maxMenuButtons = menuButtons.Length;
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            positionMarker.RenderEntity();

            for (var i = 0; i < maxMenuButtons; i++) {
                if (i != activeMenuButton) {
                    menuButtons[i].SetColor(new Vec3I(255, 0, 0));
                } else {
                    menuButtons[i].SetColor(new Vec3I(0, 255, 0));
                }

                menuButtons[i].RenderText();
            }
        }

        public void HandleKeyEvent(string keyValue, string keyAction) {
            if (keyAction == "KEY_PRESSED") {
                switch (keyValue) {
                case "KEY_UP":
                    if (activeMenuButton < maxMenuButtons - 1) {
                        activeMenuButton++;
                        positionMarker.Shape.AsDynamicShape().Move(new Vec2F(0f, 0.15f));
                    } else {
                        activeMenuButton = 0;
                        positionMarker.Shape.AsDynamicShape().SetPosition(mainMenuPos);
                    }

                    break;
                case "KEY_DOWN":
                    if (activeMenuButton > 0) {
                        positionMarker.Shape.AsDynamicShape().Move(new Vec2F(0f, -0.15f));
                        activeMenuButton--;
                    } else {
                        activeMenuButton = maxMenuButtons - 1;
                        positionMarker.Shape.AsDynamicShape().SetPosition(resumeGamePos);
                    }

                    break;
                case "KEY_ENTER":


                    if (activeMenuButton == resumeGame) {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "GAME_RUNNING", ""));
                    } else if (activeMenuButton == mainMenu) {
                        GalagaBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.GameStateEvent,
                                this,
                                "CHANGE_STATE",
                                "MAIN_MENU", ""));
                    }

                    break;
                }
            }
        }

        public void GameLoop() { }

        public void InitializeGameState() { }

        public void UpdateGameLogic() { }

        public static GamePaused GetInstance() {
            return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
        }
    }
}