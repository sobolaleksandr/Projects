#include "SocketServer.h"
#include "DataParser.h"

const int PORT_NUMBER = 1111;

int main()
{
	// Создаём приёмник данных (сервер)
	SocketServer server(PORT_NUMBER);

	// Создаём обработчик данных
	DataParser parser;
	
	bool bIsClientConnected;

	while (true)
	{
		// Ждём присоединения программы 1 (клиента)
		if (server.waitConnection())
		{
			bIsClientConnected = true;
			while (bIsClientConnected)
			{
				char buf[8] = "";

				// Получаем данные и проверяем не оборвалось ли соединение
				if (server.recvData_8(buf) == -1)
					bIsClientConnected = false; // При обрыве связи, возвращаемся к ожиданию нового соединения
				else
					parser.parseData(buf); // Обрабатываем данные согласно заданию (более 2-х цифр и кратность 32-м)
			}
		}
	}
	return 0;
}