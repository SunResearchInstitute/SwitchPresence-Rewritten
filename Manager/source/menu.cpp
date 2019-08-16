#include <switch.h>
#include <vector>
#include "Scenes/scene.h"
#include <string>
#include "Scenes/scenes.h"
#include "Utils.h"

signed int selection = 0;
int scene = 0;

using namespace std;

void MainMenuLoop()
{
    if (!Utils::isPresenceActive())
        MainMenuItems[1] = "SwitchPresence is disabled!";
    else
        MainMenuItems[1] = "SwitchPresence is enabled!";

    printItems(MainMenuItems, "Main Menu");
    Scene *currentScene;

    while (appletMainLoop())
    {
        switch (scene)
        {
        case 0:
            currentScene = new MainMenu();
            break;
            case 1:
            currentScene = new DumpResMenu();
            break;
        default:
            return;
        }

        hidScanInput();
        u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
        if (kDown & KEY_PLUS || (scene == 0 && kDown & KEY_B))
        {
            delete currentScene;
            return;
        }
        currentScene->Display(kDown);
        delete currentScene;
        consoleUpdate(nullptr);
    }
}

void printItems(const vector<string> &items, string menuTitle)
{
    printf(CONSOLE_MAGENTA "\x1b[0;%dH%s\n", (40 - ((int)menuTitle.size() / 2)), menuTitle.c_str());
    for (int i = 0; i < (int)items.size(); i++)
    {
        const char *prefix = " ";
        if (selection == i)
            prefix = ">";
        printf(CONSOLE_WHITE "%s%s", prefix, items[i].c_str());

        printf("\n");
    }
}