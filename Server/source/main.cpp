#include "Sockets.h"
#include <netinet/in.h>
#include <sys/socket.h>
#include <unistd.h>
#include "Utils.h"
#include <string>
#include "Nacp.h"

#define HEAP_SIZE 120000

extern "C"
{
    extern u32 __start__;
    void __libnx_initheap(void);
    void __appInit(void);
    void __appExit(void);
    u32 __nx_applet_type = AppletType::AppletType_None;

    // setup a fake heap
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
        R_ASSERT(getSystemLanguage());
        R_ASSERT(setsysInitialize());
        SetSysFirmwareVersion fw;
        R_ASSERT(setsysGetFirmwareVersion(&fw));
        hosversionSet(MAKEHOSVERSION(fw.major, fw.minor, fw.micro));
        setsysExit();

        R_ASSERT(pmdmntInitialize());
        R_ASSERT(nsInitialize());
        R_ASSERT(pminfoInitialize());

        constexpr SocketInitConfig sockConf = {
            .bsdsockets_version = 1,

            .tcp_tx_buf_size = 0x800,
            .tcp_rx_buf_size = 0x1000,
            .tcp_tx_buf_max_size = 0x2EE0,
            .tcp_rx_buf_max_size = 0x2EE0,

            .udp_tx_buf_size = 0x0,
            .udp_rx_buf_size = 0x0,

            .sb_efficiency = 4,
        };
        R_ASSERT(socketInitialize(&sockConf));
        smExit();
    }

    void __appExit(void)
    {
        pminfoExit();
        pmdmntExit();
        nsExit();
        socketExit();
    }
}

int main(int argc, char **argv)
{
    int sock = setupSocketServer();
    struct sockaddr_in client_addr;
    socklen_t client_len = sizeof(client_addr);
    int connection = accept(sock, (struct sockaddr *)&client_addr, &client_len);

    u64 lastPid = 0;
    u64 lastProgramId = 0;
    const char *lastGame;

    while (true)
    {
        Result rc;
        //Socket Result
        int src;
        u64 pid;
        u64 program_id;
        rc = pmdmntGetApplicationProcessId(&pid);

        if (R_SUCCEEDED(rc))
        {
            if (lastPid != pid)
            {
                pminfoGetProgramId(&program_id, pid);
                lastPid = pid;

                if (program_id != lastProgramId)
                {
                    lastProgramId = program_id;
                    lastGame = Utils::getAppName(program_id);
                }
            }

            src = sendData(connection, program_id, lastGame);
        }
        else
        {
            //This is so we can make sure our connection is not broken if so, start and accept a new one
            src = sendData(connection, 0, "NULL");
        }
        if (src < 0)
        {
            close(connection);
            close(sock);
            sock = setupSocketServer();
            connection = accept(sock, (struct sockaddr *)&client_addr, &client_len);
        }

        svcSleepThread(5e+9);
    }
}
