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

namespace Scripts._986_88_Day
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
            string[] ColumnNames = new string[] { "TimeStamp", "value1", "value2", "value3", "value4", "value5", "value6" };
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

            ReportInitialize initiator = new ReportInitialize(Splitter, FilteredPaths);
            
            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            Tag item = initiator.Sections[0].Items.Find(i => i.Name == "Desc");
            item.DaData.Values = initiator.opcda.GetData(item.Path);

            Serializer serializer = new Serializer(item, KpRegex, ObjectRegex, new string[0]);

            Report88Day report = new Report88Day(initiator.Sections, ColumnNames,
                MaxTestingValue, initiator.isTesting,
                serializer.KpList, initiator.opchda, valueColsCount);

            initiator.opcda.TryToDisconnect();
            initiator.opchda.TryToDisconnect();

            return report.result;
        }
    }
}
