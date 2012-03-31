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
            ExcelClass.PreRequeriments pr = new ExcelClass.PreRequeriments();
            pr.CheckPrerequeriments();

            ExcelClass.OperateExcel op = new ExcelClass.OperateExcel();
            string pf1= programPath + "\\aaa.xlsx";
            string pf2 = programPath + "\\bb.xlsx";
            //op.Create(pf1);
            //op.Open(pf1);
            op.Copy(pf1, pf2);
            Console.ReadKey();
            op.Close();
            Console.ReadKey();
        }
    }
}
