using System.Collections.Generic;
using System.Data;
using EleSy.Common.Script;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;
using System.Linq;

namespace Scripts._986_88_Liquids
{
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const int MaxTestingValue = 0;

            string[] RowNames = { "Давление", "Температура", "Плотность", "Массовая доля воды",
                "Масса", "Суточный массовый дебит", "Суточный объемный дебит" };

            string[] Liquids = { "TZ", "PZZ ", "PZZ_n1", "CYCLE_arch" };
            string[] ColumnNames = { "Name", "TZ", "PZZ ", "PZZ_n1", "CYCLE_arch" };

            string[] LiquidTags = { "TazovskoeMR.IZU.TZ.liquid", "TazovskoeMR.IZU.PZZ.liquid",
                "TazovskoeMR.IZU.PZZ_n1.liquid", "TazovskoeMR.IZU.CYCLE_arch.liquid" };
            string[] Prefix = { ".mass", ".md", ".P", ".r", ".smd", ".T", ".svd" };

            ReportInitialize initiator = new ReportInitialize();

            Section section = new Section(Liquids, "defaultName");

            for (int i = 0; i < section.Items.Count; i++)
            {
                for (int j = 0; j < Prefix.Length; j++)
                    section.Items[i].Path.Add(LiquidTags[i] + Prefix[j]);

                section.Items[i].DaData.Values = initiator.opcda.GetData(section.Items[i].Path);
            }

            List<Section> sections = new List<Section>
            {
                section
            };

            Report88Liquids report = new Report88Liquids(sections, ColumnNames,
            MaxTestingValue, RowNames);

            initiator.opcda.Disconnect();
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
