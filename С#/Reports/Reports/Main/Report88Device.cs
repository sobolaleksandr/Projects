using Reports.Infrostructure;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Reports.Main
{
    public class Report88Device : Report88
    {
        readonly List<string> Status = new List<string>();
        readonly List<string> Fail = new List<string>();

        public Report88Device(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting,
            Dictionary<int, string> FailDictionary, Dictionary<int, string> StatusDictionary,
            List<int> FailList) :
            base(sections, ColumnNames, MaxTestingValue, isTesting)
        {
            if (isTesting)
                FailTestValue(FailList);

            FillLists(FailDictionary, StatusDictionary);
            FillTable();
        }

        public override void FillWithTestValues()
        {
            Tag Item = sections[0].GetItem(ColumnNames[1]);
            if (Item != null)
            {
                Item.DaData.RandomInt(MaxTestingValue);
                Item.HdaData.RandomInt(MaxTestingValue);
            }
        }
        void FailTestValue(List<int> FailList)
        {
            Tag Item = sections[0].GetItem(ColumnNames[2]);
            if (Item != null)
            {
                Item.DaData.RandomIntFromList(FailList);
                Item.HdaData.RandomIntFromList(FailList);
            }
        }

        void FillLists(Dictionary<int, string> FailDictionary,
            Dictionary<int, string> StatusDictionary)
        {
            foreach (Tag item in sections[0].Items)
            {
                object[] DaArray = item.DaData.Values;

                if (item.Name == ColumnNames[0])
                    descArray = DaArray;
                else if (item.Name == ColumnNames[1])
                {
                    list = Status;
                    dictionary = StatusDictionary;
                    GetValue(DaArray);
                }
                else if (item.Name == ColumnNames[2])
                {
                    list = Fail;
                    dictionary = FailDictionary;
                    GetValue(DaArray);
                }
            }
        }

        public override void AddDefaultValue()
        {
            list.Add(StringDefaultValue);
        }
        public override void AppendList(string value)
        {
            list.Add(value);
        }

        void FillTable()
        {
            int length = Status.Count;
            Regex regex = new Regex("(Скважина (\\w*) |Площадка отключающей арматуры(\\w*) )");

            for (int i = 0; i < length; i++)
            {
                if (descArray[i] != null)
                {
                    row = table.NewRow();
                    FillEmptySpaces();
                    //TODO: убрать часть имени
                    MatchCollection matches = regex.Matches(descArray[i].ToString());
                    string str = "defe";
                    if (matches.Count > 0)
                        str = matches[0].Value;

                    row[0] = descArray[i].ToString().Replace(str + " ", "");
                    row[1] = Fail[i];
                    row[2] = Status[i];

                    table.Rows.Add(row);
                }
            }
        }
    }

}
