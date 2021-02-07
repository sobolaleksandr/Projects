using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class DAServer : Server//Класс сервера оперативных данных
    {
        Opc.Da.Server server;//Создаем объект сервера оперативных данных

        Opc.Da.ItemValueResult[] Data;//Создаем массив оперативных данных

        public DAServer(string MasterIP, string SlaveIP, string OPCServer, string path)
            : base(MasterIP, SlaveIP, OPCServer, path)
        {

        }

        public override void CreateServer(Opc.URL url)//Функция создания оперативного сервера по URL
        {
            server = new Opc.Da.Server(new OpcCom.Factory(), url);
        }

        public override void _Connect()//Вызов функции подключения
        {
            server.Connect();
        }

        public override void _Disconnect()//Вызов функции отключения
        {
            server.Disconnect();
        }

        public object[] GetData(List<string> list)//Функция вычитывания списка переменных с сервера
        {
            if (server.IsConnected == false)
            {
                Connect();
            }

            Opc.Da.Item[] tags = ConvertTags(list);
            Data = server.Read(tags);

            return ConvertedDataToObjectArray();
        }

        object[] ConvertedDataToObjectArray()//Преобразуем массив оперативных данных в массив object[]
        {
            int length = Data.Length;
            object[] result = new object[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = Data[i].Value;
            }
            return result;
        }

        Opc.Da.Item[] ConvertTags(List<string> list)//Преобразуем переменные в объект Item оперативного сервера
        {
            int count = list.Count;
            Opc.Da.Item[] tags = new Opc.Da.Item[count];

            for (int i = 0; i < count; i++)
            {
                tags[i] = new Opc.Da.Item(
                    new Opc.ItemIdentifier(list[i]));
            }

            return tags;
        }

    }

}
