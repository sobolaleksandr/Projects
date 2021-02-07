#include "SocketServer.h"

SocketServer::SocketServer(int port)
{
	_bIsSuccessfullyCreated = true;
	_bIsClientConnected = false;

		_servSock = socket(AF_INET, SOCK_STREAM, 0);

	if (_servSock == -1)
	{
		std::cout << "Program 2: Unable to create socket" << std::endl;
		_bIsSuccessfullyCreated = false;
	}

	_sin.sin_family = AF_INET;
	_sin.sin_port = htons(port); 
	inet_pton(AF_INET, "0.0.0.0", &_sin.sin_addr);

	int retVal = bind(_servSock, (sockaddr*)&_sin, sizeof(_sin));
	if (retVal == -1)
	{
		std::cout << "Program 2: Unable to bind" << std::endl;
		_bIsSuccessfullyCreated = false;
	}
}

// Установка соединения с программой 1 (клиентом)
bool SocketServer::waitConnection()
{
	if (_bIsSuccessfullyCreated) 
	{
		int retVal = listen(_servSock, 1);
		if (retVal == -1)
		{
			std::cout << "Program 2: Unable to listen" << std::endl;
			return false;
		}
		
		// Ждём программу 1 (клиента)
		std::cout << "Program 2: Waiting for connection ..." << std::endl;

		_clientSock = accept(_servSock, NULL, NULL);
		if (_clientSock == -1)
		{
			std::cout << "Program 2: Unable to accept" << std::endl;
			return false;
		}

		_bIsClientConnected = true;
		std::cout << "Program 2: The connection has been established" << std::endl;
		return true;
	}

	std::cout << "Program 2: The server was not created correctly" << std::endl;
	return false;
}

// Принятие данных из сокета от программы 1 (клиента)
int SocketServer::recvData_8(char* buf)
{
	if (_bIsClientConnected)
	{
		int retVal = recv(_clientSock, buf, 8, 0);

		if (retVal == -1 || retVal == 0)
		{
			std::cout << "Program 2: The connection is lost" << std::endl;
			return -1;
		}
		
		return 0;
	}
	std::cout << "Program 2: The client is not connected" << std::endl;
	return -1;
}

SocketServer::~SocketServer()
{
	close(_clientSock);
	close(_servSock);
}


