#pragma once
#include <string>
#include "IBufferHandler.h"
#include <iostream>
#include <fstream>
#include <mutex>

// Класс обработчик ввода пользователя
class StringReciever
{
public:
	StringReciever(const std::string& fileName, IBufferHandler* handler, std::mutex* fileLock);

	void start();

private:
	std::string _fileName;
	IBufferHandler* _handler;
	std::mutex* _fileLock;

	const int SYMBOLS_AMOUNT = 64;

	bool isStringCorrect(const std::string& userData);

	std::string handleString(const std::string& userData);
};

