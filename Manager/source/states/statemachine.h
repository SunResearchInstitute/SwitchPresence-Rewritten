#pragma once

#include <string>
#include <vector>
#include "switch.h"

namespace states
{
class StateMachine;

class State
{
public:
    virtual std::string name() = 0;

    virtual void enter(){};
    virtual void calc(StateMachine *, u64){};
    virtual void exit(){};
    virtual ~State(){};
};

class StateMachine
{
public:
    std::vector<State *> states;
    State *currentState;
    State *nextState;

    StateMachine();

    void calc(u64);

    void pushState(State *);
    void pushState(std::string);
    ~StateMachine();
};
}; // namespace states