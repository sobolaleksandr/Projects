using Reports.Infrostructure;
using Reports.Servers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Reports.Main
{
    public class ReportBase
    {
        public  DataSet result = new DataSet();

        protected List<Section> sections;
        protected int MaxTestingValue;
        protected DataRow row;
        protected DataTable table = new DataTable("ScriptData");
        protected string[] ColumnNames;

        protected const string StringDefaultValue = "Нет такого состояния";

        public ReportBase(List<Section> sections, string[] ColumnNames, int MaxTestingValue)
        {
            this.MaxTestingValue = MaxTestingValue;
            this.sections = sections;
            this.ColumnNames = ColumnNames;
            CreateTable(ColumnNames);
        }

        public string CreateHours(int index) =>
            (index * 2).ToString("00") + ":00";

        public double GetDoubleValueFromArray(object[] array, int numerator) =>
                array.Length > numerator ? System.Convert.ToDouble(array[numerator]) : 0;

        public string GetStringValueFromList(List<string> list, int numerator) =>
                list.Count > numerator ? list[numerator] : "";

        public void CreateTable(string[] ColumnNames)
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

        public void FillEmptySpaces()
        {
            int length = row.Table.Columns.Count;
            for (int i = 0; i < length; i++)
                row[i] = "";
        }

        public int GetMax(int[] array)
        {
            int Max = array[0];
            int length = array.Length;

            for (int i = 0; i < length; i++)
                if (Max < array[i])
                    Max = array[i];

            return Max;
        }

        public void FillHeader(int index, string value)
        {
            if (value == default)
                return;

            row = table.NewRow();
            FillEmptySpaces();
            row[index] = value;
            table.Rows.Add(row);
        }

        public void FillRowWithHeaders(List<string> values)
        {
            if (values.Count == 0)
                return;

            row = table.NewRow();
            FillEmptySpaces();

            int length = values.Count;

            for (int i = 0; i < length; i++)
                row[i] = values[i];

            table.Rows.Add(row);
        }


        public void FillRowWithValues(bool isTesting, HDAServer opchda, List<string> values)
        {
            if (values.Count == 0)
                return;

            for (int i = 0; i < 12; i++)
            {
                DateTime startTime = DateTime.Now.AddHours(-1 - i * 2);
                DateTime endTime = DateTime.Now.AddHours(-i * 2);

                Data array = new Data { Values = opchda.GetData(startTime, endTime, values) };

                if (isTesting)
                    array.RandomDouble(MaxTestingValue);

                var list = array.ConvertToString().ToList();

                foreach (var value in list)
                    Console.WriteLine("{0} - startTime, {1} - endTime, {2} - value", startTime, endTime, value);

                list.Insert(0, CreateHours(i));

                FillRowWithHeaders(list);
            }
        }

        public List<List<string>> SplitList(List<string> list, int chunkLength)
        {
            return Enumerable.Range(0, 10)
                             .Select(i => list.Skip(i * chunkLength)
                                              .Take(chunkLength)
                                              .ToList())
                             .ToList();
        }
    }

}
