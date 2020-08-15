using SFML.Graphics;
using SFML.System;

namespace engine {
    partial class Engine {
        private Room currentRoom = new TestRoom();

        private void init() {
            currentRoom.Init();
        }

        private void draw() {
            var view = new View();
            view.Center = new Vector2f(
                Settings.ScreenSize.X / 2 + currentRoom.GetViewPosition().X,
                Settings.ScreenSize.Y / 2 + currentRoom.GetViewPosition().Y
            );
            view.Size = new Vector2f(Settings.ScreenSize.X, Settings.ScreenSize.Y);
            window.SetView(view);
            window.Draw(currentRoom);
        }

        private void update(float deltaTime, KeyState keys) {
            if(keys.Quit) {
                window.Close();
            }

            currentRoom.Update(deltaTime, keys);
            currentRoom = currentRoom.ChangeIfShould();
        }
    }
}
