using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports.Infrostructure
{
    public class Tag
    {
        public string Name;
        public List<string> Path;
        public Data DaData;
        public Data HdaData;
        public Tag(string Name)
        {
            this.Name = Name;
            Path = new List<string>();
            DaData = new Data();
            HdaData = new Data();
        }
    }

}
