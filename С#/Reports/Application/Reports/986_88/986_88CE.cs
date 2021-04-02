using System.Xml.Linq;
using Reports;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;
using Reports.Servers.Services;
using EleSy.Common.Script;
using System.Data;
using System.Collections.Generic;
using System;

namespace Scripts._986_88_CE
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 0;

            string[] ColumnNames = new string[] { "Desc", "CE" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths);
            List<Section> Sections = new List<Section>();

            Section section = initiator.Sections.Find(p => p.Name == "CE");
            Sections.Add(section);
            initiator.Sections = Sections;

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            initiator.GetAllDataFromServer();

            Report88CE report = new Report88CE(initiator.Sections, ColumnNames,
                MaxTestingValue, initiator.isTesting, initiator.opcda);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }
        public class Report88CE : ReportBase
        {
            object[] PAZ;
            readonly List<int> StateList = new List<int>
            {
                1,2,1024,2048,4096,8192,16384,32768,4,8,16,32,64,128,256,52
            };

            DAServer opcda;
            object[] descArray;
            object[] DaArray;

            public Report88CE(List<Section> sections, string[] ColumnNames, int MaxTestingValue, bool isTesting, DAServer opcda) :
            base(sections, ColumnNames, MaxTestingValue)
            {
                this.opcda = opcda;

                if (isTesting)
                {
                    FillWithTestValues();
                }

                FillLists();
                FillTable();
            }

            void FillWithTestValues()
            {
                Tag item = sections[0].GetItem(ColumnNames[1]);
                item.DaData.RandomIntFromList(StateList);
                item.HdaData.RandomIntFromList(StateList);
            }
            void FillLists()
            {
                foreach (Tag item in sections[0].Items)
                {
                    DaArray = item.DaData.Values;

                    if (item.Name == ColumnNames[0])
                    {
                        descArray = DaArray;
                    }
                    else if (item.Name == ColumnNames[1])
                    {
                        FillCE(item.Path);
                    }
                }
            }

            void FillCE(List<string> Path)
            {
                int length = DaArray.Length;
                List<string> CE = new List<string>();

                for (int i = 0; i < length; i++)
                {
                    CE.Add(
                        Path[i].Replace(".flg0", "")
                        + ".N"
                        + GetBit(i)
                        + ".Description");
                }

                PAZ = opcda.GetData(CE);
            }
            string GetBit(int i)
            {
                return
                    System.Convert.ToString(
                    Math.Log(
                    System.Convert.ToDouble(DaArray[i]), 2d));
            }
            void FillTable()
            {
                int length = PAZ.Length;

                for (int i = 0; i < length; i++)
                {
                    if (descArray[i] != null && PAZ[i] != null)
                    {
                        row = table.NewRow();
                        FillEmptySpaces();

                        row[0] = descArray[i].ToString();
                        row[1] = PAZ[i];

                        table.Rows.Add(row);
                    }
                }
            }
        }

    }
}
