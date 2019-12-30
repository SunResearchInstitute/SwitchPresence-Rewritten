#pragma once

#include "statemachine.h"
#include "../utils/Utils.h"

namespace states
{
class ErrorState : public State
{
public:
    virtual std::string name();
    virtual void calc(StateMachine *, u64);
    static Result error;
};
}; // namespace states