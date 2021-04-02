using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using EleSy.Common.Script;
using Reports.Infrostructure;
using Reports.Servers;
using Reports.FileHandler;
using Reports.Main;

namespace Scripts._1040_02AGT
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>
    class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 8;

            //Codes Статусов
            Dictionary<int, string> StatusDictionary = new Dictionary<int, string>();
                StatusDictionary.Add(1, "Отключен, нет напряжения");
                StatusDictionary.Add(2, "Отключен, цепи включения неисправны");
                StatusDictionary.Add(3, "Отключен");
                StatusDictionary.Add(4, "Отключается");
                StatusDictionary.Add(5, "Включается");
                StatusDictionary.Add(6, "Включен, пускатель неисправен");
                StatusDictionary.Add(7, "Включен, цепи отключения неисправны");
                StatusDictionary.Add(8, "Включен");

            List<int> FailList = new List<int>
            {
                1,2,1024,2048,4096,8192,16384,32768,4,8,16,32,64,128,256,512
            };

            string[] ColumnNames = new string[] { "Desc", "Status", "Fail" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths)
            {
                isTesting = false
            };

            if (initiator.FailedDaConnection || initiator.FailedHdaConnection)
                return new DataSet();

            CodesCreater prefix = new CodesCreater(initiator.opcda,"AGT", "codes.ELECTON.StopCause.bit");
            Dictionary<int, string> FailDictionary = prefix.CreateDictionary();

            Section section = initiator.Sections.Find(p => p.Name == "ECN");
            initiator.Sections = new List<Section> { section };

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            initiator.GetAllDataFromServer();

            Report88Device report = new Report88Device(initiator.Sections, ColumnNames,
                MaxTestingValue, initiator.isTesting,
                FailDictionary, StatusDictionary, FailList);

            initiator.opcda.TryToDisconnect();
            initiator.opchda.TryToDisconnect();

            return report.result;
        }
    }
}
