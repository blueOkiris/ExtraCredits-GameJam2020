using SFML.Graphics;
using SFML.System;

namespace engine {
    partial class Engine {
        private Room currentRoom = new TestRoom();

        private void init() {
            currentRoom.Init();
        }

        private void draw() {
            window.SetView(currentRoom.GetView());
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
