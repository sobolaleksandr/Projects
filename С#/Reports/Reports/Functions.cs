using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml.Linq;

namespace Reports
{
    public class Script //Стадартный класс для формирования отчета в SCADA
    {
        public DataSet GetData(object[] parameters)//Стандартная функция для формирования отчета
        {
            //Определяем начальные настройки отчета. Сортируем теги по Цеху (Section), затем по счетчику (PIK), и уже потом типу Wh(реактивная) и Var(активная) мощность
            const bool FilteredPaths = true; 
            const int MaxTestingValue = 100;//Максимальное значение для тестирования функции

            string[] ColumnNames = new string[] { "Section", "Value_active", "Value_reactive", "PIK"};
            string[] Splitter = new string[] { "Wh", "Var" };

            Initiator initiator = new Initiator(Splitter, FilteredPaths);//Функция инициализации

            object[] DaData = initiator.opcda.GetData(initiator.settings.Sections);//Получаем оперативные данные с сервера

            for (int i = 0; i < DaData.Length - 2; i++)//В данном отчете было необходимо проверить определенный список тегов, который отвечает за то надо ли включать данный цех в отчет
            {
                if (DaData[i] != null)
                {
                    DaData[i] = System.Convert.ToBoolean(DaData[i]);
                }
                else
                {
                    DaData[i] = true;//Если у нас не получилось получить знаачение с сервера, то лучше добавить лишний цех в отчет, чем пропустить нужный
                }
            }

            initiator.opcda.Disconnect();//Получили все оперативные данные. Можем отключаться от сервера

            Time time = new Time(DaData);//Получаем время формирования отчета

            List<string> SelectedSections = new List<string>();

            for (int i = 0; i < DaData.Length - 2; i++)//Проверяем какие секции идут в отчет
            {
                if (System.Convert.ToBoolean(DaData[i]))
                {
                    SelectedSections.Add(initiator.settings.Labels[i]);
                }
            }

            HDAData data = new HDAData(ref initiator.Sections, initiator.opchda, time);//Обрабатываем исторические данные
            Report130 report = new Report130(initiator.Sections, ColumnNames, MaxTestingValue, SelectedSections, initiator.isTesting);//Формируем отчет
            
            initiator.opchda.Disconnect();// Отключаемся от исторического сервера

            //Можем сформировать "шапку" отчета, если делаем xdoc. 
            //Надо добавить в параметры функции эту строку

            //StartingString = "Начало периода " + time.startDate.ToString() + ". Конец периода " + time.endDate.ToString();
            
            return report.result;//Возращаем таблицу данных
        }       

    }
}






