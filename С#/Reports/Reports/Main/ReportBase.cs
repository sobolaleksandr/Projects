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

        public string CreateHours(DateTime time) =>
            time.ToShortTimeString();

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

        public void FillRowWithValues(List<string> values)
        {
            if (values.Count == 0)
                return;

            row = table.NewRow();
            FillEmptySpaces();

            int length = values.Count;

            for (int i = 0; i < length; i++)
                row[i] = TryConvertToDouble(values[i]);

            table.Rows.Add(row);
        }

        string TryConvertToDouble(string value)
        {
            if (Double.TryParse(value, out double doubleValue))
                return doubleValue.ToString("0.000");
            else
                return value;
        }

        public void FillTableWithHdaValues(bool isTesting, HDAServer opchda, List<string> values, 
            int rangeInHours, int timeIntervalInHours)
        {
            if (values.Count == 0)
                return;

            for (int i = 0; i < rangeInHours; i++)
            {
                DateTime startTime = DateTime.Now.AddHours(-1 - i * timeIntervalInHours);
                DateTime endTime = DateTime.Now.AddHours(-i * timeIntervalInHours);

                Data array = new Data { Values = opchda.GetData(startTime, endTime, values) };

                if (isTesting)
                    array.RandomDouble(MaxTestingValue);

                var list = array.ConvertToString().ToList();

                list.Insert(0, CreateHours(endTime));

                FillRowWithValues(list);
            }
        }

        
        public void FillTableWithDaValues(bool isTesting, DAServer opcda, List<string> tags)
        {
            if (tags.Count == 0)
                return;

            Data DaData = new Data { Values = opcda.GetData(tags) };

            if (isTesting)
                DaData.RandomDouble(MaxTestingValue);

            FillRowWithValues(DaData.ConvertToString().ToList());
        }

        public List<List<string>> SplitList(List<string> list, int chunkLength)
        {
            return Enumerable.Range(0, list.Count/chunkLength + 1)
                             .Select(i => list.Skip(i * chunkLength)
                                              .Take(chunkLength)
                                              .ToList())
                             .ToList();
        }
    }

}
