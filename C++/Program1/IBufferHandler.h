#pragma once

// ��������� ��� ������� ������������ ������. � ������ ��������� ��� ����������� StringReciever-� �� ����������� �����������
class IBufferHandler
{
public:
	virtual void handleBuffer() = 0;

	virtual ~IBufferHandler(){};
};

