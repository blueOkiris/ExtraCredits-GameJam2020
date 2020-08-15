using SFML.Graphics;
using SFML.System;
using SFML.Audio;

namespace engine {
    class Settings {
        public static readonly Vector2u ScreenSize = new Vector2u(1920, 1080);
        public static readonly string Title = "4D Tic Tac Toe";
        public static readonly int FrameRate = 30;

        public static readonly Color DefaultBgColor = Color.Blue;
        public static readonly Color DefaultFillColor = new Color(60, 60, 100);
        public static readonly Color DefaultOutlineColor = new Color(10, 10, 30);

        public static readonly Font DefaultFont = new Font("fonts/Ubuntu-R.ttf");

        public static readonly SoundBuffer BgMusicFile = new SoundBuffer("sound/bg-music.wav");
        public static readonly SoundBuffer PopSfxFile = new SoundBuffer("sound/pop.wav");
        public static readonly Sound BgMusic = new Sound(BgMusicFile);
        public static readonly Sound PopSfx = new Sound(PopSfxFile);
    }
}