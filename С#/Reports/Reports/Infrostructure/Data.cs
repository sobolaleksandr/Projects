using System;
using System.Collections.Generic;

namespace Reports.Infrostructure
{
    public class Data
    {
        private object[] values;
        public object[] Values
        {
            get => values;
            set
            {
                values = value;
                length = value.Length;
            }
        }

        int length;
        readonly Random rand;

        public Data() =>
            rand = new Random(System.Convert.ToInt32(DateTime.Now.Millisecond));

        public string[] ConvertToString()
        {
            string[] output = new string[length];

            for (int i = 0; i < length; i++)
                output[i] = Values[i] != null? Values[i].ToString() : "";

            return output;
        }

        public void RandomDouble(int MaxValue)
        {
            for (int j = 0; j < length; j++)
                Values[j] = (rand.NextDouble() * MaxValue).ToString("00.00");
        }

        public void RandomInt(int MaxValue)
        {
            for (int j = 0; j < length; j++)
                Values[j] = rand.Next(MaxValue) + 1;
        }

        public void RandomIntFromList(List<int> list)
        {
            for (int j = 0; j < length; j++)
                Values[j] = list[rand.Next(list.Count)];
        }

        public void RandomBool()
        {
            for (int i = 0; i < length; i++)
                Values[i] = rand.NextDouble() > 0.5d;
        }
    }

}
