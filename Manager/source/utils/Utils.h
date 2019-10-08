#pragma once

#include <switch.h>
#include <string>
#include <cstring>
#include <sstream>
#include <filesystem>
#include <sys/stat.h>
#include <gd.h>


#define center(p, c) ((p - c) / 2)

#define PresenceTID 0x0100000000000464
const std::string boot2Flag = "sdmc:/atmosphere/titles/0100000000000464/flags/boot2.flag";
namespace Utils
{
    extern Result error_currentError;

    void printItems(const std::vector<std::string> &, std::string , int);
    bool isPresenceActive();
    Result DumpIcons();
    Result getAppControlData(u64, NsApplicationControlData *);
}