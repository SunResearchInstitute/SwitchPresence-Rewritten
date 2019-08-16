#include <switch.h>
#include <string>
#include "menu.h"

using namespace std;

int main(int argc, char **argv)
{
    consoleInit(nullptr);
    Result rc;
    rc = pmshellInitialize();
    if (R_FAILED(rc))
    {
        printf(CONSOLE_RED "Error: 0x%x\n", rc);
        printf(CONSOLE_RED "Press `+` to exit!");
        consoleUpdate(nullptr);
        while (appletMainLoop())
        {
            hidScanInput();
            u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
            if (kDown & KEY_PLUS)
            {
                consoleExit(nullptr);
                return 0;
            }
        }
    }
    rc = pmdmntInitialize();
    if (R_FAILED(rc))
    {
        printf(CONSOLE_RED "Error: 0x%x\n", rc);
        printf(CONSOLE_RED "Press `+` to exit!");
        consoleUpdate(nullptr);
        while (appletMainLoop())
        {
            hidScanInput();
            u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
            if (kDown & KEY_PLUS)
            {
                consoleExit(nullptr);
                return 0;
            }
        }
    }
    rc = nsInitialize();
    if (R_FAILED(rc))
    {
        printf(CONSOLE_RED "Error: 0x%x\n", rc);
        printf(CONSOLE_RED "Press `+` to exit!");
        consoleUpdate(nullptr);
        while (appletMainLoop())
        {
            hidScanInput();
            u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
            if (kDown & KEY_PLUS)
            {
                consoleExit(nullptr);
                return 0;
            }
        }
    }
    MainMenuLoop();
    
    pmshellExit();
    pmdmntExit();
    nsExit();
    consoleExit(nullptr);
    return 0;
}
