using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Settings
    {
        //Параметры для подключения по-умолчанию. Легко корректируются в текстовом редакторе оператором/наладчиком
        public string DAMasterIP = "localhost";
        public string DASlaveIP = "localhost";
        public string HDAMasterIP = "localhost";
        public string HDASlaveIP = "localhost";
        public string HDAServer = "EleSy.HDADualSource";
        public string DAServer = "Elesy.DualSource";
        public List<string> Sections = new List<string>{"Reports.Section1","Reports.Section2","Reports.Section3",
            "Reports.Section4","Reports.Section5","Reports.Section6","Reports.Section7",
            "Reports.Section8","Reports.Section9","Reports.Section10","Reports.EndDate","Reports.StartDate"};
        public string[] Labels = { "22/7", "22/40", "54/7", "23/40", "36/40", "41/40",
            "Подвал/40", "38/1,2", "Подвал/2,3", "Инженерный корпус" };
        public string INIPath = @"C:\Data\Reports\1042_02.ini";
        public string DApath = "opcda://";
        public string HDApath = "opchda://";

        readonly string path;

        string value;
        XElement elem;
        XDocument xdoc;

        public Settings(string path)//Считываем из файла или создаем его в конструкторе
        {
            this.path = path;

            if (System.IO.File.Exists(path))
            {
                GetData();
            }
            else
            {
                Create();
            }
        }

        void GetData()//Считываем по отдельности секцию настроек сервера и расположения файлов
        {
            xdoc = XDocument.Load(path);

            foreach (XElement objectElement in xdoc.Element("settings").Elements("object"))
            {
                elem = objectElement;
                value = objectElement.FirstAttribute.Value;

                GetServer();
                GetPath();
            }
        }

        void GetServer()//Функция считывания настроек сервера
        {
            switch (value)
            {
                case "OPCDA":
                    DAMasterIP = elem.Element("MasterIP").Value;
                    DASlaveIP = elem.Element("SlaveIP").Value;
                    DAServer = elem.Element("OPCServer").Value;
                    break;
                case "OPCHDA":
                    HDAMasterIP = elem.Element("MasterIP").Value;
                    HDASlaveIP = elem.Element("SlaveIP").Value;
                    HDAServer = elem.Element("OPCServer").Value;
                    break;
                default: break;
            }
        }

        void GetPath()//Функция считывания расположения ini-файлов и прочих настроек 
        {
            switch (elem.FirstAttribute.Name.LocalName)
            {
                case "path":
                    INIPath = value;
                    break;
                case "section":
                    Sections = new List<string>(value.Replace(" ", "").Split(','));
                    break;
                case "label":
                    Labels = value.Split(';');
                    break;
                case "DApath":
                    DApath = value;
                    break;
                case "HDApath":
                    HDApath = value;
                    break;
                default: break;
            }
        }

        void Create()//Функция создания XML-файла с настройками по-умолчанию
        {
            XDocument xdoc = new XDocument(new XElement("settings",
                new XElement("object",
                    new XAttribute("server", "OPCDA"),
                    new XElement("MasterIP", DAMasterIP),
                    new XElement("SlaveIP", DASlaveIP),
                    new XElement("OPCServer", DAServer)),
                new XElement("object",
                    new XAttribute("server", "OPCHDA"),
                    new XElement("MasterIP", HDAMasterIP),
                    new XElement("SlaveIP", HDASlaveIP),
                    new XElement("OPCServer", HDAServer)),
                new XElement("object",
                    new XAttribute("path", INIPath)),
                new XElement("object",
                    new XAttribute("DApath", DApath)),
                new XElement("object",
                    new XAttribute("HDApath", HDApath))));
            xdoc.Save(path);
        }
    }

}
