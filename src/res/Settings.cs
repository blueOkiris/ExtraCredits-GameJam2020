using SFML.Graphics;
using SFML.System;

namespace engine {
    class Settings {
        public static readonly Vector2u ScreenSize = new Vector2u(1920, 1080);
        public static readonly string Title = "4D Tic Tac Toe";
        public static readonly Color DefaultBgColor = Color.Blue;
        public static readonly int FrameRate = 30;
    }
}