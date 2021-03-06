﻿using EleSy.Common.Script;
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
            const int RANGE_IN_HOURS = 24;
            const int TIME_INTERVAL_IN_HOURS = 2;
            const bool FILTERED_PATHS = false;
            const int MAX_TESTING_VALUE = 100;
            string[] COLUMN_NAMES = { "TimeStamp", "value1", "value2", "value3"};
            string[] SPLITTER = new string[] { "Desc", "Value" };
            const int VALUE_COLS_COUNT = 3;

            ReportInitialize initiator = new ReportInitialize(SPLITTER, FILTERED_PATHS);
            initiator.isTesting = false;

            Area group = new Area()
            {
                Name = "",
                Items = new List<string>()
                {
                    "MKS1.TI.MOTO.Description"
                },
                Path = new List<string>()
                {
                    "MKS1.TI.MOTO"
                }
            };

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            foreach (var section in initiator.Sections)
            {
                if (section.Name == "AP4" || section.Name == "AP2")
                {
                    group.Items.AddRange(section.Items[0].Path);
                    group.Path.AddRange(section.Items[1].Path);
                }
            }

            Report1042 report = new Report1042(initiator.Sections, COLUMN_NAMES,
                MAX_TESTING_VALUE, initiator.isTesting,
                initiator.opchda, initiator.opcda, VALUE_COLS_COUNT, 
                RANGE_IN_HOURS, TIME_INTERVAL_IN_HOURS, group);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }
    }
    public class Report1042 : ReportBase
    {
        readonly HDAServer opchda;
        readonly int valueColsCount;
        readonly bool isTesting;
        readonly int rangeInHours;
        readonly int timeIntevalInHours;

        public Report1042(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting,
            HDAServer opchda, DAServer opcda, int valueColsCount, 
            int rangeInHours, int timeIntevalInHours, Area group) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            this.isTesting = isTesting;
            this.opchda = opchda;
            this.valueColsCount = valueColsCount;
            this.rangeInHours = rangeInHours;
            this.timeIntevalInHours = timeIntevalInHours;

            group.Items = ConvertToStringList(opcda.GetData(group.Items));
            FillTableWithGroup(group);
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
                FillRowWithValues(items[i]);
                FillTableWithHdaValues(isTesting, opchda, path[i], 
                    rangeInHours, timeIntevalInHours);
            }
        }
    }

}
