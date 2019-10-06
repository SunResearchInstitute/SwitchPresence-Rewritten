#include "DumpResMenu.h"
#include "MainMenu.h"

using namespace std;

DumpResMenu::DumpResMenu() {}
DumpResMenu::~DumpResMenu() {}

void DumpResMenu::Display(u64 kDown)
{
    if (kDown & KEY_B)
    {
        scene = 0;
        printf(CONSOLE_ESC(2J));
        Utils::printItems(MainMenuItems, "Main Menu");
    }
}
