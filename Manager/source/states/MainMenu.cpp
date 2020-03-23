#include "MainMenu.h"

namespace states
{
std::vector<std::string> MainMenuItems = {
    "Dump Icons",
    //Placeholder for enabled/disable
    "PlaceHldr"};

void MainMenu::enter()
{
    updateStatus();
}

void MainMenu::calc(StateMachine *stateMachine, u64 inputs)
{
    if (inputs & KEY_UP)
        selection--;

    if (inputs & KEY_DOWN)
        selection++;

    // check for under/overflow
    long int size = MainMenuItems.size();
    if (selection < 0)
        selection = size - 1;
    if (size <= selection)
        selection = 0;
    Utils::printItems(MainMenuItems, "Main Menu", selection);

    if (inputs & KEY_A)
        switch (selection)
        {
        case 0:
            stateMachine->pushState("dumpRes");
            break;
        case 1:
            PresenceState state = Utils::getPresenceState();
            if (state == PresenceState::Enabled)
            {
                if (R_SUCCEEDED(pmshellTerminateProgram(SYSMODULE_PROGRAMID)))
                    remove(BOOT2FLAG);
            }
            else if (state == PresenceState::Disabled)
            {
                u64 programId;
                NcmProgramLocation programLocation{
                    .program_id = SYSMODULE_PROGRAMID,
                    .storageID = NcmStorageId_None,
                };
                if (R_SUCCEEDED(pmshellLaunchProgram(0, &programLocation, &programId)))
                {
                    mkdir(FLAGSDIR, 0777);
                    fclose(fopen(BOOT2FLAG, "w"));
                }
            }
            updateStatus();
            break;
        }
}

void MainMenu::updateStatus()
{
    switch (Utils::getPresenceState())
    {
    case PresenceState::NotFound:
        MainMenuItems[1] = CONSOLE_RED "!!! SwitchPresence could not be found!";
        break;
    case PresenceState::Error:
        MainMenuItems[1] = CONSOLE_RED "!!! Failed to retreive the state of SwitchPresence!";
        break;
    case PresenceState::Disabled:
        MainMenuItems[1] = "SwitchPresence is disabled!";
        break;
    case PresenceState::Enabled:
        MainMenuItems[1] = "SwitchPresence is enabled!";
        break;
    }
}

std::string MainMenu::name()
{
    return std::string("main");
}
}; // namespace states