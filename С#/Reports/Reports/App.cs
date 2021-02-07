using System;
using System.Data;


namespace Scripts
{
    public class App
    { 
        static void MakeFile()
        {
            var sk = new Reports.Script();

            //string OutputPath = @"С:\Output.docx";
            //string StartingString = "";

            object[] obj = new object[3];
            DataSet ds = new DataSet();

            //Часть кода для отладки на объекте

            //try
            //{
            //    ds = sk.GetData(ref StartingString);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            ds = sk.GetData88VLV(obj);// вызваем функцию формирования отчета

            //Можем сформировать отчет в формате xdocument

            //int rowcount = ds.Tables[0].Rows.Count;
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
