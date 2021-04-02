using System.Xml.Linq;


namespace Reports.FileHandler
{
    public struct ServerProperties
    {
        public string MasterIp;
        public string SlaveIp;
        public string Type;
        public string Path;
    }

    public class XmlHandler
    {
        public ServerProperties DaProperties = new
            ServerProperties
        {
            MasterIp = "localhost",
            SlaveIp= "localhost",
            Type = "Elesy.DualSource",
            Path = "opcda://"
        };

        public ServerProperties HdaProperties = new
    ServerProperties
        {
            MasterIp = "localhost",
            SlaveIp = "localhost",
            Type = "EleSy.HDADualSource",
            Path = "opchda://"
        };

        public string IniPath = @"C:\Data\Reports\1042_02.ini";

        public XmlHandler(string path)
        {
            if (System.IO.File.Exists(path))
            {
                GetData(path);
            }
            else
            {
                CreateXml(path);
            }
        }

        void GetData(string path)
        {
            XDocument xdoc = XDocument.Load(path);

            foreach (XElement objectElement in xdoc.Element("settings").Elements("object"))
            {
                XElement elem = objectElement;
                string value = objectElement.FirstAttribute.Value;

                GetServer(value, elem);
                GetPath(value, elem);
            }
        }

        void GetServer(string value, XElement elem)
        {
            switch (value)
            {
                case "OPCDA":
                    DaProperties.MasterIp     = elem.Element("MasterIP").Value;
                    DaProperties.SlaveIp      = elem.Element("SlaveIP").Value;
                    DaProperties.Type         = elem.Element("OPCServer").Value;
                    break;
                case "OPCHDA":
                    HdaProperties.MasterIp    = elem.Element("MasterIP").Value;
                    HdaProperties.SlaveIp     = elem.Element("SlaveIP").Value;
                    HdaProperties.Type        = elem.Element("OPCServer").Value;
                    break;
                default: break;
            }
        }

        void GetPath(string value, XElement elem)
        {
            switch (elem.FirstAttribute.Name.LocalName)
            {
                case "path":
                    IniPath = value;
                    break;
                case "DApath":
                    DaProperties.Path = value;
                    break;
                case "HDApath":
                    HdaProperties.Path = value;
                    break;
                default: break;
            }
        }

        void CreateXml(string path)
        {
            XDocument xdoc = new XDocument(new XElement("settings",
                new XElement("object",
                    new XAttribute("server", "OPCDA"),
                    new XElement("MasterIP",    DaProperties.MasterIp),
                    new XElement("SlaveIP",     DaProperties.SlaveIp),
                    new XElement("OPCServer",   DaProperties.Type)),
                new XElement("object",
                    new XAttribute("server", "OPCHDA"),
                    new XElement("MasterIP",    DaProperties.MasterIp),
                    new XElement("SlaveIP",     DaProperties.SlaveIp),
                    new XElement("OPCServer",   DaProperties.Type)),
                new XElement("object",
                    new XAttribute("path", IniPath)),
                new XElement("object",
                    new XAttribute("DApath", DaProperties.Path)),
                new XElement("object",
                    new XAttribute("HDApath", DaProperties.Path))));
            xdoc.Save(path);
        }
    }

}
