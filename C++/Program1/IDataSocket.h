#pragma once

// Интерфейс для классов передатчиков данных. В данной программе для отвязывания FileHandler-а от конкретного способа передачи данных программе 2
class IDataSocket
{
public:
	virtual bool sendData(char* data) = 0;

	virtual ~IDataSocket() {};
};

