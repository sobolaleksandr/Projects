#pragma once
#include <iostream>
#include <sys/types.h>
#include <unistd.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <cstring>

#include "IDataSocket.h"

// Класс передатчик данных через сокет (клиент)
class SocketClient : public IDataSocket
{
public:

	SocketClient(int port);

	bool connectToServer();

	virtual bool sendData(char* data) override;

	~SocketClient();

private:
		
	int _clientSock;
	sockaddr_in _serverInfo;

	bool _bIsSuccessfullyCreated;
	bool _bIsConnected;
};

