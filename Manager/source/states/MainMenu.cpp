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
        switch(selection){
            case 0:
                stateMachine->pushState("dumpRes");
                break;
            case 1:
                if (Utils::isPresenceActive())
                {
                    if (R_SUCCEEDED(pmshellTerminateProcessByTitleId(TID)))
                        remove(BOOT2FLAG);
                }
                else
                {
                    u64 pid;
                    if (R_SUCCEEDED(pmshellLaunchProcess(0, TID, FsStorageId::FsStorageId_None, &pid))) {
                        mkdir(FLAGSDIR, 0777);
                        fclose(fopen(BOOT2FLAG, "w"));
                    }
                }
                updateStatus();
                break;
    }
}

void MainMenu::updateStatus(){
    if (Utils::isPresenceActive())
        MainMenuItems[1] = "SwitchPresence is enabled!";
    else
        MainMenuItems[1] = "SwitchPresence is disabled!";
}

std::string MainMenu::name()
{
    return std::string("main");
}
}; // namespace states