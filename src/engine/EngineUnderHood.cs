using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace engine {
    class KeyState {
        public bool Quit = false;

        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;

        public bool Jump = false;
        public bool Fire1 = false;
        public bool Fire2 = false;
    }

    partial class Engine {
        private static Engine instance = null;
        private static Mutex stateLock = new Mutex();

        public static Engine getInstance() {
            if(instance == null) {
                instance = new Engine();
            }

            return instance;
        }

        public readonly Thread windowThread;

        private RenderWindow window;
        private KeyState keys;
        private Thread updateThread;

        private Engine() {
            keys = new KeyState();

            updateThread = new Thread(new ThreadStart(updateLoop));

            windowThread = new Thread(new ThreadStart(renderLoop));
            windowThread.Start();
        }

        private void renderLoop() {
            window = new RenderWindow(
                new VideoMode(Settings.ScreenSize.X, Settings.ScreenSize.Y),
                Settings.Title,
                Styles.Fullscreen | Styles.Close
            );
            window.Closed += (object sender, EventArgs e) => (sender as RenderWindow).Close();
            window.KeyPressed += (object sender, KeyEventArgs e) => keyPressed(e);
            window.KeyReleased += (object sender, KeyEventArgs e) => keyReleased(e);

            init();
            updateThread.Start();

            while(window.IsOpen) {
                stateLock.WaitOne();
                window.DispatchEvents();
                window.Clear(Settings.DefaultBgColor);

                draw();
                
                window.Display();
                stateLock.ReleaseMutex();
            }
        }

        private void updateLoop() {
            var watch = new Stopwatch();
            float expectedDeltaTimeS = 1 / ((float) Settings.FrameRate);
            float deltaTimeS = expectedDeltaTimeS;
            float timeDifferenceMs = 0;

            while(window.IsOpen) {
                watch.Reset();
                watch.Start();
                stateLock.WaitOne();
                
                update(deltaTimeS, keys);

                stateLock.ReleaseMutex();
                watch.Stop();
                deltaTimeS = ((float) watch.Elapsed.Milliseconds) / 1000;
                
                if((timeDifferenceMs = 1000 * (deltaTimeS - expectedDeltaTimeS)) > 100) {
                    Thread.Sleep((int) timeDifferenceMs - 100);
                }
            }
        }

        private void keyPressed(object eventArgs) {
            var keyEventArgs = eventArgs as KeyEventArgs;
            stateLock.WaitOne();
            keys.Quit = keyEventArgs.Code == Keyboard.Key.Escape ? true : keys.Quit;
            keys.Up = (keyEventArgs.Code == Keyboard.Key.Up || keyEventArgs.Code == Keyboard.Key.W) ? true : keys.Up;
            keys.Down = (keyEventArgs.Code == Keyboard.Key.Down || keyEventArgs.Code == Keyboard.Key.S) ? true : keys.Down;
            keys.Left = (keyEventArgs.Code == Keyboard.Key.Left || keyEventArgs.Code == Keyboard.Key.A) ? true : keys.Left;
            keys.Right = (keyEventArgs.Code == Keyboard.Key.Right || keyEventArgs.Code == Keyboard.Key.D) ? true : keys.Right;
            keys.Jump = keyEventArgs.Code == Keyboard.Key.Space ? true : keys.Jump;
            keys.Fire1 = (keyEventArgs.Code == Keyboard.Key.LShift || keyEventArgs.Code == Keyboard.Key.Z) ? true : keys.Fire1;
            keys.Fire2 = (keyEventArgs.Code == Keyboard.Key.LControl || keyEventArgs.Code == Keyboard.Key.X) ? true : keys.Fire2;
            stateLock.ReleaseMutex();
        }

        private void keyReleased(object eventArgs) {
            var keyEventArgs = eventArgs as KeyEventArgs;
            stateLock.WaitOne();
            keys.Quit = keyEventArgs.Code == Keyboard.Key.Escape ? false : keys.Quit;
            keys.Up = (keyEventArgs.Code == Keyboard.Key.Up || keyEventArgs.Code == Keyboard.Key.W) ? false : keys.Up;
            keys.Down = (keyEventArgs.Code == Keyboard.Key.Down || keyEventArgs.Code == Keyboard.Key.S) ? false : keys.Down;
            keys.Left = (keyEventArgs.Code == Keyboard.Key.Left || keyEventArgs.Code == Keyboard.Key.A) ? false : keys.Left;
            keys.Right = (keyEventArgs.Code == Keyboard.Key.Right || keyEventArgs.Code == Keyboard.Key.D) ? false : keys.Right;
            keys.Jump = keyEventArgs.Code == Keyboard.Key.Space ? false : keys.Jump;
            keys.Fire1 = (keyEventArgs.Code == Keyboard.Key.LShift || keyEventArgs.Code == Keyboard.Key.Z) ? false : keys.Fire1;
            keys.Fire2 = (keyEventArgs.Code == Keyboard.Key.LControl || keyEventArgs.Code == Keyboard.Key.X) ? false : keys.Fire2;
            stateLock.ReleaseMutex();
        }

        private static bool intersect(IntRect a, IntRect b) {
            return Math.Max(a.Left, b.Left) < Math.Min(a.Left + a.Width, b.Left + b.Width)
                && Math.Max(a.Top, b.Top) < Math.Min(a.Top + a.Height, b.Top + b.Height);
        }

        public static bool IsPlaceMeeting(Vector2f pos, GameObject self, GameObject[] objects) {
            var center = self.GetSpriteIndex().Origin;
            var mask = self.GetMask();

            var relativeMask = new IntRect(
                (int) (pos.X - center.X + mask.Left),
                (int) (pos.Y - center.X + mask.Top),
                mask.Width, mask.Height
            );

            foreach(var obj in objects) {
                var otherPos = obj.GetPosition();
                var otherCenter = obj.GetSpriteIndex().Origin;
                var otherMask = obj.GetMask();

                var otherRelativeMask = new IntRect(
                    (int) (otherPos.X - otherCenter.X + otherMask.Left),
                    (int) (otherPos.Y - otherCenter.Y + otherMask.Top),
                    otherMask.Width, otherMask.Height
                );

                if(intersect(relativeMask, otherRelativeMask)) {
                    return true;
                }
            }

            return false;
        }

        public static GameObject[] FindGameObjectsByTag(string tag, Room room) {
            var taggedObjects = new List<GameObject>();

            GameObject[] objs = room.GetGameObjects();
            foreach(var gameObj in objs) {
                if(gameObj.GetTag() == tag) {
                    taggedObjects.Add(gameObj);
                }
            }

            return taggedObjects.ToArray();
        }
    }
}