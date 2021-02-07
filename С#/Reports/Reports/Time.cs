using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Time//Класс для работы с данными типа время
    {
        DateTime endDate;
        DateTime startDate;

        object[] DaData;

        public Opc.Hda.Time startTime = new Opc.Hda.Time();
        public Opc.Hda.Time startTimedelta = new Opc.Hda.Time();
        public Opc.Hda.Time endTime = new Opc.Hda.Time();
        public Opc.Hda.Time endTimedelta = new Opc.Hda.Time();

        public Time(object[] DaData)//В конструкторе полуаем время с сервера. Если не получилось, задаем по-умолчанию. Затем ковертируем
        {
            this.DaData = DaData;

            if (ParseResult())
            {
                SetProper();
            }
            else
            {
                SetDefault();
            }

            Convert();
        }

        void SetProper()//Оператор может по-ошибке перепутать начальное и конечное время формирования отчета
        {
            if (endDate > DateTime.Now)
            {
                endDate = DateTime.Now;
            }

            if (endDate == startDate)
            {
                startDate = endDate.AddHours(-24);
            }
        }

        void SetDefault()//Если не получилось получить время с сервера, то устанавливаем по-умолчанию
        {
            endDate = DateTime.Now;
            startDate = DateTime.Now.AddHours(-24);
        }

        bool ParseResult()//В данном случае последние два тега массива отвечали за время. Данная задача решалась только в этом проекте, поэтому такое "неуниверсальное" решение
        {
            int LastItem = DaData.Length - 1;
            int prevLastItem = LastItem - 1;

            //На сервере время хранится в переменной типа String
            bool ParseResult = DateTime.TryParse(DaData[LastItem].ToString(), out endDate) ||
                DateTime.TryParse(DaData[prevLastItem].ToString(), out startDate);
            return ParseResult;
        }

        void Convert()//Преобразуем DateTime в время OPC
        {
            startTime = new Opc.Hda.Time(startDate);
            startTimedelta = new Opc.Hda.Time(startDate.AddHours(1));
            endTime = new Opc.Hda.Time(endDate);
            endTimedelta = new Opc.Hda.Time(endDate.AddHours(-1));
        }
    }
}
