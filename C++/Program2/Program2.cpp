#include "SocketServer.h"
#include "DataParser.h"

const int PORT_NUMBER = 1111;

int main()
{
	// ������ ������� ������ (������)
	SocketServer server(PORT_NUMBER);

	// ������ ���������� ������
	DataParser parser;
	
	bool bIsClientConnected;

	while (true)
	{
		// ��� ������������� ��������� 1 (�������)
		if (server.waitConnection())
		{
			bIsClientConnected = true;
			while (bIsClientConnected)
			{
				char buf[8] = "";

				// �������� ������ � ��������� �� ���������� �� ����������
				if (server.recvData_8(buf) == -1)
					bIsClientConnected = false; // ��� ������ �����, ������������ � �������� ������ ����������
				else
					parser.parseData(buf); // ������������ ������ �������� ������� (����� 2-� ���� � ��������� 32-�)
			}
		}
	}
	return 0;
}