#include "sockets.h"

int sendData(int sock, u64 tid, std::string name)
{
    struct titlepacket packet;
    packet.magic = TITLE_MAGIC;
    strcpy(packet.name, name.c_str());
    packet.tid = tid;
    int rc = send(sock, &packet, sizeof(packet), 0);

    return rc;
}

int setupSocketServer()
{
    int sockfd = socket(AF_INET, SOCK_STREAM, 0);

    if (sockfd < 0)
        fatalThrow(MAKERESULT(Module_Discord, Error_SocketInitFailed));

    int opt = 1;
    if (setsockopt(sockfd, SOL_SOCKET, SO_REUSEADDR, &opt, sizeof(opt)) < 0)
        fatalThrow(MAKERESULT(Module_Discord, Error_OptFailed));

    struct sockaddr_in servaddr;
    memset(&servaddr, 0, sizeof(servaddr));

    servaddr.sin_family = AF_INET;
    servaddr.sin_addr.s_addr = INADDR_ANY;
    servaddr.sin_port = htons(PORT);
    socklen_t serv_len = sizeof(servaddr);

    while (bind(sockfd, (struct sockaddr *)&servaddr, serv_len) < 0)
        svcSleepThread(1e+9L);

    listen(sockfd, 69);
    return sockfd;
}
