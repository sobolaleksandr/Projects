#include "DataParser.h"

DataParser::DataParser()
{
}

void DataParser::parseData(char* data)
{
	int length = strlen(data);

	// Проверка правильности числа (более 2-х цифр)
	if (length > 2)
	{
		// Восстановление полученного числа
		int number = 0;
		int power = 1;

		for (int i = length; i > -1; i--)
		{
			if (data[i] >= '0' && data[i] <= '9') {
				int digit = data[i] - NUMBERS_ASCII_SHIFT;
				number += digit * power;
				power = power * 10;
			}
		}

		// Проверка правильности числа (кратность 32-м)
		if (number % 32 == 0)
			std::cout << "Program 2: Recieved correct number: " << number << std::endl;
		
		else
			std::cout << "Program 2: Error: Recieved incorrect number" << std::endl;
	}
	else
		std::cout << "Program 2: Error: Recieved incorrect number" << std::endl;
	
}
