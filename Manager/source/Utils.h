#pragma once
#include <string>

#define center(p, c) ((p - c) / 2)

#define PresenceTID 0x0100000000000464
const std::string boot2Flag = "sdmc:/atmosphere/titles/0100000000000464/flags/boot2.flag";
namespace Utils
{
    bool isPresenceActive();
    Result DumpIcons();
    Result getAppControlData(u64 tid, NsApplicationControlData *appControlData);
}