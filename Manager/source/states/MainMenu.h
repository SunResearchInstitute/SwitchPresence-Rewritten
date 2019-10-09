#pragma once

#include <vector>
#include <string>
#include "statemachine.h"
#include "../utils/Utils.h"

extern std::vector<std::string> MainMenuItems;

namespace states {
    class MainMenu : public State {
        public:
            virtual void enter();
            virtual void calc(StateMachine*, u64);
            virtual std::string name();

        private:
            signed int selection;
    };
};