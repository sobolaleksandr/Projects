#pragma once

// ��������� ��� ������� ������������ ������. � ������ ��������� ��� ����������� FileHandler-� �� ����������� ������� �������� ������ ��������� 2
class IDataSocket
{
public:
	virtual bool sendData(char* data) = 0;

	virtual ~IDataSocket() {};
};

