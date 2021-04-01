using Reports.Infrostructure;
using Reports.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports.Main
{
    public class Report88Day : ReportBase
    {
        bool isTesting;
        HDAServer opchda;
        int valueColsCount;

        public Report88Day(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting,
            List<ControlPoint> KpList, HDAServer opchda, int valueColsCount) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            this.isTesting = isTesting;
            this.opchda = opchda;
            this.valueColsCount = valueColsCount;

            foreach (var kp in KpList)
            {
                string KpName = kp.Name;

                foreach (var group in kp.GroupList)
                {
                    FillHeader(1, KpName);
                    FillHeader(1, group.Name);

                    FillTableWithGroup(group);
                }
            }
        }

        public void FillTableWithGroup(Area group)
        {
            var items = SplitList(group.Items, valueColsCount);
            var path = SplitList(group.Path, valueColsCount);
            int length = items.Count;

            for (int i = 0; i < length; i++)
            {
                if (items[i].Count == 0)
                    break;

                items[i].Insert(0, "Время");
                FillRowWithValues(items[i]);
                FillTableWithHdaValues(isTesting, opchda, path[i], 24 ,2);
            }
        }
    }
}
