using EleSy.Common.Script;
using Reports.FileHandler;
using Reports.Infrostructure;
using Reports.Main;
using Reports.Servers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Scripts._1042_02
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>
    public class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FILTERED_PATHS = false;
            const int MAX_TESTING_VALUE = 100;
            string[] COLUMN_NAMES = { "TimeStamp", "value1", "value2", "value3"};
            string[] SPLITTER = new string[] { "Desc", "Value" };
            const int VALUE_COLS_COUNT = 3;
            string[] KpRegex = new[]
            {
                ""
            };
            string[] ObjectRegex = new[]
            {
                @"ШУП МКС(\w*\s)"
            };

            ReportInitialize initiator = new ReportInitialize(SPLITTER, FILTERED_PATHS);
            initiator.isTesting = false;

            var motoItem = initiator.Sections.Find(p => p.Name == "Elem_TI").Items[0];

            foreach (string tag in motoItem.Path)
            {
                if (tag.Contains("MOTO"))
                {
                    motoItem.Path.Clear();
                    motoItem.Path.Add(tag);
                    break;
                }
            }

            var paths = initiator.Sections.Find(p => p.Name == "AP4").Items[0].Path;
            paths.AddRange(initiator.Sections.Find(p => p.Name == "AP2").Items[0].Path);
            paths.AddRange(initiator.Sections.Find(p => p.Name == "Elem_TI").Items[0].Path);

            Tag item = new Tag("");
            item.Path = paths;
            item.HdaData.Values = new object[paths.Count];
            item.DaData.Values = new object[paths.Count];


            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;          

            //Serializer serializer = new Serializer(item, KpRegex, ObjectRegex, new string[0]);

            Report1042 report = new Report1042(initiator.Sections, COLUMN_NAMES,
                MAX_TESTING_VALUE, initiator.isTesting,
                initiator.opchda, initiator.opcda, VALUE_COLS_COUNT);

            //Report88Day report = new Report88Day(initiator.Sections, COLUMN_NAMES,
            //    MAX_TESTING_VALUE, initiator.isTesting, serializer.KpList,
            //    initiator.opchda, VALUE_COLS_COUNT);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }
    }
    public class Report1042 : ReportBase
    {
        HDAServer opchda;
        readonly int valueColsCount;
        readonly bool isTesting;

        public Report1042(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting,
            HDAServer opchda, DAServer opcda, int valueColsCount) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            this.isTesting = isTesting;
            this.opchda = opchda;
            this.valueColsCount = valueColsCount;

            foreach (Section section in sections)
            {
                Area group = new Area();

                foreach (Tag item in section.Items)
                {
                    if (item.Name == "Desc")
                        group.Items = ConvertToStringList(opcda.GetData(item.Path));
                    else if (item.Name == "Value")
                        group.Path = item.Path;
                }

                FillTableWithGroup(group);
            }
        }

        public List<string> ConvertToStringList(object[] array)
        {
            List<string> result = new List<string>();
            int length = array.Length;

            for (int i=0; i<length; i++)
            {
                string value = array[i] != null ? array[i].ToString() : "";
                result.Add(value);
            }

            return result;
        }

        public void FillTableWithGroup(Area group)
        {
            var items = SplitList(group.Items, valueColsCount);
            var path = SplitList(group.Path, valueColsCount);
            int length = items.Count;

            for (int i = 0; i < length; i++)
            {
                if (items[i].Count == 0)
                    break;

                items[i].Insert(0, "Время");
                FillRowWithHeaders(items[i]);
                FillRowWithValues(isTesting, opchda, path[i]);
            }
        }
    }

}
