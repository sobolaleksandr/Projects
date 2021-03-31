using System.Collections.Generic;
using System.Data;
using EleSy.Common.Script;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;
using System.Linq;
using System;

namespace Scripts._986_88_AGT
{
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 8;

            Dictionary<int, string> StatusDictionary = new Dictionary<int, string>
            {
                {1,"Отключен, нет напряжения" },
                {2,"Отключен, цепи включения неисправны" },
                {3,"Отключен" },
                {4,"Отключается" },
                {5,"Включается" },
                {6,"Включен, пускатель неисправен" },
                {7,"Включен, цепи отключения неисправны" },
                {8,"Включен" }
            };

            List<int> FailList = new List<int>
            {
                1,2,1024,2048,4096,8192,16384,32768,4,8,16,32,64,128,256,512
            };

            string[] ColumnNames = new string[] { "Desc", "Status", "Fail" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths);

            CodesCreater prefix = new CodesCreater(initiator.opcda, "AGT", "codes.AGT.fail.bit");
            Dictionary<int, string> FailDictionary = prefix.CreateDictionary();

            List<Section> Sections = new List<Section>();

            Section section = initiator.Sections.Find(p => p.Name == "AGT");
            Sections.Add(section);
            initiator.Sections = Sections;

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            initiator.GetData();

            Report88Device report = new Report88Device(initiator.Sections, ColumnNames,
                MaxTestingValue, initiator.isTesting,
                FailDictionary, StatusDictionary, FailList);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }

        public class Report88Liquids : ReportBase
        {
            const int MaxTestingValueInt = 9;
            public Report88Liquids(List<Section> sections, string[] ColumnNames, int MaxTestingValue,
                string[] RowNames) :
            base(sections, ColumnNames, MaxTestingValue)
            {
                FillItems();
                InitTable();
                FillTable(RowNames);
            }

            void FillItems()
            {
                foreach (Section section in sections)
                {
                    foreach (Tag item in section.Items)
                    {
                        item.DaData.RandomInt(MaxTestingValueInt);
                    }
                }
            }

            void InitTable()
            {
                row = table.NewRow();
                row[0] = "";
                row[1] = "Текущий замер";
                row[2] = "Последний завершённый замер";
                row[3] = "Последний завершённый замер по отводу n";
                row[4] = "Циклический архив последних 100 отчетов";
                table.Rows.Add(row);
            }

            void FillTable(string[] RowNames)
            {
                int length = RowNames.Length;
                for (int i = 0; i < length; i++)
                {
                    row = table.NewRow();
                    row[0] = RowNames[i];

                    for (int j = 1; j < 5; j++)
                    {
                        object[] values = sections[0].Items[j - 1].DaData.Values;
                        row[j] = values[i];
                    }
                    table.Rows.Add(row);
                }
            }
        }
    }
}
