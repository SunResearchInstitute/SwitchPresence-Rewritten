#pragma once
#include "scene.h"
#include <vector>
#include <string>

extern std::vector<std::string> MainMenuItems;

class MainMenu : public Scene
{
public:
    virtual void Display(u64);
    virtual ~MainMenu();
    MainMenu();
};