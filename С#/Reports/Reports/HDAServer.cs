using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class HDAServer : Server//Класс сервера исторических данных
    {
        Opc.Hda.Server server;//Создаем объект сервера исторических данных

        List<string> list;
        Opc.Hda.ItemValueCollection col;//Создаем коллекцию исторических данных

        public HDAServer(string MasterIP, string SlaveIP, string OPCServer, string path)
            : base(MasterIP, SlaveIP, OPCServer, path)
        {

        }

        public override void CreateServer(Opc.URL url)//Функция создания сервера исторических данных
        {
            server = new Opc.Hda.Server(new OpcCom.Factory(), url);
        }

        public override void _Connect()//Вызваем функцию подключения
        {
            server.Connect();
        }

        public override void _Disconnect()//Вызваем функцию отключения
        {
            server.Disconnect();
        }

        public object[] GetData(Opc.Hda.Time startTime, Opc.Hda.Time endTime, List<string> list)//Функция получения данных из исторического сервера
            //по мимо списка переменных принимает начальное и конечное время выборки
        {
            if (server.IsConnected == false)
            {
                Connect();
            }

            this.list = list;

            Opc.ItemIdentifier[] HDA_tags = ConvertTags();
            Opc.IdentifiedResult[] items = server.CreateItems(HDA_tags);
            Opc.Hda.ItemValueCollection[] RawData = server.ReadRaw(startTime, endTime, 100, true, items);

            return Filter(RawData);
        }

        Opc.ItemIdentifier[] ConvertTags()//Конвертируем список в массив OPC-переменных
        {
            int count = list.Count;
            Opc.ItemIdentifier[] tags = new Opc.ItemIdentifier[count];

            for (int i = 0; i < count; i++)
            {
                tags[i] = new Opc.ItemIdentifier(list[i].ToString());
            }

            return tags;
        }

        object[] Filter(Opc.Hda.ItemValueCollection[] Collection)//Функция фильтрации данных, полученных с исторического сервера
        {

            int lengthOfCollection = Collection.Length;
            object[] filteredData = new object[lengthOfCollection];

            for (int colIndex = 0; colIndex < lengthOfCollection; colIndex++)
            {
                col = Collection[colIndex];
                filteredData[colIndex] = FindGoodValue();
            }

            return filteredData;
        }

        object FindGoodValue()//Функция поиска "хороших" значения
        {
            int length = col.Count;

            for (int itemIndex = 0; itemIndex < length; itemIndex++)
            {
                if (CheckValue(itemIndex))
                {
                    return col[itemIndex].Value;
                }
            }

            return null;
        }

        bool CheckValue(int itemIndex)//Функция проверки значений. "NoBound" означает выход за границы диапазона
        {
            return (col[itemIndex].HistorianQuality.ToString() != "NoBound");
        }
    }

}
