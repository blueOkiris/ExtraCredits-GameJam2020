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
        View GetView();
    }

    class TestRoom : Room
    {
        private GameObject[] gameObjects;
        private Vector2u size;
        private Vector2u viewPos;
        private View view;

        public TestRoom() {
            size = new Vector2u(2560, 1280);
            viewPos = new Vector2u(0, 0);
            view = new View(
                new Vector2f(
                    Settings.ScreenSize.X / 2 + viewPos.X,
                    Settings.ScreenSize.Y / 2 + viewPos.Y
                ), 
                (Vector2f) Settings.ScreenSize
            );

            gameObjects = new GameObject[] {
                new MessageBox("Hello, world!"),
                new TestPlayer(new Vector2f(512, 512))
            };
            Array.Sort(gameObjects);
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
                if(gameObject.GetTag() == "player") {
                    calculateView(gameObject.GetPosition());
                }

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

                if(!gameObject.ShouldCull()) {
                    target.Draw(gameObject);
                }
            }
        }

        private void calculateView(Vector2f pos) {
            var viewX = (int) pos.X - Settings.ScreenSize.X / 2;
            var viewY = (int) pos.Y - Settings.ScreenSize.Y / 2;

            if(viewX < 0) {
                viewX = 0;
            } else if(viewX + Settings.ScreenSize.X > size.X) {
                viewX = size.X - Settings.ScreenSize.X;
            }
            if(viewY < 0) {
                viewY = 0;
            } else if(viewY + Settings.ScreenSize.Y > size.Y) {
                viewY = size.Y - Settings.ScreenSize.Y;
            }
            
            viewPos.X = (uint) viewX;
            viewPos.Y = (uint) viewY;

            view.Center = new Vector2f(
                Settings.ScreenSize.X / 2 + viewPos.X,
                Settings.ScreenSize.Y / 2 + viewPos.Y
            );
        }

        public GameObject[] GetGameObjects() => gameObjects;
        public Vector2u GetSize() => size;
        public Vector2u GetViewPosition() => viewPos;
        public View GetView() => view;
    }
}