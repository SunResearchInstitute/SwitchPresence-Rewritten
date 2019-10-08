#include "statemachine.h"

namespace states {
    StateMachine::StateMachine(){
        currentState = NULL;
        nextState = NULL;
    }

    void StateMachine::calc(u64 inputs) {
        if(nextState != NULL){
            if(currentState != NULL)
                currentState->exit();

            currentState = nextState;
            nextState = NULL;

            currentState->enter();
        }

        if(currentState == NULL){
            printf("State machine has no current state!");
            return;
        }

        currentState->calc(this, inputs);
    }

    void StateMachine::pushState(State* state){
        nextState = state;
    }

    void StateMachine::pushState(std::string str){
        for (auto & i : states) {
            if(i->name() == str){
                nextState = i;
                return;
            }
        }
    }

    StateMachine::~StateMachine() {
        for (auto & i : states) {
            delete i;
        }
    }
};