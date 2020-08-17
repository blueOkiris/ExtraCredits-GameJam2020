#pragma once

#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <memory>
#include <string>

namespace gamejam {
    namespace settingshelper {
        std::shared_ptr<sf::SoundBuffer> soundFromFile(std::string fileName);
        std::shared_ptr<sf::Sound> createSoundFromBuffer(std::shared_ptr<sf::SoundBuffer> buffer);
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

        extern const std::shared_ptr<sf::SoundBuffer> bgMusicFile;
        extern const std::shared_ptr<sf::SoundBuffer> popSfxFile;
        extern const std::shared_ptr<sf::Sound> bgMusic;
        extern const std::shared_ptr<sf::Sound> popSfx;
    }
}