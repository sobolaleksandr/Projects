using Reports.Infrostructure;

namespace Reports.Servers.Services
{
    public class HdaData
    {
        public HdaData(ref System.Collections.Generic.List<Section> sections,
            HDAServer opchda, System.DateTime startTime, System.DateTime endTime)
        {
            foreach (Section section in sections)
                foreach (Tag item in section.Items)
                    item.HdaData.Values = GetDifference(
                        ConvertToDouble(opchda.GetData(startTime, startTime.AddHours(1), item.Path)), 
                        ConvertToDouble(opchda.GetData(endTime.AddHours(-1), endTime, item.Path)));
        }

        double[] ConvertToDouble(object[] array)
        {
            int length = array.Length;
            double[] output = new double[length];

            for (int i = 0; i < length; i++)
                if (!System.Double.TryParse(array[i].ToString(), out output[i]) || array[i] == null)
                    output[i] = 0d;

            return output;
        }

        object[] GetDifference(double[] start, double[] end)
        {
            object[] output = new object[start.Length];

            for (int i = 0; i < start.Length; i++)
                output[i] = end[i] - start[i];

            return output;
        }

    }
}
