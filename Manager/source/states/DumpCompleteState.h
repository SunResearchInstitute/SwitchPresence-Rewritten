#pragma once

#include <vector>
#include <string>
#include "statemachine.h"

namespace states
{
class DumpCompleteState : public State
{
public:
    virtual std::string name();
    virtual void calc(states::StateMachine *, u64);
};
}; // namespace states