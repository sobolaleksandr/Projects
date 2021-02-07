#include "StringReciever.h"

StringReciever::StringReciever(const std::string& fileName, IBufferHandler* handler, std::mutex* fileLock)
{
	_fileName = fileName;
	_handler = handler;
	_fileLock = fileLock;
}

// Данный метод представляет поток 1 согласно заданию
void StringReciever::start()
{
	std::string userData;
	while (true)
	{
		getline(std::cin, userData);

		if (!isStringCorrect(userData)) // Проверка корректности строки (в строке не более 64 символов и она содержит только цифры)
		{
			std::cout << "Program 1: String is incorrect" << std::endl;
			continue;
		}
		
		_fileLock->lock();

		std::ofstream fout(_fileName);
		if (!fout.is_open())  
		{
			std::cout << "Program 1: Error: Reciever can't open file" << std::endl;

			_fileLock->unlock();
			continue;
		}
		else 
		{
			fout << handleString(userData) << std::endl; // Обработка строки (сортировка по убыванию и замена чётных элементов на "KB")
			fout.close();
		}

		_fileLock->unlock();

		_handler->handleBuffer();
	}
}

// Проверка корректности строки
bool StringReciever::isStringCorrect(const std::string& userData)
{
	int length = userData.length();

	// В строке не более 64 символов
	if (length > SYMBOLS_AMOUNT)
	{
		return false;
	}

	// Строка содержит только цифры
	for (int i = 0; i < length; i++) {
		if (userData[i] < '0' || userData[i]>'9')
		{
			return false;
		}
	}

	return true;
}

// Обработка строки
std::string  StringReciever::handleString(const std::string& userData)  
{
	int length = userData.length();
	
	// Сортировка по убыванию
	std::string sortedUserData = userData;

	for (int i = 1; i < length; ++i)
	{
		for (int r = 0; r < length - i; r++)
		{
			if (sortedUserData[r] < sortedUserData[r + 1])
			{
				char temp = sortedUserData[r];
				sortedUserData[r] = sortedUserData[r + 1];
				sortedUserData[r + 1] = temp;
			}
		}
	}

	// Замена чётных элементов на "КВ" (под "чётный элемент" было понято чётное значение элемента, а не чётная позиция элемента)
	std::string augmentedUserData = "";

	for (int i = 0; i < length; i++)
	{
		if (sortedUserData[i] % 2 == 0) {  // Так как цифры 0 - 9 соответствуют по чётности своим кодам в ASCII таблице (48 - 57), преобразование опускаем
			augmentedUserData += "KB";
		}
		else {
			augmentedUserData += sortedUserData[i];
		}
	}

	return augmentedUserData;
}
