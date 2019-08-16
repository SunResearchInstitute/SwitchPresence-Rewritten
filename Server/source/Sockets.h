#pragma once
#include <switch.h>
#include <string>

#define TITLE_MAGIC 0xffaadd23

struct titlepacket
{
    u32 magic;
    u64 tid;
    char name[512];
};

int sendData(int sock, u64 tid, std::string name);
int setupSocketServer();
