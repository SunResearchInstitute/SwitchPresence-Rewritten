#pragma once

#include <switch.h>
#include <string>
#include <cstring>
#include <sstream>
#include <filesystem>
#include <sys/stat.h>
#include <gd.h>
#include <vector>

#define center(p, c) ((p - c) / 2)

#define TID 0x0100000000000464
#define CONTENTSDIR "sdmc:/atmosphere/contents/0100000000000464"
#define PROGRAMDIR CONTENTSDIR "/exefs.nsp"
#define FLAGSDIR CONTENTSDIR "/flags"
#define BOOT2FLAG FLAGSDIR "/boot2.flag"

enum class PresenceState
{
    NotFound,
    Error,
    Enabled,
    Disabled,
};

namespace Utils
{
void printItems(const std::vector<std::string> &items, std::string menuTitle, int);
PresenceState getPresenceState();
Result DumpIcons();
Result getAppControlData(u64, NsApplicationControlData *);
} // namespace Utils