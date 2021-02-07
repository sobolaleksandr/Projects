using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Initiator//Класс инициализации. Выполняет ряд операций, которые необходимо выполнить в каждом отчете
    {
        public DAServer opcda;
        public HDAServer opchda;
        public List<Section> Sections;
        public bool isTesting = true;

        public Settings settings;

        public Initiator()
        {
            const string xPath = @"C:\settings.xml";
            settings = new Settings(xPath);//Вычитываем из файла основные параметры (Ip-адрес Серверов, путь до ini-файла с тегами)

            ConnectDAServer();//Подключаемся к Серверу оперативных данных
        }

        void ConnectDAServer()
        {
            opcda = new DAServer(settings.DAMasterIP, settings.DASlaveIP, settings.DAServer, settings.DApath);//Создаем объект сервера оперативных данных
            opcda.Connect();//Подключаемся
        }

        public Initiator(string[] Splitter, bool FilteredPaths)
            : this()
        {
            INIData inidata = new INIData(settings.INIPath, Splitter, FilteredPaths);//Создаем объект для обработки Ini-файла
            Sections = inidata.Parse();//Парсим ini-файл в массив секций

            ConnectHDAServer();//Подключаемся к Серверу исторических данных
        }

        void ConnectHDAServer()
        {
            opchda = new HDAServer(settings.HDAMasterIP, settings.HDASlaveIP, settings.HDAServer, settings.HDApath);//Создаем объект сервера исторических данных
            opchda.Connect();//Подключаемся
        }

        public void GetData()//В некоторых отчетах необходимо получить срез параметров на начало смены (при условии формирования отчета в конец смены)
        {
            DateTime startDate = DateTime.Now.AddHours(-13);
            DateTime endDate = DateTime.Now.AddHours(-12);

            Opc.Hda.Time startTime = new Opc.Hda.Time(startDate);
            Opc.Hda.Time endTime = new Opc.Hda.Time(endDate);

            foreach (Section section in Sections)
            {
                foreach (Tag item in section.Items)
                {
                    object[] DAvalue = opcda.GetData(item.Path);
                    object[] HDAvalue = opchda.GetData(startTime, endTime, item.Path);
                    item.ValDA.SetValue(DAvalue);
                    item.ValHDA.SetValue(HDAvalue);
                }
            }
        }
    }
