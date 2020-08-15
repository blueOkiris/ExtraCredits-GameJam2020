using SFML.Graphics;
using SFML.System;

namespace engine {
    interface Room : Drawable {
        GameObject[] GetGameObjects();
        void Init();
        void Update(float deltaTime, KeyState keys);
        Room ChangeIfShould();
    }

    class TestRoom : Room
    {
        private GameObject[] gameObjects;

        public TestRoom() {
            gameObjects = new GameObject[] {
                new Player(new Vector2f(512, 512))
            };
        }

        public Room ChangeIfShould() {
            return this;
        }

        public void Init() {
            foreach(var gameObject in gameObjects) {
                gameObject.Init();
            }
        }

        public void Update(float deltaTime, KeyState keys) {
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

        public void Draw(RenderTarget target, RenderStates states) {
            foreach(var gameObject in gameObjects) {
                var pos = gameObject.GetPosition();

                if(pos.X > -Settings.ScreenSize.X * 0.5f && pos.X <= Settings.ScreenSize.X * 1.5f
                        && pos.Y > -Settings.ScreenSize.Y * 0.5f && pos.Y <= Settings.ScreenSize.Y * 1.5f) {
                    target.Draw(gameObject);
                }
            }
        }

        public GameObject[] GetGameObjects() => gameObjects;
    }
}