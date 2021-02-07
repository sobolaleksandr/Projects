#include "SocketClient.h"

SocketClient::SocketClient(int port)
{
	_bIsSuccessfullyCreated = true;
	_bIsConnected = false;
		
	_clientSock = socket(AF_INET, SOCK_STREAM, 0);

	if (_clientSock == -1)
	{
		std::cout << "Program 1: Unable to create socket" << std::endl;
		_bIsSuccessfullyCreated = false;
	}

	_serverInfo.sin_family = AF_INET;
    _serverInfo.sin_port = htons(port);
	inet_pton(AF_INET, "0.0.0.0", &_serverInfo.sin_addr);
}

// Установка соединения с программой 2 (сервером)
bool SocketClient::connectToServer()
{
	if (_bIsSuccessfullyCreated)
	{
		std::cout << "Program 1: Conecting to server ..." << std::endl;

		int retVal = connect(_clientSock, (sockaddr*)&_serverInfo, sizeof(_serverInfo));
		if (retVal == -1)
		{
			std::cout << "Program 1: Unable to connect" << std::endl;
			return false;
		}

		_bIsConnected = true;
		std::cout << "Program 1: The connection has been established" << std::endl;
    
		return true;
	}

	std::cout << "Program 1: The client was not created correctly" << std::endl;
	return false;
}

bool SocketClient::sendData(char* data) // Интерфейсный вызов
{
	if (_bIsConnected)
	{
		int retVal = send(_clientSock, data, strlen(data), 0);

		if (retVal == -1)
		{
			std::cout << "Program 1: Unable to send" << std::endl;
			return false;
		}

		return true;
	}
	std::cout << "Program 1: There is no connection to server" << std::endl;
	return false;
}

SocketClient::~SocketClient()
{
	close(_clientSock);
}
