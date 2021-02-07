#include "FileHandler.h"


FileHandler::FileHandler(const std::string& fileName, std::mutex* fileLock, IDataSocket* socket)
{
	_fileName = fileName;
	_fileLock = fileLock;
	_socket = socket;
	_bFileEmpty = true;
}

void FileHandler::handleBuffer()  // Интерфейсный вызов
{
	_bFileEmpty = false;
}

// Данный метод представляет поток 2 согласно заданию
void FileHandler::start()
{
	while (true)
	{
		if (!_bFileEmpty)
		{
			_fileLock->lock();

			std::ifstream fin(_fileName);

			if (!fin.is_open()) 
				std::cout << "Program 1: Error: Handler can't open file" << std::endl;
			else {
				std::string outData;

				getline(fin, outData);

				std::cout << "Program 1: "<< outData << std::endl;

				char buf[8] = "";
				getSum(outData, buf); // Расчёт суммы элементов-чисел
				_socket->sendData(buf); // Отправка данных программе 2

				fin.close();
				clearFile();
			}

			_bFileEmpty = true;

			_fileLock->unlock();
		}
	}
}

void FileHandler::clearFile()
{
	std::fstream fclear(_fileName, std::ofstream::out | std::ofstream::trunc);
	fclear.close();
}

// Расчёт суммы элементов-чисел
void FileHandler::getSum(const std::string& data, char* buf)
{
	int length = data.length();
	int sum = 0;

	for (int i = 0; i < length; i++) {
		if (data[i] >= '0' && data[i] <= '9')
		{
			sum += data[i] - NUMBERS_ASCII_SHIFT;
		}
	}
	
	sprintf(buf, "%d", sum);
}


