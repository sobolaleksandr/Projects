#include "StringReciever.h"
#include "FileHandler.h"
#include <thread>
#include "SocketClient.h"


const std::string FILENAME = "FileBuffer.txt";
const int PORT_NUMBER = 1111;


int main()
{
	// Создаём передатчик данных (клиент)
	SocketClient client(PORT_NUMBER);
	
	// Подключаемся к программе 2 (к серверу)
	if (client.connectToServer())
	{
		std::mutex fileLock; // Мьютекс для обработки доступа потоков к общему буферу

		// Создаём обработчик буфера
		FileHandler handler(FILENAME, &fileLock, &client);

		// Создаём обработчик ввода пользователя
		StringReciever reciever(FILENAME, &handler, &fileLock);

		// Поток 2 согласно заданию
		std::thread handlerThread(&FileHandler::start, &handler);
		handlerThread.detach();

		// Поток 1 согласно заданию
		std::thread recieverThread(&StringReciever::start, &reciever);
		recieverThread.join();
	}
	
	return 0;
}







