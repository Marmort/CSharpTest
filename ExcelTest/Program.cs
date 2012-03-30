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
            ExcelClass.OperateExcel os = new ExcelClass.OperateExcel();
            Worksheet sp = new Worksheet();
            string names = programPath + "\\aaa.xlsx";

            sp = os.GetSheet("sheet008");
            os.UniteCells(sp, 1, 1, 1, 2);
            os.Close();
            Console.ReadKey();
        }
    }
}
