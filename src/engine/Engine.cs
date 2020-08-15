using SFML.System;

namespace engine {
    partial class Engine {
        private GameObject[] gameObjects = new GameObject[] {
            new Player(new Vector2f(512, 512))
        };

        private void init() {
            foreach(var gameObject in gameObjects) {
                gameObject.Init();
            }
        }

        private void draw() {
            foreach(var gameObject in gameObjects) {
                var pos = gameObject.GetPosition();

                if(pos.X > -Settings.ScreenSize.X * 0.5f && pos.X <= Settings.ScreenSize.X * 1.5f
                        && pos.Y > -Settings.ScreenSize.Y * 0.5f && pos.Y <= Settings.ScreenSize.Y * 1.5f) {
                    window.Draw(gameObject);
                }
            }
        }

        private void update(float deltaTime, KeyState keys) {
            if(keys.Quit) {
                window.Close();
            }

            foreach(var gameObject in gameObjects) {
                var pos = gameObject.GetPosition();
                var vel = gameObject.GetVelocity();
                var acc = gameObject.GetAcceleration();

                gameObject.SetPosition(new Vector2f(
                    pos.X + vel.X * deltaTime, pos.Y + vel.Y * deltaTime)
                );
                gameObject.SetVelocity(new Vector2f(
                    vel.X + acc.X * deltaTime, vel.Y + acc.Y * deltaTime)
                );

                gameObject.Update(deltaTime, keys);
            }
        }
    }
}
