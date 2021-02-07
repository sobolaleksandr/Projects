#pragma once
#include "IBufferHandler.h"
#include <string>
#include <iostream>
#include <fstream>
#include <mutex>
#include <thread>

#include "IDataSocket.h"

// ����� ���������� ������ �� ������ ������-�����
class FileHandler: public IBufferHandler
{
public:
	FileHandler(const std::string& fileName, std::mutex* fileLock, IDataSocket* socket);

	virtual void handleBuffer() override;

	void start();

private:

	std::string _fileName;
	std::mutex* _fileLock;
	IDataSocket* _socket;

	bool _bFileEmpty;
	const int NUMBERS_ASCII_SHIFT = 48; // ����� ����� ��������-���� ������������ ���������� ���� � ASCII �������

	void clearFile();

	void getSum(const std::string& data, char* buf);
};

