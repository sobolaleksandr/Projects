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

namespace Scripts._986_88_Change
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 8;

            //string dayPath = @"D:\СуточныйСрез_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt";
            string dayPath = @"C:\СуточныйСрез_6_7_2020.txt";

            string[] ColumnNames = new string[] { "Desc", "Status", "Mode", "MskSet" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths);

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            initiator.GetAllDataFromServer();

            Report88Change report = new Report88Change(initiator.Sections, ColumnNames, MaxTestingValue, initiator.isTesting, dayPath);

            initiator.opcda.Disconnect();
            initiator.opchda.Disconnect();

            return report.result;
        }
    }
    public class Report88Change : Report88
    {
        readonly List<string> DABlocks = new List<string>();
        const string BlocksDaId = "5";

        readonly List<string> HDABlocks = new List<string>();
        const string BlocksHDaId = "2";

        readonly Dictionary<int, string> VLVDictionary = new Dictionary<int, string>
        {
            [2] = ". Местный режим",
        };
        readonly Dictionary<int, string> APDictionary = new Dictionary<int, string>
        {
            [1] = ". Верхняя аварийная уставка",
            [2] = ". Верхняя предупредительная уставка",
            [3] = ". Нижння предупредительная уставка",
            [4] = ". Нижняя аварийная уставка"
        };

        readonly List<string> AGTDArepair = new List<string>();
        readonly List<string> AGTHDArepair = new List<string>();
        readonly Dictionary<int, string> AGTStatusDictionary = new Dictionary<int, string>
        {
            [1] = "Ремонт",
        };

        readonly List<string> AGTDAoff = new List<string>();
        readonly List<string> AGTHDAoff = new List<string>();
        readonly Dictionary<int, string> AGTModeOnDictionary = new Dictionary<int, string>
        {
            [8] = "Включен"
        };

        readonly List<string> AGTDAon = new List<string>();
        readonly List<string> AGTHDAon = new List<string>();
        readonly Dictionary<int, string> AGTModeOffDictionary = new Dictionary<int, string>
        {
            [3] = "Отключен"
        };

        const string AGTHDaId = "1";
        const string AGTDaId = "4";

        readonly List<string> Actions = new List<string>();
        const string ActionsId = "3";

        object[] DaArray, HDaArray;
        string SectionName;

        public Report88Change(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting, string filepath) :
        base(sections, ColumnNames, MaxTestingValue, isTesting)
        {
            FillLists();

            WriteAGT(AGTHDAon, AGTHDAoff, AGTHDArepair, AGTHDaId);
            WriteAGT(AGTDAon, AGTDAoff, AGTDArepair, AGTDaId);

            WriteBlocks(HDABlocks, BlocksHDaId);
            WriteBlocks(DABlocks, BlocksDaId);

            FillActions(filepath);
            WriteActions(ActionsId);
        }

        public override void FillWithTestValues()
        {
            foreach (Section section in sections)
            {
                foreach (Tag item in section.Items)
                {
                    if (item.Name != "Desc")
                    {
                        item.DaData.RandomInt(MaxTestingValue);
                        item.HdaData.RandomInt(MaxTestingValue);
                    }
                }

            }
        }
        void FillLists()
        {
            foreach (Section section in sections)
            {
                SectionName = section.Name;
                foreach (Tag item in section.Items)
                {
                    DaArray = item.DaData.Values;
                    HDaArray = item.HdaData.Values;

                    if (item.Name == ColumnNames[0])
                    {
                        descArray = DaArray;
                    }
                    else if (item.Name == ColumnNames[1])
                    {
                        GetMode();
                    }
                    else if (item.Name == ColumnNames[2])
                    {
                        GetMask();
                    }
                    else if (item.Name == ColumnNames[3])
                    {
                        GetStatus();
                    }
                }
            }
        }

        void GetMode()
        {
            switch (SectionName)
            {
                case "AGT":
                    dictionary = AGTModeOnDictionary;
                    list = AGTDAon;
                    GetValue(DaArray);
                    list = AGTHDAon;
                    GetValue(HDaArray);

                    dictionary = AGTModeOffDictionary;
                    list = AGTDAoff;
                    GetValue(DaArray);
                    list = AGTHDAoff;
                    GetValue(HDaArray);
                    break;
                case "VLV":
                    dictionary = VLVDictionary;
                    list = DABlocks;
                    GetValue(DaArray);
                    list = HDABlocks;
                    GetValue(HDaArray);
                    break;
            }
        }
        void GetMask()
        {
            if (SectionName == "AP")
            {
                dictionary = APDictionary;
                list = DABlocks;
                GetValue(DaArray);
                list = HDABlocks;
                GetValue(HDaArray);
            }
        }
        void GetStatus()
        {
            if (SectionName == "AGT")
            {
                dictionary = AGTStatusDictionary;
                list = AGTDArepair;
                GetValue(DaArray);
                list = AGTHDArepair;
                GetValue(HDaArray);
            }
        }

        public override void AddDefaultValue()
        {

        }
        public override void AppendList(string value)
        {
            if (SectionName == "AGT")
            {
                list.Add(desc);
            }
            else
            {
                list.Add(desc + value);
            }
        }

        void WriteAGT(List<string> arrayOn, List<string> arrayOff,
            List<string> arrayRepair, string id)
        {
            int[] demension = { arrayOn.Count, arrayOff.Count, arrayRepair.Count };
            int length = GetMax(demension);

            for (int i = 0; i < length; i++)
            {
                row = table.NewRow();
                row[0] = id;
                row[1] = GetStringValueFromList(arrayOff, i);
                row[2] = GetStringValueFromList(arrayOn, i);
                row[3] = GetStringValueFromList(arrayRepair, i);
                table.Rows.Add(row);
            }
        }
        void WriteBlocks(List<string> Blocks, string id)
        {
            int length = Blocks.Count;

            for (int i = 0; i < length; i++)
            {
                row = table.NewRow();
                FillEmptySpaces();
                row[0] = id;
                row[1] = i.ToString();
                row[2] = Blocks[i];
                table.Rows.Add(row);
            }
        }
        void FillActions(string filepath)
        {
            System.IO.StreamReader stream =
                new System.IO.StreamReader(filepath, System.Text.Encoding.GetEncoding(1251));

            while (!stream.EndOfStream)
            {
                string tmp = stream.ReadLine();
                Actions.Add(tmp);
            }
            stream.Dispose();
        }
        void WriteActions(string id)
        {
            int count = Actions.Count;

            for (int i = 0; i < count; i++)
            {
                row = table.NewRow();
                FillEmptySpaces();
                row[0] = id;
                row[1] = i.ToString();
                row[2] = Actions[i];
                table.Rows.Add(row);
            }
        }
    }

}