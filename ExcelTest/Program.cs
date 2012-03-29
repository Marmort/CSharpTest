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
            InitializeExcel.InstallExcel();            
            string names = programPath + "\\aaa.xlsx";
            Console.WriteLine(names);
            InitializeExcel.OpenExcel(names);
            Console.ReadKey();
        }
    }
}
