using System.Collections.Generic;
using System.Data;
using EleSy.Common.Script;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;
using System.Linq;

namespace Scripts._1040_02Liquids
{
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 0;

            Dictionary<string, string> KP08 = new Dictionary<string, string> 
            {
                {".AT001", "Плотность жидкости"                             },
                {".FT001", "Объемный расход жидкости"                       },
                {".FT002", "Средний дебит жидкости на отводе"               },
                {".QT001", "Массовый расход жидкости"                       },
                {".QT002", "Мгновенные значения массового расхода жидкости" },
                {".TT001", "Средняя температура жидкости"                   }
            };

            string[] RowNames = new string[KP08.Count];
            string[] Prefix = new string[KP08.Count];

            KP08.Values.CopyTo(RowNames, 0);
            KP08.Keys.CopyTo(Prefix, 0);

            string[] Liquids = { "Liquid" };
            string[] ColumnNames = { "Name", "Liquid" };
            string[] LiquidTags = { "RussMR.KP008.MERA.Liquid" };

            ReportInitialize reportInitialize = new ReportInitialize(ColumnNames, FilteredPaths);
            reportInitialize.isTesting = true;

            Section section = new Section(Liquids, "defaultName");

            for (int i = 0; i < section.Items.Count; i++)
            {
                for (int j = 0; j < Prefix.Length; j++)
                    section.Items[i].Path.Add(LiquidTags[i] + Prefix[j]);

                section.Items[i].DaData.Values = reportInitialize.opcda.GetData(section.Items[i].Path);
            }

            List<Section> sections = new List<Section>
            {
                section
            };

            Report1040_02l report = new Report1040_02l(sections, ColumnNames,
            MaxTestingValue, RowNames);

            reportInitialize.opcda.TryToDisconnect();
            return report.result;
        }
        public class Report1040_02l : ReportBase
        {
            const int MaxTestingValueInt = 9;
            public Report1040_02l(List<Section> sections, string[] ColumnNames, int MaxTestingValue,
                string[] RowNames) :
            base(sections, ColumnNames, MaxTestingValue)
            {
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
                //row[2] = "Последний завершённый замер";
                //row[3] = "Последний завершённый замер по отводу n";
                //row[4] = "Циклический архив последних 100 отчетов";
                table.Rows.Add(row);
            }

            void FillTable(string[] RowNames)
            {
                int length = RowNames.Length;
                for (int i = 0; i < length; i++)
                {
                    row = table.NewRow();
                    row[0] = RowNames[i];
                    object[] values = sections[0].Items[0].DaData.Values;
                    row[1] = values[i];

                    //for (int j = 1; j < 5; j++)
                    //{
                    //    object[] values = sections[0].Items[j - 1].ValDA.GetValue();
                    //    row[j] = values[i];
                    //}
                    table.Rows.Add(row);
                }
            }
        }

    }
}
