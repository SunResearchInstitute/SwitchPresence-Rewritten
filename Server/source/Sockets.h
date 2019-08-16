#pragma once
#include <switch.h>
#include <string>

#define TITLE_MAGIC 0xffaadd23

struct PACKED titlepacket
{
    u32 magic;
    u64 tid;
    char name[512];
};

//VS code always returns an error but compiler should get it right.
static_assert(sizeof(titlepacket) == 0x20C, "Invalid titlepacket size");

int sendData(int sock, u64 tid, std::string name);
int setupSocketServer();
