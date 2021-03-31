using Reports.Infrostructure;
using System.Collections.Generic;

namespace Reports.Main
{
    public abstract class Report88 : ReportBase
    {
        public object[] descArray;
        public List<string> list;
        public Dictionary<int, string> dictionary;
        public string desc;

        public Report88(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            if (isTesting)
                FillWithTestValues();
        }

        public abstract void FillWithTestValues();

        public void GetValue(object[] array)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                int mode = System.Convert.ToInt32(array[i]);

                if (descArray[i] == null)
                    continue;

                desc = descArray[i].ToString();
                FindItem(mode);
            }
        }

        public void FindItem(int mode)
        {
            if (!dictionary.ContainsKey(mode))
            {
                AddDefaultValue();
                return;
            }

            foreach (var elem in dictionary)
            {
                if (mode == elem.Key)
                {
                    AppendList(elem.Value);
                    return;
                }
            }
        }

        public abstract void AddDefaultValue();
        public abstract void AppendList(string value);
    }

}
