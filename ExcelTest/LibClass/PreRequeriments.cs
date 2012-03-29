using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ExcelClass
{
    class PreRequeriments
    {
        public PreRequeriments()
        {
            // TODO: 在此处添加构造函数逻辑            
        }

        /// <summary>
        ///  检查计算机是否安装了.NET Framework和Excel
        /// </summary>
        public static void CheckPrerequeriments()
        {
            String checkNet4 = Environment.Version.ToString();
            Application checkExcel = new Application();

            if (checkNet4.StartsWith("4"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("发现：.NET Framework " + checkNet4);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("请更新：.NET Framework to" + " version 4");
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);
            }
           
            if (checkExcel != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("发现：Micrsoft.Office.Excel " + checkExcel.Version);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("请更新：Micrsoft.Office.Excel " + "2003/2007");
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
