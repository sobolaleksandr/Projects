using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class HDAData//Класс для работы с историческими данными
    {
        object[] FilteredValuesStart;
        object[] FilteredValuesEnd;

        double[] FilteredValuesStartDouble;
        double[] FilteredValuesEndDouble;

        public HDAData(ref List<Section> sections,//В конструкторе получаем данные за два периода. Затем вычитаем значения "начального" периода из "конечного" и получаем показания счетчиков за период
            HDAServer opchda, Time time)
        {
            foreach (Section section in sections)
            {
                foreach (Tag item in section.Items)
                {
                    GetData(time, opchda, item);
                    item.ValHDA.SetValue(
                        GetDifference(FilteredValuesStartDouble, FilteredValuesEndDouble));
                }
            }
        }

        void GetData(Time time, HDAServer opchda, Tag item)//Получаем данные за период и преобразуем их
        {
            FilteredValuesStart = opchda.GetData(time.startTime, time.startTimedelta, item.Path);
            FilteredValuesEnd = opchda.GetData(time.endTimedelta, time.endTime, item.Path);

            FilteredValuesStartDouble = ConvertToDouble(FilteredValuesStart);
            FilteredValuesEndDouble = ConvertToDouble(FilteredValuesEnd);
        }

        double[] ConvertToDouble(object[] array)//Преобразуем полученные с сервера данные к типу double. Если не получилось преобразовать, то считаем, что там 0
        {
            int length = array.Length;
            double[] output = new double[length];

            for (int i = 0; i < length; i++)
            {
                if (!Double.TryParse(array[i].ToString(), out output[i])
                    || array[i] == null)
                {
                    output[i] = 0d;
                }
            }

            return output;
        }

        object[] GetDifference(double[] start, double[] end)//Вычисляем разницу между значением на начало и конец периода
        {
            object[] output = new object[start.Length];

            for (int i = 0; i < start.Length; i++)
            {
                output[i] = end[i] - start[i];
            }
            return output;
        }


    }
}
