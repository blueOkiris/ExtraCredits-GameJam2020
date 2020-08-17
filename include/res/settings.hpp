#pragma once

#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <memory>
#include <string>

namespace gamejam {
    namespace settingshelper {
        sf::SoundBuffer *soundFromFile(std::string fileName);
        sf::Font fontFromFile(std::string fileName);
    }

    namespace settings {
        extern const sf::Vector2u screenSize;
        extern const std::string title;
        extern const int frameRate;
        
        extern const sf::Color defaultBgColor;
        extern const sf::Color defaultFillColor;
        extern const sf::Color defaultOutlineColor;

        extern const sf::Font defaultFont;

        extern const sf::SoundBuffer *bgMusicFile;
        extern const sf::SoundBuffer *popSfxFile;
        extern sf::Sound *bgMusic;
        extern sf::Sound *popSfx;
    }
}