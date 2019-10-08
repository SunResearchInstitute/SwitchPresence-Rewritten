#pragma once

#include "statemachine.h"
#include "../utils/Utils.h"

namespace states
{
    class ErrorState : public State {
        virtual std::string name();
        virtual void calc(StateMachine*, u64);
    };
};