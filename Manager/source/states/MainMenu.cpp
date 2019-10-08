#include "MainMenu.h"

namespace states {

    std::vector<std::string> MainMenuItems = {
        "Dump Icons",
        //Placeholder for enabled/disable
        "PlaceHldr"};


    void MainMenu::calc(StateMachine* stateMachine, u64 inputs)
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
        Utils::printItems(MainMenuItems, "MainMenu", selection);

        if(inputs & KEY_A){
            if(selection == 0){
                stateMachine->pushState("dumpRes");
            }
        }
    }

    std::string MainMenu::name(){
        return std::string("main");
    }
};