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

namespace Scripts._1042_02_operative
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
            string[] COLUMN_NAMES = {"value1", "value2", "value3"};
            string[] SPLITTER = new string[] { "Desc", "Value" };

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

            Report1042_operative report = new Report1042_operative
                (initiator.Sections, COLUMN_NAMES,
                MAX_TESTING_VALUE, initiator.isTesting,
                initiator.opcda, group);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }
    }
    public class Report1042_operative : ReportBase
    {
        readonly DAServer opcda;
        readonly bool isTesting;


        public Report1042_operative(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting,
            DAServer opcda, Area group 
            ) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            this.isTesting = isTesting;
            this.opcda = opcda;

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
            var items = SplitList(group.Items, ColumnNames.Length);
            var path = SplitList(group.Path, ColumnNames.Length);
            int length = items.Count;

            for (int i = 0; i < length; i++)
            {
                if (items[i].Count == 0)
                    break;

                FillRowWithValues(items[i]);
                FillTableWithDaValues(isTesting, opcda, path[i]);
            }
        }
    }

}
