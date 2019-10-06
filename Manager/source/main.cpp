#include <switch.h>
#include <string>
#include "menu.h"
#include "Utils.h"

using namespace std;

int main(int argc, char **argv)
{
    consoleInit(nullptr);

    Result rc;
    rc = pmshellInitialize();
    if (R_FAILED(rc))
        Utils::startErrorScreen(rc);
    rc = pmdmntInitialize();
    if (R_FAILED(rc))
        Utils::startErrorScreen(rc);
    rc = nsInitialize();
    if (R_FAILED(rc))
        Utils::startErrorScreen(rc);

    MainMenuLoop();

    pmshellExit();
    pmdmntExit();
    nsExit();
    consoleExit(nullptr);
    return 0;
}
