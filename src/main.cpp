#include <iostream>
#include <engine.hpp>
#include <res/settings.hpp>

namespace gamejame {
    void safelyDisposeMusic();

    int main(int argc, char **args) {
        auto engine = engine::Engine::getInstance();
        engine.windowThread.join();

        safelyDisposeMusic();

        return 0;
    }

    void safelyDisposeMusic() {
        settings::bgMusic.stop();
        settings::popSfx.stop();
        settings::bgMusic.dispose();
        settings::popSfx.dispose();

        settings::bgMusicFile.dispose();
        settings::popSfx.dispose();
    }
}
