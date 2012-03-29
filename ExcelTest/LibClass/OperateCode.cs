using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace ExcelClass
{
    class OperateCode
    {
        public string fileName;
        public Application application;
        public Workbooks workBooks;
        public Workbook workBook;
        public Worksheet workSheet;

        public OperateCode()
        {
            // TODO: 在此处添加构造函数逻辑            
        }

        /// <summary>
        /// 创建一个Excel对象
        /// </summary>
        public void Create()
        {
            application = new Application();
            workBooks = application.Workbooks;
            workBook = workBooks.Add(true);
        }

        /// <summary>
        /// 打开一个Excel文件
        /// </summary>
        /// <param name="fileName">文件路径和名称</param>
        public void Open(string fileName)
        {
            application = new Application();
            workBooks = application.Workbooks;

             // 判断文件是否被其他进程使用
            try 
            {                
                workBook = workBooks.Open(fileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("打开Excel文件！" + workBook.Name);
                Console.ResetColor();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel文件处于打开状态，请保持关闭！");
                Console.ResetColor();
                Console.ReadKey();
                Environment.Exit(0);
            }
            this.fileName = fileName;
        }
        
        /// <summary>
        /// 获得所有表的名称
        /// </summary>
        public void GetSheetsName()
        {
            int nCount = workBook.Worksheets.Count;
            string[] sheetNames = new string[nCount];
           
            for (int i = 0; i < nCount; i++) 
            {
                sheetNames[i] = ((Worksheet)workBook.Worksheets[i + 1]).Name;
                Console.WriteLine(sheetNames[i]);
            }
        }

        //public Worksheet GetSheet(string SheetName)//获取一个工作表
        //{
        //    workSheet = (Worksheet)workBook.Worksheets[SheetName];
        //    return workSheet;
        //}

        /// <summary>
        /// 添加一个工作表
        /// </summary>
        /// <param name="SheetName">表名称</param>
        /// <returns></returns>
        public Worksheet AddSheet(string SheetName)
        {
            workSheet = (Worksheet)workBook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            workSheet.Name = SheetName;
            return workSheet;
        }

        //public void DelSheet(string SheetName)//删除一个工作表
        //{
        //    ((Worksheet)workBook.Worksheets[SheetName]).Delete();
        //}

        //public Worksheet ReNameSheet(string OldSheetName, string NewSheetName)//重命名一个工作表一
        //{
        //    workSheet = (Worksheet)workBook.Worksheets[OldSheetName];
        //    workSheet.Name = NewSheetName;
        //    return workSheet;
        //}

        //public Worksheet ReNameSheet(Worksheet Sheet, string NewSheetName)//重命名一个工作表二
        //{
        //    Sheet.Name = NewSheetName;
        //    return Sheet;
        //}

        //public void SetCellValue(Worksheet workSheet, int x, int y, object value)//workSheet：要设值的工作表     X行Y列     value   值
        //{
        //    workSheet.Cells[x, y] = value;
        //}

        //public void SetCellValue(string workSheet, int x, int y, object value)//workSheet：要设值的工作表的名称 X行Y列 value 值
        //{
        //    GetSheet(workSheet).Cells[x, y] = value;
        //}

        //public void SetCellProperty(Worksheet workSheet, int Startx, int Starty, int Endx, int Endy, int size, string name, Constants color, Excel.Constants HorizontalAlignment)//设置一个单元格的属性   字体，   大小，颜色   ，对齐方式
        //{
        //    name = "宋体";
        //    size = 12;
        //    color = Constants.xlAutomatic;
        //    HorizontalAlignment = Constants.xlRight;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Name = name;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Size = size;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Color = color;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        //}

        //public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Constants color, Constants HorizontalAlignment)
        //{
        //    //name = "宋体";
        //    //size = 12;
        //    //color = Constants.xlAutomatic;
        //    //HorizontalAlignment = Constants.xlRight;

        //    Worksheet workSheet = GetSheet(wsn);
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Name = name;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Size = size;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Color = color;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        //}

        //public void UniteCells(Worksheet workSheet, int x1, int y1, int x2, int y2)//合并单元格
        //{
        //    workSheet.get_Range(workSheet.Cells[x1, y1], workSheet.Cells[x2, y2]).Merge(Type.Missing);
        //}

        //public void UniteCells(string workSheet, int x1, int y1, int x2, int y2)//合并单元格
        //{
        //    GetSheet(workSheet).get_Range(GetSheet(workSheet).Cells[x1, y1], GetSheet(workSheet).Cells[x2, y2]).Merge(Type.Missing);
        //}

        //public void InsertTable(System.Data.DataTable dt, string workSheet, int startX, int startY)//将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时使用一
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        for (int j = 0; j <= dt.Columns.Count - 1; j++)
        //        {
        //            GetSheet(workSheet).Cells[startX + i, j + startY] = dt.Rows[i][j].ToString();
        //        }
        //    }
        //}

        //public void InsertTable(System.Data.DataTable dt, Worksheet workSheet, int startX, int startY)//将内存中数据表格插入到Excel指定工作表的指定位置二
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        for (int j = 0; j <= dt.Columns.Count - 1; j++)
        //        {
        //            workSheet.Cells[startX + i, j + startY] = dt.Rows[i][j];
        //        }
        //    }
        //}

        //public void AddTable(System.Data.DataTable dt, string workSheet, int startX, int startY)//将内存中数据表格添加到Excel指定工作表的指定位置一
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        for (int j = 0; j <= dt.Columns.Count - 1; j++)
        //        {
        //            GetSheet(workSheet).Cells[i + startX, j + startY] = dt.Rows[i][j];
        //        }
        //    }
        //}

        //public void AddTable(System.Data.DataTable dt, Worksheet workSheet, int startX, int startY)//将内存中数据表格添加到Excel指定工作表的指定位置二
        //{
        //    for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        for (int j = 0; j <= dt.Columns.Count - 1; j++)
        //        {
        //            workSheet.Cells[i + startX, j + startY] = dt.Rows[i][j];
        //        }
        //    }
        //}

        //public void InsertPictures(string Filename, string workSheet)//插入图片操作一
        //{
        //    GetSheet(workSheet).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);//后面的数字表示位置
        //}

        ////public void InsertPictures(string Filename, string workSheet, int Height, int Width)//插入图片操作二
        ////{
        ////    GetSheet(workSheet).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).Height = Height;
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).Width = Width;
        ////}

        ////public void InsertPictures(string Filename, string workSheet, int left, int top, int Height, int Width)//插入图片操作三
        ////{
        ////    GetSheet(workSheet).Shapes.AddPicture(Filename, MsoTriState.msoFalse, MsoTriState.msoTrue, 10, 10, 150, 150);
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).IncrementLeft(left);
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).IncrementTop(top);
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).Height = Height;
        ////    GetSheet(workSheet).Shapes.get_Range(Type.Missing).Width = Width;
        ////}

        //public void InsertActiveChart(XlChartType ChartType, string workSheet, int DataSourcesX1, int DataSourcesY1, int DataSourcesX2, int DataSourcesY2, XlRowCol ChartDataType)//插入图表操作
        //{
        //    ChartDataType = XlRowCol.xlColumns;
        //    workBook.Charts.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    {
        //        workBook.ActiveChart.ChartType = ChartType;
        //        workBook.ActiveChart.SetSourceData(GetSheet(workSheet).get_Range(GetSheet(workSheet).Cells[DataSourcesX1, DataSourcesY1], GetSheet(workSheet).Cells[DataSourcesX2, DataSourcesY2]), ChartDataType);
        //        workBook.ActiveChart.Location(XlChartLocation.xlLocationAsObject, workSheet);
        //    }
        //}

        //public bool Save()//保存文档
        //{
        //    if (fileName == "")
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            workBook.Save();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public bool SaveAs(object FileName)//文档另存为
        //{
        //    try
        //    {
        //        workBook.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// 关闭一个Excel对象，销毁对象
        /// </summary>
        public void Close()
        {
            //workBook.Save();
            workBook.Close(Type.Missing, Type.Missing, Type.Missing);
            workBooks.Close();
            application.Quit();
            workBook = null;
            workBooks = null;
            application = null;
            GC.Collect();
        }
    }
}
