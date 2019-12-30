#include "main.h"

using namespace std;

int main(int argc, char **argv)
{
    consoleInit(nullptr);

    states::StateMachine stateMachine;

    stateMachine.states.push_back(new states::MainMenu());
    stateMachine.states.push_back(new states::DumpResState());
    stateMachine.states.push_back(new states::DumpCompleteState());
    stateMachine.states.push_back(new states::ErrorState());

    if (R_FAILED(states::ErrorState::error))
        stateMachine.pushState("error");
    else
        stateMachine.pushState("main");

    while (appletMainLoop())
    {
        hidScanInput();
        u64 kDown = hidKeysDown(CONTROLLER_P1_AUTO);
        if (kDown & KEY_PLUS || (kDown & KEY_B && stateMachine.currentState->name() == "main"))
            break;

        printf(CONSOLE_ESC(2J));
        stateMachine.calc(kDown);
        consoleUpdate(nullptr);
    }

    consoleExit(nullptr);
    return 0;
}

extern "C"
{
    void userAppInit(void)
    {
        Result rc;
        rc = pmshellInitialize();
        if (R_FAILED(rc))
            goto error;
        rc = pmdmntInitialize();
        if (R_FAILED(rc))
            goto error;
        rc = nsInitialize();

    error:
        states::ErrorState::error = rc;
    }

    void userAppExit(void)
    {
        nsExit();
        pmdmntExit();
        pmshellExit();
    }
}