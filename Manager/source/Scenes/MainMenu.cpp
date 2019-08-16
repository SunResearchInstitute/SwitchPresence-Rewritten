#include "MainMenu.h"
#include "../Utils.h"
#include <string.h>

using namespace std;

vector<string> MainMenuItems = {
    "Dump Icons",
    //Placeholder for enabled/disable
    "PlaceHldr"};

MainMenu::MainMenu() {}
MainMenu::~MainMenu() {}

void MainMenu::Display(u64 kDown)
{
    bool needsRefresh = false;
    if (kDown & KEY_UP)
    {
        selection--;
        needsRefresh = true;
    }
    if (kDown & KEY_DOWN)
    {
        selection++;
        needsRefresh = true;
    }

    // check for under/overflow
    long int size = MainMenuItems.size();
    if (selection < 0)
        selection = size - 1;
    if (size <= selection)
        selection = 0;
    if (kDown & KEY_A)
    {
        switch (selection)
        {
        case 0:
        {
            scene = 1;
            selection = 0;
            printf(CONSOLE_ESC(2J));
            printf("\x1b[21;%dHDumping Icons...", center(80, 10));
            printf("\x1b[22;%dHPlease Wait!", center(80, 12));
            consoleUpdate(nullptr);
            Result rc = Utils::DumpIcons();
            printf(CONSOLE_ESC(2J));
            if (R_FAILED(rc))
            {
                char str[50];
                sprintf(str, "Dump Failed, Error: 0x%x", rc);
                printf(CONSOLE_RED "\x1b[21;%dH%s\n", center(80, (int)strlen(str)), str);
            }
            else
                printf(CONSOLE_GREEN "\x1b[21;34HDump Success!\n");

            printf("\x1b[22;10HPress `+` to exit, or `B` to go back to the previous screen!");
            break;
        }
        case 1:
        {
            if (Utils::isPresenceActive())
            {
                if (R_SUCCEEDED(pmshellTerminateProcessByTitleId(PresenceTID)))
                {
                    MainMenuItems[1] = "SwitchPresence is disabled!";
                    remove(boot2Flag.c_str());
                }
            }
            else
            {
                u64 pid;
                if (R_SUCCEEDED(pmshellLaunchProcess(0, PresenceTID, FsStorageId_None, &pid)))
                {
                    MainMenuItems[1] = "SwitchPresence is enabled!";
                    fclose(fopen(boot2Flag.c_str(), "w"));
                }
            }
            needsRefresh = true;
            break;
        }
        }
    }

    if (needsRefresh)
    {
        printf(CONSOLE_ESC(2J));
        printItems(MainMenuItems, "Main Menu");
    }
}