using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class ReportBase//Общий класс отчета. От него наследуются отчеты под каждый проект
    {
        public List<Section> sections;
        public int MaxTestingValue;
        public DataRow row;
        public DataSet result = new DataSet();
        public DataTable table = new DataTable("ScriptData");
        public string[] ColumnNames;

        public const string StringDefaultValue = "Нет такого состояния";

        public ReportBase(List<Section> sections, string[] ColumnNames, int MaxTestingValue)//В конструкторе создаем таблицу
        {
            this.MaxTestingValue = MaxTestingValue;
            this.sections = sections;
            this.ColumnNames = ColumnNames;
            CreateTable(ColumnNames);
        }

        public string CreateHours(int index)//Преобразуем обычное число к форме времени 12-> 24:00
        {
            return (index * 2).ToString("00") + ":00";
        }

        public double GetDoubleValueFromArray(object[] array, int numerator)
        {
            double result = 0;
            if (array.Length > numerator)
            {
                result = System.Convert.ToDouble(array[numerator]);
            }
            return result;
        }

        public string GetStringValueFromList(List<string> array, int numerator)
        {
            string val = "";

            if (array.Count > numerator)
            {
                val = array[numerator];
            }

            return val;
        }

        public void CreateTable(string[] ColumnNames)//Создаем таблицу
        {
            foreach (string name in ColumnNames)
            {
                DataColumn column = new DataColumn(name, typeof(System.String))
                {
                    AllowDBNull = false
                };

                table.Columns.Add(column);
            }
            result.Tables.Add(table);
        }

        public void FillEmptySpaces()//Для корректного вывода данных, заполняем пустыми элементами таблицу
        {
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                row[i] = "";
            }
        }

        public int GetMax(int[] array)//Получаем максимальный элемент массива
        {
            int Max = array[0];
            int length = array.Length;

            for (int i = 0; i < length; i++)
            {
                if (Max < array[i])
                {
                    Max = array[i];
                }
            }

            return Max;
        }
    }

}
