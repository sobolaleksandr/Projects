using Reports.Servers;
using System;
using System.Collections.Generic;

namespace Reports.Infrostructure
{
    public class CodesCreater
    {
        readonly DAServer server;
        string path;
        string type;

        public CodesCreater(DAServer server, string type, string path)
        {
            this.server = server;
            this.type = type;
            this.path = path;
        }

        public Dictionary<int, string> CreateDictionary()
        {
            Dictionary<int, string> Codes =
    new Dictionary<int, string>();

            if (type != "AGT")
                return Codes;

            var values = CreateTags("");
            var description = CreateTags(".Description");

            object[] valuesData = server.GetData(values);
            object[] descriptionData = server.GetData(description);

            for (int i = 0; i < descriptionData.Length; i++)
                Codes.Add(System.Convert.ToInt32(valuesData[i]), descriptionData[i].ToString());

            return Codes;
        }

        List<string> CreateTags(string prefix)
        {
            int bitCount = 31;
            List<string> list = new List<string>();

            for (int i = 0; i < bitCount; i++)
            {
                string tag = String.Concat(path, System.Convert.ToInt32(i));
                tag = String.Concat(tag, prefix);

                list.Add(tag);
            }

            return list;
        }
    }

}
