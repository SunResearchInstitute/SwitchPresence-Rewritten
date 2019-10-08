#include "DumpResState.h"

namespace states {
    void DumpResState::calc(states::StateMachine* stateMachine, u64){
        printf("\x1b[21;%dHDumping Icons...", center(80, 16));
        printf("\x1b[22;%dHPlease Wait!", center(80, 12));
        consoleUpdate(NULL); // long task ahead, manually refresh
        Result rc = Utils::DumpIcons();

        if (R_FAILED(rc))
        {
            Utils::error_currentError = rc;
            stateMachine->pushState("error");
        }
        else
            stateMachine->pushState("dumpComplete");
    }

    std::string DumpResState::name() {
        return "dumpRes";
    }
};