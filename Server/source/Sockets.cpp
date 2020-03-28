#include "Sockets.h"
#include "Results.h"
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <cstring>

static int sockfd = 0;
static int connection = 0;

int sendData(u64 programId, const char *name)
{
    titlepacket packet;
    packet.magic = PACKETMAGIC;
    strcpy(packet.name, name);
    packet.programId = programId;
    return send(connection, &packet, sizeof(packet), 0);
}

Result setupSocketServer()
{
    sockfd = socket(AF_INET, SOCK_STREAM, 0);

    if (sockfd == -1)
        return MAKERESULT(Module_Discord, Error_SocketInitFailed);

    int opt = 1;
    if (setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt)) == -1)
    {
        close(sockfd);
        return MAKERESULT(Module_Discord, Error_OptFailed);
    }

    const sockaddr_in servaddr {
        .sin_family = AF_INET,
        .sin_port = htons(PORT),
        .sin_addr = {INADDR_ANY},
    };

    while (bind(sockfd, reinterpret_cast<const sockaddr *>(&servaddr), sizeof(servaddr)) == -1)
        svcSleepThread(1e+9L);

    listen(sockfd, 20);

    connection = accept(sockfd, nullptr, nullptr);
    return 0;
}

void closeSocketServer()
{
    close(connection);
    close(sockfd);
}
