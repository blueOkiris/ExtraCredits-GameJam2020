#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <thread>
#include <mutex>
#include <memory>
#include <objects.hpp>
#include <rooms.hpp>
#include <settings.hpp>
#include <engine.hpp>

using namespace gamejam;
using namespace engine;

static std::shared_ptr<Engine> instance = nullptr;

std::shared_ptr<Engine> Engine::getInstance() {
    if(instance == nullptr) {
        instance = std::make_shared<Engine>(Engine());
    }

    return instance;
}

void Engine::init() {

}

void Engine::draw() {

}

void Engine::update(double deltaTimeS, KeyState keys) {
    
}

Engine::Engine() {
    _windowThread = std::make_shared<std::thread>(
        std::thread(
            [&] () {
                std::mutex lock;

                _window = new sf::RenderWindow(
                    sf::VideoMode(settings::screenSize.x, settings::screenSize.y),
                    settings::title,
                    sf::Style::Close | sf::Style::Fullscreen
                );

                init();

                std::thread updateThread(
                    [&] () {
                        auto startTime = std::chrono::system_clock::now();
                        auto endTime = std::chrono::system_clock::now();
                        double deltaTimeS = std::chrono::duration_cast<std::chrono::milliseconds>(endTime - startTime).count();
                        double expectedDeltaTimeS = 1 / static_cast<double>(settings::frameRate);
                        double timeDifferenceS = 0;

                        while(_window->isOpen()) {
                            startTime = std::chrono::system_clock::now();
                            lock.lock();

                            update(deltaTimeS, _keys);

                            lock.unlock();
                            endTime = std::chrono::system_clock::now();
                            double deltaTimeS = std::chrono::duration_cast<std::chrono::milliseconds>(endTime - startTime).count();

                            if((timeDifferenceS = (deltaTimeS - expectedDeltaTimeS)) > 0.1) {
                                std::this_thread::sleep_for(std::chrono::duration<double>(timeDifferenceS - 0.1));
                            }
                        }
                    }
                );

                while(_window->isOpen()) {
                    lock.lock();

                    _window->clear(settings::defaultBgColor);
                    draw();
                    _window->display();

                    sf::Event event;
                    _window->pollEvent(event);

                    switch(event.type) {
                        case sf::Event::Closed:
                            _window->close();
                            break;

                        case sf::Event::KeyPressed:
                            keyPressed(event.key.code);
                            break;

                        case sf::Event::KeyReleased:
                            keyReleased(event.key.code);
                            break;

                        default:
                            break;
                    }

                    lock.unlock();
                }
            }
        )
    );
}

void Engine::keyPressed(sf::Keyboard::Key key) {
    _keys.quit = key == sf::Keyboard::Key::Escape ? true : _keys.quit;
    _keys.pause = key == sf::Keyboard::Key::P ? true : _keys.pause;
    _keys.up = (key == sf::Keyboard::Key::Up || key == sf::Keyboard::Key::W) ? true : _keys.up;
    _keys.down = (key == sf::Keyboard::Key::Down || key == sf::Keyboard::Key::S) ? true : _keys.down;
    _keys.left = (key == sf::Keyboard::Key::Left || key == sf::Keyboard::Key::A) ? true : _keys.left;
    _keys.right = (key == sf::Keyboard::Key::Right || key == sf::Keyboard::Key::D) ? true : _keys.right;
    _keys.jump = key == sf::Keyboard::Key::Space ? true : _keys.jump;
    _keys.fire1 = (key == sf::Keyboard::Key::Z || key == sf::Keyboard::Key::RShift) ? true : _keys.fire1;
    _keys.fire2 = (key == sf::Keyboard::Key::X || key == sf::Keyboard::Key::RControl) ? true : _keys.fire2;
}

void Engine::keyReleased(sf::Keyboard::Key key) {
    _keys.quit = key == sf::Keyboard::Key::Escape ? false : _keys.quit;
    _keys.pause = key == sf::Keyboard::Key::P ? false : _keys.pause;
    _keys.up = (key == sf::Keyboard::Key::Up || key == sf::Keyboard::Key::W) ? false : _keys.up;
    _keys.down = (key == sf::Keyboard::Key::Down || key == sf::Keyboard::Key::S) ? false : _keys.down;
    _keys.left = (key == sf::Keyboard::Key::Left || key == sf::Keyboard::Key::A) ? false : _keys.left;
    _keys.right = (key == sf::Keyboard::Key::Right || key == sf::Keyboard::Key::D) ? false : _keys.right;
    _keys.jump = key == sf::Keyboard::Key::Space ? false : _keys.jump;
    _keys.fire1 = (key == sf::Keyboard::Key::Z || key == sf::Keyboard::Key::RShift) ? false : _keys.fire1;
    _keys.fire2 = (key == sf::Keyboard::Key::X || key == sf::Keyboard::Key::RControl) ? false : _keys.fire2;
}

void Engine::joinWindowThread() {
    _windowThread->join();
}

bool intersect(sf::IntRect a, sf::IntRect b) {
    return std::max(a.left, b.left) < std::min(a.left + a.width, b.left + b.width)
        && std::max(a.top, b.top) < std::min(a.top + a.height, b.top + b.height);
}

bool Engine::isPlaceMeeting(sf::Vector2f pos, GameObject self, std::vector<GameObject> objects) {
    /*auto center = self.getSpriteIndex().Origin;
    auto mask = self.getMask();

    sf::IntRect relativeMask(
        (int) (pos.x - center.x + mask.left),
        (int) (pos.y - center.y + mask.top),
        mask.width, mask.height
    );

    for(auto obj : objects) {
        auto otherPos = obj.getPosition();
        auto otherCenter = obj.getSpriteIndex().Origin;
        auto otherMask = obj.getMask();

        sf::IntRect otherRelativeMask(
            (int) (otherPos.x - otherCenter.x + otherMask.left),
            (int) (otherPos.y - otherCenter.y + otherMask.top),
            otherMask.width, otherMask.height
        );

        if(intersect(relativeMask, otherRelativeMask)) {
            return true;
        }
    }*/

    return false;
}

std::vector<GameObject> Engine::findGameObjectsByTag(std::string tag, Room room) {
    std::vector<GameObject> taggedObjects;

    /*
    auto objs = room.getGameObjects();
    for(auto gameObj : objs) {
        if(gameObj.getTag() == tag) {
            taggedObjects.push_back(gameObj);
        }
    }
    */

   return taggedObjects;
}
