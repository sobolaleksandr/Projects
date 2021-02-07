#pragma once
#include<iostream>
#include <cstring>

// Класс обработчик приянтых данных
class DataParser
{
public:
	
	DataParser();

	void parseData(char* data);

private:

	const int NUMBERS_ASCII_SHIFT = 48; // Сдвиг кодов символов-цифр относительно десятичных цифр в ASCII таблице
};

