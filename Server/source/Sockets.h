#pragma once
#include <switch.h>


#define PACKETMAGIC 0xFFAADD23
#define PORT 0xCAFE

struct titlepacket
{
    u32 magic;
    u64 tid;
    char name[512];
};

int sendData(int sock, u64 tid, const char *name);
int setupSocketServer();
