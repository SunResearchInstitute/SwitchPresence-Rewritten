#include "Sockets.h"
#include <netinet/in.h>
#include <sys/socket.h>
#include "Utils.h"

using namespace std;

#define HEAP_SIZE 350000

extern "C"
{
    extern u32 __start__;

    u32 __nx_applet_type = AppletType_None;

#define INNER_HEAP_SIZE 0x60000
    size_t nx_inner_heap_size = INNER_HEAP_SIZE;
    char nx_inner_heap[INNER_HEAP_SIZE];

    void __libnx_init_time(void);
    void __libnx_initheap(void);
    void __appInit(void);
    void __appExit(void);

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
        Result rc;
        rc = smInitialize();
        if (R_FAILED(rc))
            fatalSimple(rc);
        rc = setsysInitialize();
        if (R_SUCCEEDED(rc))
        {
            SetSysFirmwareVersion fw;
            rc = setsysGetFirmwareVersion(&fw);
            if (R_SUCCEEDED(rc))
                hosversionSet(MAKEHOSVERSION(fw.major, fw.minor, fw.micro));
            else
                fatalSimple(rc);
            setsysExit();
        }
        else
            fatalSimple(rc);
        rc = pmdmntInitialize();
        if (R_FAILED(rc))
            fatalSimple(rc);
        rc = nsInitialize();
        if (R_FAILED(rc))
            fatalSimple(rc);
        
        SocketInitConfig sockConf = {
            .bsdsockets_version = 1,

            .tcp_tx_buf_size = 0x800,
            .tcp_rx_buf_size = 0x1000,
            .tcp_tx_buf_max_size = 0x2EE0,
            .tcp_rx_buf_max_size = 0x2EE0,

            .udp_tx_buf_size = 0x0,
            .udp_rx_buf_size = 0x0,

            .sb_efficiency = 4,

            .serialized_out_addrinfos_max_size = 0x1000,
            .serialized_out_hostent_max_size = 0x200,
            .bypass_nsd = false,
            .dns_timeout = 0,
        };
        rc = socketInitialize(&sockConf);
        if (R_FAILED(rc))
            fatalSimple(rc);
        rc = pminfoInitialize();
        if (R_FAILED(rc))
            fatalSimple(rc);
    }

    void __appExit(void)
    {
        pminfoExit();
        pmdmntExit();
        nsExit();
        socketExit();
        smExit();
    }
}

int main(int argc, char **argv)
{
    int sock = setupSocketServer();
    struct sockaddr_in client_addr;
    socklen_t client_len = sizeof(client_addr);
    int connection = accept(sock, (struct sockaddr *)&client_addr, &client_len);


    int src;
    while (true)
    {
        Result rc;
        u64 pid;
        u64 tid;
        rc = pmdmntGetApplicationPid(&pid);
        if (R_SUCCEEDED(rc))
        {
            pminfoGetTitleId(&tid, pid);
            src = sendData(connection, tid, Utils::getAppName(tid));
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
