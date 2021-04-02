using EleSy.Common.Logging;
using Reports.FileHandler;
using Reports.Infrostructure;
using Reports.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Main
{
    public class ReportInitialize
    {
        public DAServer opcda;
        public HDAServer opchda;
        public List<Section> Sections;
        public bool isTesting = true;
        public bool FailedDaConnection = true;
        public bool FailedHdaConnection = true;

        private readonly XmlHandler settings;

        public ReportInitialize()
        {

            const string xPath = @"C:\settings.xml";
            settings = new XmlHandler(xPath);
            
            opcda = new DAServer(settings.DaProperties);

            if (opcda.TryToConnect())
                FailedDaConnection = false;
        }

        public ReportInitialize(string[] Splitter, bool FilteredPaths) : this()
        {
            IniHandler inidata = new IniHandler(settings.IniPath, Splitter, FilteredPaths);
            Sections = inidata.Parse();

            opchda = new HDAServer(settings.HdaProperties);

            if (opchda.TryToConnect())
                FailedHdaConnection = false;
        }

        public void GetAllDataFromServer()
        {
            DateTime startTime = DateTime.Now.AddHours(-13);
            DateTime endTime = DateTime.Now.AddHours(-12);

            foreach (Section section in Sections)
                foreach (Tag item in section.Items)
                {
                    item.DaData.Values = opcda.GetData(item.Path);
                    item.HdaData.Values = opchda.GetData(startTime,endTime,item.Path);
                }
        }
    }
}
