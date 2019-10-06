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
    int lastScene = scene;
    Scene *currentScene;
    if (scene == 0)
    {
        currentScene = new MainMenu();
        
        if (!Utils::isPresenceActive())
            MainMenuItems[1] = "SwitchPresence is disabled!";
        else
            MainMenuItems[1] = "SwitchPresence is enabled!";

        Utils::printItems(MainMenuItems, "Main Menu");
    }
    else
        return;

    while (appletMainLoop())
    {
        if (lastScene != scene)
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
        }

        hidScanInput();
        u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
        if (kDown & KEY_PLUS || (scene == 0 && kDown & KEY_B))
        {
            delete currentScene;
            return;
        }
        currentScene->Display(kDown);
        if (lastScene != scene)
        {
            lastScene = scene;
            delete currentScene;
        }
        consoleUpdate(nullptr);
    }
}
