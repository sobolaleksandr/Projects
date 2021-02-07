#pragma once

// Интерфейс для классов обработчиков буфера. В данной программе для отвязывания StringReciever-а от конкретного обработчика
class IBufferHandler
{
public:
	virtual void handleBuffer() = 0;

	virtual ~IBufferHandler(){};
};

