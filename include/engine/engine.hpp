#pragma once

#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <thread>
#include <chrono>
#include <mutex>
#include <objects.hpp>
#include <rooms.hpp>
#include <engine.hpp>

namespace gamejam {
    namespace engine {
        struct KeyState {
            bool quit = false;
            bool pause = false;

            bool up = false;
            bool down = false;
            bool left = false;
            bool right = false;
            bool jump = false;

            bool fire1 = false;
            bool fire2 = false;
        };

        class Engine {
            private:
                std::thread _windowThread;
                std::thread _updateThread;
                KeyState _keys;
                std::shared_ptr<sf::RenderWindow> _window;
                std::mutex _lock;
                
                Engine();

                void init();
                void draw();
                void update(double deltaTimeS, KeyState keys);

                void keyPressed(sf::Keyboard::Key key);
                void keyReleased(sf::Keyboard::Key key);

            public:
                static std::shared_ptr<Engine> getInstance();
                static bool isPlaceMeeting(sf::Vector2f pos, GameObject self, std::vector<GameObject> objects);
                static std::vector<GameObject> findGameObjectsByTag(std::string tag, Room room);

                void joinWindowThread();
        };
    }
}