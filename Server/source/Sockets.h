#pragma once
#include <switch.h>


#define PACKETMAGIC 0xFFAADD23
#define PORT 0xCAFE

struct titlepacket
{
    u32 magic;
    u64 programId;
    char name[612];
};

int sendData(u64 programId, const char *name);
Result setupSocketServer();
void closeSocketServer();
