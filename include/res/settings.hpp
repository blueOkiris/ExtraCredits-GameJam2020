#pragma once

#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <string>

namespace gamejam {
    namespace settingshelper {
        sf::SoundBuffer soundFromFile(std::string fileName) {
            sf::SoundBuffer sound;
            sound.loadFromFile(fileName);
            return sound;
        }

        sf::Font fontFromFile(std::string fileName) {
            sf::Font font;
            font.loadFromFile(fileName);
            return font;
        }
    }

    namespace settings {
        extern sf::Vector2u screenSize(1980, 1080);
        extern std::string title("Game Jam Game-Engine");
        extern int frameRate(30);
        
        extern sf::Color defaultBgColor(sf::Color::Blue);
        extern sf::Color defaultFillColor(60, 60, 100);
        extern sf::Color defaultOutlineColor(10, 10, 10);

        extern sf::Font defaultFont(fontFromFile("fonts/Ubuntu-R.ttf"));

        extern sf::SoundBuffer bgMusicFile(soundFromFile("sound/bg-music.wav"));
        extern sf::SoundBuffer popSfxFile(soundFromFile("sound/pop.wav"));
        extern sf::Sound bgMusic(bgMusicFile);
        extern sf::Sound popSfx(popSfxFile);
    }
}