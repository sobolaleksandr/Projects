using EleSy.Common.Script;
using Reports.FileHandler;
using Reports.Infrostructure;
using Reports.Main;
using Reports.Servers;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Scripts._1040_02VLV
{
    /// <summary>
    /// Класс используется для подготовки набора данных.
    /// </summary>
    public class Script : IDataScript
    {
        public DataSet GetData(object[] parameters)
        {
            const bool FilteredPaths = false;
            const int MaxTestingValue = 7;

            Dictionary<int, string> FailDictionary = new Dictionary<int, string>
            {
              {0        , "В норме"                                },
              {524288   , "Несанкционированное движение"           },
              {131072   , "Сработал моментный выключатель"         },
              {1048576  , "Нарушение целостности цепей открытия"   },
              {25165824 , "Нет напряжения"                         },
              {65536    , "Нет готовности привода"                 },
              {4194304  , "Нет связи с БУ"                         },
              {134217728, "Неисправность модуля управления"        },
              {67108864 , "Неисправность модуля сигнализации"      },
              {2097152  , "Нарушение целостности цепей закрытия"   },
              {262144   , "Невыполнение команд управления"         }
            };

            Dictionary<int, string> StatusDictionary = new Dictionary<int, string>
            {
                {1, "Открыта"                   },
                {2, "Закрыта"                   },
                {3, "Промежуточное"             },
                {4, "Неопределенное состояние"  },
                {5, "В движении"                },
                {6, "Открывается"               },
                {7, "Закрывается"               }
            };

            List<int> FailList = new List<int>
            {
                0,524288,131072,1048576,25165824,65536,4194304,134217728,67108864,2097152,262144
            };

            string[] ColumnNames = new string[] { "Desc", "Status", "State" };

            ReportInitialize initiator = new ReportInitialize(ColumnNames, FilteredPaths);

            Section section = initiator.Sections.Find(p => p.Name == "VLV");
            initiator.Sections = new List<Section> { section };

            PathCreater append = new PathCreater(initiator.Sections);
            initiator.Sections = append.sections;

            foreach (Section sec in initiator.Sections)
            {
                foreach (Tag item in sec.Items)
                {
                    object[] DAvalue = initiator.opcda.GetData(item.Path);
                    item.DaData.Values = DAvalue;
                }
            }

            Report88Device report = new Report88Device(initiator.Sections, ColumnNames,
                MaxTestingValue, initiator.isTesting,
                FailDictionary, StatusDictionary, FailList);

            initiator.opcda.TryToDisconnect();
            initiator.opchda.TryToDisconnect();

            return report.result;
        }//968.88 Задвижки
    }
}
