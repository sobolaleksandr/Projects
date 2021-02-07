using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Report130 : ReportBase
    {
        List<string> SelectedSections = new List<string>();

        double ActiveSum;
        double ReactiveSum;

        public Report130(List<Section> sections, string[] ColumnNames, int MaxTestingValue,
            List<string> SelectedSections, bool isTesting) :
            base(sections, ColumnNames, MaxTestingValue)//В конструкторе создаем "шапку" таблицы. Заполняем таблицу реальными или тестовыми значениями и подсчитываем сумму
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

        void FillWithTestValues()//Если тестируем, то заполняем double значениями
        {
            foreach (Section section in sections)
            {
                foreach (Tag item in section.Items)
                {
                    item.ValHDA.RandomDouble(MaxTestingValue);
                }
                FillRow(section);
            }
        }

        void FillTable()//Обрабатываем только те цеха, которы выбрал оператор
        {
            foreach (Section section in sections)
            {
                if (SelectedSections.Contains(section.Name))
                {
                    FillRow(section);
                }
            }
        }

        void FillRow(Section section)//Заполняем строку значениями
        {
            int[] demension = { section.Items[0].Path.Count, section.Items[1].Path.Count };
            int length = GetMax(demension);

            for (int j = 0; j < length; j++)
            {
                string path = section.Items[0].Path[j];
                string PIK = GetPIK(path);

                double val_R = GetDoubleValueFromArray(section.Items[0].ValHDA.GetValue(), j);
                double val_A = GetDoubleValueFromArray(section.Items[1].ValHDA.GetValue(), j);

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

        string GetPIK(string path)//Получаем номер счетчика из пути тега
        {
            Regex regex = new Regex(@"PIK(\w*)");
            MatchCollection matches = regex.Matches(path);
            string PIK = "";

            if (matches.Count > 0)
                PIK = matches[0].Value.Replace("PIK", "");

            return PIK;
        }

        void InitTable()//Создаем "шапку" таблицы
        {
            row = table.NewRow();
            row[0] = "Цех/Корпус";
            row[1] = "Активная мощность кВт*ч";
            row[2] = "Реактивная мощность Вар*ч";
            row[3] = "Счетчик";
            table.Rows.Add(row);
        }

        void SumTable()//Получаем суммарное значение активной и реактивной мощности по таблице
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
