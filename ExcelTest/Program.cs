using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ExcelTest
{
    class Program
    {
        // 全局变量
        public static String programPath = System.IO.Directory.GetCurrentDirectory();

        static void Main(string[] args)
        {
            ExcelClass.PreRequeriments.CheckPrerequeriments();
            ExcelClass.OperateCode op = new ExcelClass.OperateCode();                 
            string names = programPath + "\\aaa.xlsx";

            //op.Create(); 
            op.Open(names);
            op.GetSheetsName();
            op.AddSheet("names");
            op.GetSheetsName();
            op.Close();
            Console.ReadKey();
        }
    }
}
