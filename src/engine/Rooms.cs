using System;
using System.Collections.Generic;
using System.Threading;
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

        private bool isPaused, canChangePause;

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

            var gameObjList = new List<GameObject>() {
                new MessageBox("Hello, world!", "hworld"),
                new MessageBox("Game Paused", "pause"),
                new TestPlayer(new Vector2f(512, 512)),
                new Block(
                    new Vector2f(256, 256),
                    TileOrientation.Solid,
                    Sprites.getInstance().GrassTiles
                )
            };

            for(int i = 0; i < size.X / 128; i++) {
                gameObjList.Add(
                    new Block(
                        new Vector2f(i * 128 + 64, 64),
                        TileOrientation.Solid,
                        Sprites.getInstance().GrassTiles
                    )
                );
                gameObjList.Add(
                    new Block(
                        new Vector2f(i * 128 + 64, size.Y - 64),
                        TileOrientation.Solid,
                        Sprites.getInstance().GrassTiles
                    )
                );
            }
            for(int i = 0; i < size.Y / 128; i++) {
                gameObjList.Add(
                    new Block(
                        new Vector2f(64, i * 128 + 64),
                        TileOrientation.Solid,
                        Sprites.getInstance().GrassTiles
                    )
                );
                gameObjList.Add(
                    new Block(
                        new Vector2f(size.X - 64, i * 128 + 64),
                        TileOrientation.Solid,
                        Sprites.getInstance().GrassTiles
                    )
                );
            }

            gameObjects = gameObjList.ToArray();
            Array.Sort(gameObjects);

            isPaused = false;
            canChangePause = true;
        }

        public Room ChangeIfShould() {
            return this;
        }

        public void Init() {
            Settings.BgMusic.Loop = true;
            Settings.BgMusic.Play();

            foreach(var gameObject in gameObjects) {
                gameObject.Init();
            }
        }

        public void Update(float deltaTime, KeyState keys) {
            if(!keys.Pause) {
                canChangePause = true;
            }

            if(keys.Pause && canChangePause) {
                canChangePause = false;
                isPaused = !isPaused;
            }

            (Engine.FindGameObjectsByTag("msg-box-pause", this)[0] as MessageBox).Show = isPaused;


            if(isPaused) {
                return;
            }

            foreach(var gameObject in gameObjects) {
                if(gameObject.GetTag() == "player") {
                    calculateView(gameObject.GetPosition());
                }

                if(gameObject.GetTag() == "msg-box-hworld") {
                    (gameObject as MessageBox).Show = keys.Fire1;
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

            if(keys.Fire2) {
                Settings.PopSfx.Play();
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            foreach(var gameObject in gameObjects) {
                var pos = gameObject.GetPosition();

                if(!gameObject.ShouldCull(this)) {
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