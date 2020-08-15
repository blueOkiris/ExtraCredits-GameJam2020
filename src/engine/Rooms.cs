using System;
using SFML.Graphics;
using SFML.System;

namespace engine {
    interface Room : Drawable {
        GameObject[] GetGameObjects();
        void Init();
        void Update(float deltaTime, KeyState keys);
        Vector2u GetSize();
        Vector2u GetViewPosition();
        Room ChangeIfShould();
    }

    class TestRoom : Room
    {
        private GameObject[] gameObjects;
        private Vector2u size;
        private Vector2u view;

        public TestRoom() {
            size = new Vector2u(2560, 1080);
            view = new Vector2u(0, 0);
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

                gameObject.Update(deltaTime, keys, this);
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            foreach(var gameObject in gameObjects) {
                var pos = gameObject.GetPosition();

                if(pos.X >= (int) view.X - 64 && pos.X <= view.X + Settings.ScreenSize.X + 64
                        && pos.Y >= (int) view.Y - 64 && pos.Y <= view.Y + Settings.ScreenSize.Y + 64) {
                    target.Draw(gameObject);
                }
            }
        }

        public GameObject[] GetGameObjects() => gameObjects;
        public Vector2u GetSize() => size;
        public Vector2u GetViewPosition() => view;
    }
}