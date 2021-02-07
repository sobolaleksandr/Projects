using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public class Data//Класс для работы с данными по тегу
    {
        object[] array;
        int length;
        Random rand;

        const string DefaultStringValue = "";

        public Data(int Demension)//Создаем массив тегов
        {
            array = new object[Demension];
            length = array.Length;
            rand = new Random(System.Convert.ToInt32(DateTime.Now.Millisecond));
        }

        public string[] ConvertToString()//Преобразуем массив Object[] в string[]
        {
            string[] output = new string[length];

            for (int i = 0; i < length; i++)
            {
                if (array[i] == null)
                {
                    output[i] = DefaultStringValue;
                    continue;
                }

                output[i] = array[i].ToString();
            }
            return output;
        }

        public object[] GetValue()//Получаем массив тегов
        {
            return array;
        }

        public void SetValue(object[] InputArray)//Записываем массив
        {
            array = InputArray;
            length = array.Length;
        }

        public void RandomDouble(int MaxValue)//Заполняем массив типом double(для тестов)
        {
            for (int j = 0; j < length; j++)
            {
                array[j] = (rand.NextDouble() * MaxValue).ToString("00.00");
            }
        }

        public void RandomInt(int MaxValue)//Заполняем массив типом int(для тестов)
        {
            for (int j = 0; j < length; j++)
            {
                array[j] = rand.Next(MaxValue) + 1;
            }
        }

        public void RandomIntFromList(List<int> list)//Заполняем массив определенным списком значений(для тестов)
        {
            for (int j = 0; j < length; j++)
            {
                int index = rand.Next(list.Count);
                array[j] = list[index];
            }
        }

        public void RandomBool()//Заполняем массив типом bool(для тестов)
        {
            for (int i = 0; i < length; i++)
            {
                array[i] = false;

                double value = rand.NextDouble();
                if (value > 0.5d)
                {
                    array[i] = true;
                }
            }
        }
    }    
}
