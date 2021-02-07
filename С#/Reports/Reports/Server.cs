using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public abstract class Server// Абстрактный класс сервера. В нем определены общие для Исторического и Оперативного сервера функции и поля
    {
        readonly Opc.URL MasterUrl;
        readonly Opc.URL SlaveUrl;

        public Server(string MasterIP, string SlaveIP, string OPCServer, string path)//Получаем ip-адресы серверов из XML-файла настроек и формируем URL
        {
            MasterUrl = new Opc.URL(path + MasterIP + "/" + OPCServer);
            SlaveUrl = new Opc.URL(path + SlaveIP + "/" + OPCServer);
        }

        public void Connect()//Подключаемся к основному серверу
        {
            CreateServer(MasterUrl);

            try
            {
                _Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ConnectSlave();
            }

            System.Threading.Thread.Sleep(1500);//время для подключения
        }

        public abstract void CreateServer(Opc.URL url);//Так как мы будем создавать либо сервер исторических, либо оперативных данных, то определяем этот метод у наследника

        void ConnectSlave()//Подключаемся к резервному серверу
        {
            CreateServer(SlaveUrl);

            try
            {
                _Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Disconnect()//Отключаемся от сервера
        {
            try
            {
                _Disconnect();
            }
            catch
            {
            }
        }

        public abstract void _Connect();//Методы в которых вызваются функции подключения к конкретному серверу определены у наследников
        public abstract void _Disconnect();
    }
}
