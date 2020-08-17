#include <SFML/System.hpp>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <memory>
#include <string>
#include <settings.hpp>

using namespace gamejam;
using namespace settings;
using namespace settingshelper;

std::shared_ptr<sf::SoundBuffer> settingshelper::soundFromFile(std::string fileName) {
    sf::SoundBuffer sound;
    sound.loadFromFile(fileName);
    return std::make_shared<sf::SoundBuffer>(sound);
}

std::shared_ptr<sf::Sound> settingshelper::createSoundFromBuffer(std::shared_ptr<sf::SoundBuffer> buffer) {
    sf::Sound sound(*buffer);
    return std::make_shared<sf::Sound>(sound);
}

sf::Font settingshelper::fontFromFile(std::string fileName) {
    sf::Font font;
    font.loadFromFile(fileName);
    return font;
}

const sf::Vector2u settings::screenSize(1980, 1080);
const std::string settings::title("Game Jam Game-Engine");
const int settings::frameRate(30);

const sf::Color settings::defaultBgColor(sf::Color::Blue);
const sf::Color settings::defaultFillColor(60, 60, 100);
const sf::Color settings::defaultOutlineColor(10, 10, 10);

const sf::Font settings::defaultFont(
    settingshelper::fontFromFile("fonts/Ubuntu-R.ttf")
);

const std::shared_ptr<sf::SoundBuffer> settings::bgMusicFile(
    settingshelper::soundFromFile("sound/bg-music.wav")
);
const std::shared_ptr<sf::SoundBuffer> settings::popSfxFile(
    settingshelper::soundFromFile("sound/pop.wav")
);
const std::shared_ptr<sf::Sound> settings::bgMusic(
    createSoundFromBuffer(settings::bgMusicFile)
);
const std::shared_ptr<sf::Sound> settings::popSfx(
    createSoundFromBuffer(settings::popSfxFile)
);
