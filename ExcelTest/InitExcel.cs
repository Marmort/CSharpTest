using System;
using System.Collections;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace ExcelTest
{
    class InitializeExcel
    {
        // 判断是否安装EXCEL
        public static void InstallExcel()
        {
            Application excelApp = new Application();
            if (excelApp == null)
            {
                Console.WriteLine("无法创建Excel对象，可能您的计算机未安装Excel！");
                return;
            }
            else
            {
                Console.WriteLine("您的计算机已经安装Excel！");
            }
        }

        public static void OpenExcel(string fileName)
        {
            Application excelApp = new Application();
            Workbook workBook;            

            try // 判断文件是否被其他进程使用
            {
                workBook = excelApp.Workbooks.Open(fileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
                Console.WriteLine("打开Excel文件！");
            }
            catch
            {
                Console.WriteLine("Excel文件处于打开状态，请保持关闭！");
                return;
            }

            // 获得所有表的名称
            int nCount = workBook.Worksheets.Count;
            string[] sheetSet = new string[nCount];
            ArrayList arrayList = new ArrayList();

            for (int i = 0; i < nCount; i++) //
            {
                sheetSet[i] = ((Worksheet)workBook.Worksheets[i + 1]).Name;
                Console.WriteLine(sheetSet[i]);
            }

            // 释放相关对象
            workBook.Close(null, null, null);
            excelApp.Quit();
            if (workBook != null)
            {
                Marshal.ReleaseComObject(workBook);
                workBook = null;
            }

            if (excelApp != null)
            {
                Marshal.ReleaseComObject(excelApp);
                excelApp = null;
            }

            GC.Collect();
        }

    }
}
