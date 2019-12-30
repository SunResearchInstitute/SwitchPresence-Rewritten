#pragma once

#include <string>
#include "../utils/Utils.h"
#include "statemachine.h"
#include "ErrorState.h"

namespace states
{
class DumpResState : public State
{
public:
    virtual std::string name();
    virtual void calc(states::StateMachine *, u64);
};
}; // namespace states