using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using EleSy.Common.Script;
using System.Xml.Linq;
using Reports;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;
using Reports.Servers.Services;

namespace Scripts._1040_02Day
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>

    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 100;
            const int valueColsCount = 6;
            string[] ColumnNames = new string[] { "TimeStamp", "value1", "value2", 
                "value3", "value4", "value5", "value6"};
            string[] Splitter = new string[] { "Desc", "Val" };
            string[] KpRegex = new[]
            {
                @"Кустовая площадка №(\w*\s)"
            };
            string[] ObjectRegex = new[]
            {
                @"Скважина №(\w*\s)",
                @"Камера приема СОД на УНПГ(\w*\s)",
                @"Площадка отключающей арматуры(\w*\s)"
            };
            string[] ObjectPrefix = new[]
{
                @"добывающая ",
                @"нагнетательная "
            };

            ReportInitialize reportInitialize = new ReportInitialize(Splitter, FilteredPaths);

            Section section = reportInitialize.Sections.Find(p => p.Name == "AP4");
            reportInitialize.Sections = new List<Section> { section };

            PathCreater append = new PathCreater(reportInitialize.Sections);
            reportInitialize.Sections = append.sections;

            Tag item = reportInitialize.Sections[0].Items.Find(i => i.Name == "Desc");
            item.DaData.Values = reportInitialize.opcda.GetData(item.Path);


            Serializer serializer = new Serializer(item, KpRegex, ObjectRegex, ObjectPrefix);

            Report88Day report = new Report88Day(reportInitialize.Sections, ColumnNames,
                MaxTestingValue, reportInitialize.isTesting,
                serializer.KpList, reportInitialize.opchda, valueColsCount);

            reportInitialize.opcda.TryToDisconnect();
            reportInitialize.opchda.TryToDisconnect();

            return report.result;
        }
    }

}
