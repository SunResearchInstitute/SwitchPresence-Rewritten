#include "DumpCompleteState.h"

namespace states
{
void DumpCompleteState::calc(states::StateMachine *stateMachine, u64 kDown)
{
    printf("\x1b[22;10HPress `+` to exit, or `B` to go back to the previous screen.");

    if (kDown & KEY_B)
        stateMachine->pushState("main");
}

std::string DumpCompleteState::name()
{
    return "dumpComplete";
}
}; // namespace states