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

namespace Scripts._130
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>

    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = true;
            const int MaxTestingValue = 100;

            string[] ColumnNames = new string[] { "Section", "Value_active", "Value_reactive", "PIK" };
            string[] Splitter = new string[] { "Wh", "Var" };

            List<string> areaPaths = new List<string>{"Reports.Section1","Reports.Section2","Reports.Section3",
            "Reports.Section4","Reports.Section5","Reports.Section6","Reports.Section7",
            "Reports.Section8","Reports.Section9","Reports.Section10","Reports.EndDate","Reports.StartDate"};

            string[] Labels = { "22/7", "22/40", "54/7", "23/40", "36/40", "41/40",
            "Подвал/40", "38/1,2", "Подвал/2,3", "Инженерный корпус" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths);

            object[] DaData = initiator.opcda.GetData(areaPaths);


            for (int i = 0; i < DaData.Length - 2; i++)
            {
                if (DaData[i] != null)
                {
                    DaData[i] = System.Convert.ToBoolean(DaData[i]);
                }
                else
                {
                    DaData[i] = true;
                }
            }

            initiator.opcda.TryToDisconnect();

            TimeHandler time = new TimeHandler(DaData);

            List<string> SelectedSections = new List<string>();

            for (int i = 0; i < DaData.Length - 2; i++)
            {
                if (System.Convert.ToBoolean(DaData[i]))
                {
                    SelectedSections.Add(Labels[i]);
                }
            }

            HdaData data = new HdaData(ref initiator.Sections, initiator.opchda, time.startTime, time.endTime);
            Report130 report = new Report130(initiator.Sections, ColumnNames, 
                MaxTestingValue, SelectedSections, initiator.isTesting);

            initiator.opchda.TryToDisconnect();

            //StartingString = "Начало периода " + time.startDate.ToString() + ". Конец периода " + time.endDate.ToString();

            return report.result;
        }

        public class TimeHandler
        {
            public DateTime endTime;
            public DateTime startTime;

            public TimeHandler(object[] DaData)
            {
                int lastItemIndex = DaData.Length - 1;
                int prevLastItemIndex = lastItemIndex - 1;

                string lastItem = DaData[lastItemIndex].ToString();
                string prevLastItem = DaData[prevLastItemIndex].ToString();

                if (ParseResult(lastItem, prevLastItem))
                {
                    SetProper();
                }
                else
                {
                    SetDefault();
                }
            }

            bool ParseResult(string lastItem, string prevLastItem)
            {
                bool ParseResult = DateTime.TryParse(lastItem, out endTime) &&
                                   DateTime.TryParse(prevLastItem, out startTime);

                return ParseResult;
            }

            void SetProper()
            {
                if (endTime > DateTime.Now)
                {
                    endTime = DateTime.Now;
                }

                if (endTime == startTime)
                {
                    startTime = endTime.AddHours(-24);
                }
            }

            void SetDefault()
            {
                endTime = DateTime.Now;
                startTime = DateTime.Now.AddHours(-24);
            }
        }

        public class Report130 : ReportBase
        {
            readonly List<string> SelectedSections = new List<string>();

            double ActiveSum;
            double ReactiveSum;

            public Report130(List<Section> sections, string[] ColumnNames, int MaxTestingValue,
                List<string> SelectedSections, bool isTesting) :
                base(sections, ColumnNames, MaxTestingValue)
            {
                this.SelectedSections = SelectedSections;

                InitTable();
                if (isTesting)
                {
                    FillWithTestValues();
                }
                else
                {
                    FillTable();
                }

                SumTable();
            }

            void FillWithTestValues()
            {
                foreach (Section section in sections)
                {
                    foreach (Tag item in section.Items)
                    {
                        item.HdaData.RandomDouble(MaxTestingValue);
                    }
                    FillRow(section);
                }
            }

            void FillTable()
            {
                foreach (Section section in sections)
                {
                    if (SelectedSections.Contains(section.Name))
                    {
                        FillRow(section);
                    }
                }
            }

            void FillRow(Section section)
            {
                int[] demension = { section.Items[0].Path.Count, section.Items[1].Path.Count };
                int length = GetMax(demension);

                for (int j = 0; j < length; j++)
                {
                    string path = section.Items[0].Path[j];
                    string PIK = GetPIK(path);

                    double val_R = GetDoubleValueFromArray(section.Items[0].HdaData.Values, j);
                    double val_A = GetDoubleValueFromArray(section.Items[1].HdaData.Values, j);

                    ReactiveSum += val_R;
                    ActiveSum += val_A;

                    row = table.NewRow();
                    row[0] = section.Name;
                    row[1] = val_A;
                    row[2] = val_R;
                    row[3] = PIK;
                    table.Rows.Add(row);

                }
            }

            string GetPIK(string path)
            {
                Regex regex = new Regex(@"PIK(\w*)");
                MatchCollection matches = regex.Matches(path);
                string PIK = "";

                if (matches.Count > 0)
                    PIK = matches[0].Value.Replace("PIK", "");

                return PIK;
            }

            void InitTable()
            {
                row = table.NewRow();
                row[0] = "Цех/Корпус";
                row[1] = "Активная мощность кВт*ч";
                row[2] = "Реактивная мощность Вар*ч";
                row[3] = "Счетчик";
                table.Rows.Add(row);
            }

            void SumTable()
            {
                row = table.NewRow();
                row[0] = "Общее";
                row[1] = ActiveSum.ToString("0.00");
                row[2] = ReactiveSum.ToString("0.00");
                row[3] = "";
                table.Rows.Add(row);
            }
        }

    }

}
