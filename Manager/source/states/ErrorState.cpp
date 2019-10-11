#include "ErrorState.h"

namespace states
{

void ErrorState::calc(StateMachine *, u64)
{
    char str[50];
    sprintf(str, "Error: 0x%x", Utils::error_currentError);
    printf(CONSOLE_RED "\x1b[21;%d%s", center(80, (int)strlen(str)), str);
    printf(CONSOLE_RED "\x1b[22;%dPress `+` to exit!", center(80, 17));
}

std::string ErrorState::name()
{
    return "error";
}
}; // namespace states