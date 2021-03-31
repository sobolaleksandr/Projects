using Reports;
using System;
using System.Data;

namespace Application
{
    public class App
    { 
        static void MakeFile()
        {
            //var sk = new Script986();
            var sk = new Scripts._1042_02.Script();

            //string OutputPath = @"С:\Output.docx";
            //string StartingString = "";
            object[] obj = new object[3];

            DataSet ds = new DataSet();
            //try
            //{
            //    ds = sk.GetData(ref StartingString);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //ds = sk.GetData986_99(obj);
            ds = sk.GetData(obj);

            int rowcount = ds.Tables[0].Rows.Count;
            //int columncount = ds.Tables[0].Columns.Count;
            //new Reports.OutputFile().CreatePackage(OutputPath, rowcount, columncount, ds, StartingString);

            //Console.WriteLine("Конец работы! Можете найти файл отчета " + OutputPath);
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            MakeFile();
        }
        
    }
    
    
    
}
