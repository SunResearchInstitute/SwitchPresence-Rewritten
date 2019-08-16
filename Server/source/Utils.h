#pragma once
#include <switch.h>
#include <string>

namespace Utils
{
std::string getAppName(u64 tid);
NsApplicationControlData *getAppControlData(u64 tid);
} // namespace Utils
