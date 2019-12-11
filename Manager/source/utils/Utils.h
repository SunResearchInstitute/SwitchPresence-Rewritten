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
#define FLAGSDIR "sdmc:/atmosphere/contents/0100000000000464/flags/"
#define BOOT2FLAG FLAGSDIR "boot2.flag"
namespace Utils
{
extern Result error_currentError;

void printItems(const std::vector<std::string> &items, std::string menuTitle, int);
bool isPresenceActive();
Result DumpIcons();
Result getAppControlData(u64, NsApplicationControlData *);
} // namespace Utils