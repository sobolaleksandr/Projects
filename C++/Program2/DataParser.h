#pragma once
#include<iostream>
#include <cstring>

// ����� ���������� �������� ������
class DataParser
{
public:
	
	DataParser();

	void parseData(char* data);

private:

	const int NUMBERS_ASCII_SHIFT = 48; // ����� ����� ��������-���� ������������ ���������� ���� � ASCII �������
};

