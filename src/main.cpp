#include <iostream>
#include <engine.hpp>
#include <settings.hpp>

namespace gamejam {
    void safelyDisposeMusic();

    int main(int argc, char **args) {
        auto engine = engine::Engine::getInstance();
        engine->joinWindowThread();

        safelyDisposeMusic();

        return 0;
    }

    void safelyDisposeMusic() {
        settings::bgMusic->stop();
        settings::popSfx->stop();
        delete settings::bgMusic.get();
        delete settings::popSfx.get();

        delete settings::bgMusicFile.get();
        delete settings::popSfx.get();
    }
}
