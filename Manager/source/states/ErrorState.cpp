#include "ErrorState.h"

namespace states
{
Result ErrorState::error = 0;

void ErrorState::calc(StateMachine *, u64)
{
    char str[50];
    sprintf(str, "Error: 0x%x", ErrorState::error);
    printf(CONSOLE_RED "\x1b[21;%dH%s", center(80, (int)strlen(str)), str);
    printf(CONSOLE_RED "\x1b[23;%dHPress `+` to exit!", center(80, 17));
}

std::string ErrorState::name()
{
    return "error";
}
}; // namespace states