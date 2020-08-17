using System;
using System.Threading;

namespace engine {
    class Program {
        public static void Main(string[] args) {
            var engine = Engine.getInstance();
            engine.windowThread.Join();

            SafelyDisposeMusic();
        }

        private static void SafelyDisposeMusic() {
            Settings.BgMusic.Stop();
            Settings.PopSfx.Stop();
            Settings.BgMusic.Dispose();
            Settings.PopSfx.Dispose();

            Settings.BgMusicFile.Dispose();
            Settings.PopSfxFile.Dispose();
        }
    }
}
