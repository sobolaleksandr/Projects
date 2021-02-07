#pragma once
#include <iostream>
#include <sys/types.h>
#include <unistd.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <cstring>

// Класс приёмник данных через сокет (сервер)
class SocketServer
{
public:
	SocketServer(int port);

	bool waitConnection();

	int recvData_8(char* buf);

	~SocketServer();

private:
		
	int _servSock;
	int _clientSock;
	sockaddr_in _sin;

	bool _bIsSuccessfullyCreated;
	bool _bIsClientConnected;
};

