#include "main.h"

using namespace std;

int main(int argc, char **argv)
{
    consoleInit(nullptr);
    
    Utils::error_currentError = init();

    states::StateMachine stateMachine;

    stateMachine.states.push_back(new states::MainMenu());
    stateMachine.states.push_back(new states::DumpResState());
    stateMachine.states.push_back(new states::DumpCompleteState());
    stateMachine.states.push_back(new states::ErrorState());

    if (R_FAILED(Utils::error_currentError))
        stateMachine.pushState("error");
    else
        stateMachine.pushState("main");

    while (appletMainLoop())
    {
        hidScanInput();
        u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
        if (kDown & KEY_PLUS)
            break;

        printf(CONSOLE_ESC(2J));
        stateMachine.calc(kDown);
        consoleUpdate(nullptr);
    }

    nsExit();
    pmdmntExit();
    pmshellExit();

    consoleExit(nullptr);
    return 0;
}

Result init()
{
    Result rc;
    rc = pmshellInitialize();
    if (R_FAILED(rc))
        return rc;
    rc = pmdmntInitialize();
    if (R_FAILED(rc))
        return rc;
    rc = nsInitialize();

    return rc;
}