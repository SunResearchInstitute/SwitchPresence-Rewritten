#include "Sockets.h"
#include "Utils.h"

extern "C"
{
    u32 __nx_applet_type = AppletType::AppletType_None;

    // setup a fake heap
    #define HEAP_SIZE 0x6'000
    char fake_heap[HEAP_SIZE];

    // we override libnx internals to do a minimal init
    void __libnx_initheap(void)
    {
        extern char *fake_heap_start;
        extern char *fake_heap_end;

        // setup newlib fake heap
        fake_heap_start = fake_heap;
        fake_heap_end = fake_heap + HEAP_SIZE;
    }

    void __appInit(void)
    {
        R_ASSERT(smInitialize());
        R_ASSERT(setsysInitialize());
        SetSysFirmwareVersion fw;
        R_ASSERT(setsysGetFirmwareVersion(&fw));
        hosversionSet(MAKEHOSVERSION(fw.major, fw.minor, fw.micro));
        setsysExit();
        R_ASSERT(setInitialize());
        R_ASSERT(pmdmntInitialize());
        R_ASSERT(nsInitialize());
        R_ASSERT(pminfoInitialize());

        constexpr SocketInitConfig sockConf = {
            .bsdsockets_version = 1,

            .tcp_tx_buf_size = 0x800,
            .tcp_rx_buf_size = 0x800,
            .tcp_tx_buf_max_size = 0x2EE0,
            .tcp_rx_buf_max_size = 0,

            .udp_tx_buf_size = 0x0,
            .udp_rx_buf_size = 0x0,

            .sb_efficiency = 1,
        };
        R_ASSERT(socketInitialize(&sockConf));
        smExit();
    }

    void __appExit(void)
    {
        socketExit();
        pminfoExit();
        nsExit();
        pmdmntExit();
        setExit();
    }
}

static u64 lastProcessId = 0;
static u64 lastProgramId = 0;
static const char * lastGameName = "A game";

int main(int argc, char **argv)
{
    R_ASSERT(setupSocketServer());

    while (true)
    {
        //Socket Result
        int src;
        u64 processId;
        u64 programId;

        if (R_SUCCEEDED(pmdmntGetApplicationProcessId(&processId)))
        {
            if (lastProcessId != processId)
            {
                lastProcessId = processId;
                if (R_SUCCEEDED(pminfoGetProgramId(&programId, processId)))
                {
                    if (lastProgramId != programId)
                    {
                        lastProgramId = programId;
                        lastGameName = Utils::getAppName(programId);
                    }
                }
            }

            src = sendData(programId, lastGameName);
        }
        else
        {
            //This is so we can make sure our connection is not broken if so, start and accept a new one
            src = sendData(0, "NULL");
        }

        if (src < 0)
        {
            closeSocketServer();
            R_ASSERT(setupSocketServer());
        }

        svcSleepThread(1e+9L);
    }
}
