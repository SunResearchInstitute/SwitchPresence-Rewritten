#pragma once
#include "switch.h"
#include "../menu.h"
#include "../Utils.h"

class Scene
{
public:
    virtual void Display(u64) = 0;
    virtual ~Scene() {};
};
