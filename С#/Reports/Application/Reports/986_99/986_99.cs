using Reports.FileHandler;
using Reports.Infrostructure;
using System;
using System.Collections.Generic;
using System.Data;
using Reports.Servers;
using System.Text.RegularExpressions;
using EleSy.Common.Script;
using System.Xml.Linq;
using Reports;
using Reports.Main;
using Reports.Servers.Services;
using System.Linq;
using System.Text;


namespace Scripts._986_99
{
    public class Script
    {
        public DataSet GetData(object[] parameters)
        {
            DAServer opcda;
            HDAServer opchda;
            List<Section> Sections;
            bool isTesting = true;
            XmlHandler settings;

            string[] Splitter = new string[] { "Desc", "Status", "Mode", "MskSet", "Val" };
            string[] ColumnNames = new string[]
            {
                "наряд", "емкость источник битума",
                "емкость источник пластификатора",
                "емкость дозатор полимера",
                "Температура битума в емкости источнике",
                "Температура в резервуаре источнике на начало операции",
                "Теплообменник",  "Температура битума на входе в теплообменник",
                "Температура битума на выходе из теплообменника",
                "Температура пластификатора в емкости источнике в момент остановки перекачки",
                "Скорость подачи битума, уставка" ,"Скорость подачи полимера, уставка",
                "Скорость подачи пластификатора, уставка", "Реактор приемник 1",  "Время начала загрузки битума",
                "Время окончания загрузки битума", "Время начала загрузки полимера",  "Время окончания загрузки полимера",
                "Время начала загрузки пластификатора",   "Время окончания загрузки пластификатора", "реактор-приемник 2",
                "Температура перекачки",   "Время начала перекачки в реактор-приемник 2",
                "Время окончания загрузки битума и всех компонентов в реактор приемник 2",
                "Температура начала перекачки",    "Время начала перекачки",
                "Время окончания перекачки" ,  "емкость-приемник" ,   "Время начала перекачки в емкость-приемник" ,
                "Время окончания перекачки в емкость-приемник"  ,  "температура начала перекачки в емкость-приемник" ,
                "температура конца перекачки в емкость-приемник"  ,"Данные расходомера по битуму",
                "Данные расходомера по пластификатору",   "Данные по количеству полимера",
                "Данные по количеству адгезионной присадки ДЕ"
            };

            bool FilteredPaths = false;
            const int MaxTestingValue = 100;

            const string xPath = @"C:\settings.xml";
            settings = new XmlHandler(xPath);

            IniHandler inidata = new IniHandler(@"C:\Data\Reports\ReportSetting.ini", Splitter, FilteredPaths);
            Sections = inidata.Parse();

            PathCreater append = new PathCreater(Sections);
            Sections = append.sections;

            //opcda = new DAServer(settings.DAMasterIP, settings.DASlaveIP, settings.DAServer, settings.DApath);
            ServerProperties DaProperties = new  ServerProperties
            {
                MasterIp = "10.13.1.42",
                SlaveIp = "localhost",
                Type = "Infinity.OPCServer",
                Path = "opcda://"
            };
            opcda = new DAServer(DaProperties);

            opcda.TryToConnect();

            //opchda = new HDAServer(settings.HDAMasterIP, settings.HDASlaveIP, settings.HDAServer, settings.HDApath);
            //opchda.Connect();

            Report986_99 report = new Report986_99(Sections, ColumnNames, MaxTestingValue, isTesting, opcda);

            return report.result;
        }
    }
    public class Report986_99 : ReportBase
    {
        Dictionary<string, string> EDsOutBit = new Dictionary<string, string>
        {
            ["E-1"] = "",
            ["E-2"] = "",
            ["E-3"] = "",
            ["E-4"] = "",
            ["E-5"] = "",
            ["E-6"] = "",
            ["E-7"] = "",
            ["E-8"] = ""
        };

        Dictionary<string, string> EDsOutPlast = new Dictionary<string, string>
        {
            ["ПЕ-1"] = "",
            ["ПE-2"] = "",
            ["ПE-5"] = "",
            ["ПE-6"] = ""
        };

        Dictionary<string, string> EDsDozPoli = new Dictionary<string, string>
        {
            ["Б-1"] = "",
            ["Б-2"] = ""
        };


        public void BusinessLogic()
        {
            row = table.NewRow();//row[37]
            FillEmptySpaces();

            Section section = sections.FirstOrDefault(p => p.Name == "Elem_TR_CHOISE");
            Tag item = section.GetItem("Val");

            row[0] = GetId();//todo

            GetBitSource(item);//1,6

            row[2] = GetPlastSource(item);
            row[3] = GetDozSource(item);//todo
            row[4] = GetTempBitSource(item);//todo
            row[5] = GetTempPlastSource(item);//todo

            section = sections.FirstOrDefault(p => p.Name == "AP4");
            item = section.GetItem("Val");

            row[7] = GetTempBitInTO(item);
            row[8] = GetTempBitOutTO(item);

            row[9] = GetId();//todo

            section = sections.FirstOrDefault(p => p.Name == "Картауставок");
            item = section.GetItem("Val");
            
            row[10] = GetSpeedBit(item);

            row[11] = GetId();//todo
            row[12] = GetId();//todo

            row[13] = "М-5";
            if (row[3].ToString() == "Б-1")
            {
                row[13] = "М-6";
            }

            row[14] = DateTime.Now;//todo
            row[15] = DateTime.Now;//todo
            row[16] = DateTime.Now;//todo
            row[17] = DateTime.Now;//todo
            row[18] = DateTime.Now;//todo
            row[19] = DateTime.Now;//todo

            row[20] = "М-8";

            row[21] = GetId();//todo

            row[22] = DateTime.Now;//todo
            row[23] = DateTime.Now;//todo

            row[24] = GetId();//todo

            row[25] = DateTime.Now;//todo
            row[26] = DateTime.Now;//todo

            row[27] = "М-6";

            row[28] = DateTime.Now;//todo
            row[29] = DateTime.Now;//todo

            row[30] = GetId();//todo
            row[31] = GetId();//todo
            row[32] = GetId();//todo
            row[33] = GetId();//todo
            row[34] = GetId();//todo
            row[35] = GetId();//todo

            table.Rows.Add(row);
        }

        Random rand = new Random(System.Convert.ToInt32(DateTime.Now.Millisecond));

        string GetId()
        {
            return rand.Next(MaxTestingValue).ToString();
        }

        void GetBitSource(Tag item)
        {
            int index = item.Path.IndexOf("RyazanPBM.Algoritms.PREP.PBV.Item1");
            int value = Convert.ToInt32(item.DaData.Values[index]);

            if (value > 4)
            {
                row[6] = "T1";
            }
            else
            {
                row[6] = "T2";
            }

            switch (value)
            {
                case 1:
                case 5:
                    row[1] = "Е-1";
                    return;

                case 2:
                case 6:
                    row[1] = "Е-3";
                    return;

                case 3:
                case 7:
                    row[1] = "Е-6";
                    return;

                case 4:
                case 8:
                    row[1] = "Е-7";
                    return;
            }

        }
        string GetPlastSource(Tag item)
        {
            int index = item.Path.IndexOf("RyazanPBM.Algoritms.PREP.PBV.Item5");
            int value = Convert.ToInt32(item.DaData.Values[index]);

            if (value == 1)
            {
                return "ПЕ-5";
            }
            else
            {
                return "ПЕ-6";
            }
        }
        string GetDozSource(Tag item)
        {
            //int index = item.Path.IndexOf("Algoritms.PREP.PBV.Item13");
            //int value = Convert.ToInt32(item.DaData.Values[index]);

            //switch (value)
            //{
            //    case 1:
            //        return "Е-2";
            //    case 2:
            //        return "Е-4";
            //    case 3:
            //        return "Е-5";
            //    case 4:
            //        return "Е-6";
            //    case 5:
            //        return "Е-7";
            //    case 6:
            //        return "Е-8";
            //}
            //return "";
            return rand.Next(MaxTestingValue).ToString();
        }
        string GetTempBitSource(Tag item)
        {
            //string tag = "";
            //string temp = row[1].ToString();
            //switch (temp)
            //{
            //    case "Е-1":
            //        tag = "RyazanPBM.Ind_of_Real.Real1";
            //        break;
            //    case "Е-3":
            //        tag = "RyazanPBM.Ind_of_Real.Real5";
            //        break;
            //    case "Е-6":
            //        tag = "RyazanPBM.Ind_of_Real.Real11";
            //        break;
            //    case "Е-7":
            //        tag = "RyazanPBM.Ind_of_Real.Real13";
            //        break;
            //}

            //int index = item.Path.IndexOf(tag);
            //return item.DaData.Values[index].ToString();
            return rand.Next(MaxTestingValue).ToString();
        }
        string GetTempPlastSource(Tag item)
        {
            //string tag = "";
            //switch (row[2])
            //{
            //    case "ПE-5":
            //        tag = "RyazanPBM.Ind_of_Real.Real31";
            //        break;
            //    case "ПE-6":
            //        tag = "RyazanPBM.Ind_of_Real.Real33";
            //        break;
            //}

            //int index = item.Path.IndexOf(tag);
            //return item.DaData.Values[index].ToString();
            return rand.Next(MaxTestingValue).ToString();
        }
        string GetTempBitInTO(Tag item)
        {
            if (row[6].ToString() == "Т1")
            {
                int index = item.Path.IndexOf("RyazanPBM.PromCeh.Tepl_T1.TT100");
                return item.DaData.Values[index].ToString();
            }
            else
            {
                int index = item.Path.IndexOf("RyazanPBM.PromCeh.Tepl_T2.TT102");
                return item.DaData.Values[index].ToString();
            }
        }
        string GetTempBitOutTO(Tag item)
        {
            if (row[6].ToString() == "Т1")
            {
                int index = item.Path.IndexOf("RyazanPBM.PromCeh.Tepl_T1.TT101");
                return item.DaData.Values[index].ToString();
            }
            else
            {
                int index = item.Path.IndexOf("RyazanPBM.PromCeh.Tepl_T2.TT103");
                return item.DaData.Values[index].ToString();
            }
        }
        string GetSpeedBit(Tag item)
        {
            //int index = item.Path.IndexOf("RyazanPBM.Algoritms.PREP.PBV.SET.xa.set01");
            int index = item.Path.IndexOf("RyazanPBM.Algoritms.PREP.PBV.SET");
            return item.DaData.Values[index].ToString();
        }

        public Report986_99(List<Section> sections, string[] ColumnNames,
            int MaxTestingValue, bool isTesting, DAServer opcda) :
        base(sections, ColumnNames, MaxTestingValue)
        {
            foreach (var section in sections)
            {
                foreach (var item in section.Items)
                {
                    item.DaData.Values = opcda.GetData(item.Path);

                    if (isTesting && item.Name != "Desc")
                        item.DaData.RandomInt(8);
                }
            }

            BusinessLogic();
        }
    }

}
