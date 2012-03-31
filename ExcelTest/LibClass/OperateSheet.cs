using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ExcelClass
{
    partial class OperateExcel : IDisposable
    {        
        /// <summary>
        /// 设置某单元格的值
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="x">X行</param>
        /// <param name="y">Y列</param>
        /// <param name="value">值</param>
        public void SetCellValue(Worksheet workSheet, int x, int y, object value)
        {
            if (workSheet != null)
                workSheet.Cells[x, y] = value;
        }

        /// <summary>
        /// 设置某单元格的值
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="x">X行</param>
        /// <param name="y">Y列</param>
        /// <param name="value">值</param>
        public void SetCellValue(string workSheet, int x, int y, object value)
        {
            if(!string.IsNullOrEmpty(workSheet))
                GetSheet(workSheet).Cells[x, y] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        //public void UniteCells(Worksheet workSheet, int x1, int y1, int x2, int y2)//合并单元格
        //{
        //    Console.WriteLine("sss");
        //    if (workSheet != null)            
        //        workSheet.get_Range(workSheet.Cells[x1, y1], workSheet.Cells[x2, y2]).Merge(true);
        //}

        /////// <summary>
        ///// 
        ///// </summary>
        ///// <param name="workSheet"></param>
        ///// <param name="x1"></param>
        ///// <param name="y1"></param>
        ///// <param name="x2"></param>
        ///// <param name="y2"></param>
        //public void UniteCells(string workSheet, int x1, int y1, int x2, int y2)//合并单元格
        //{
        //    Worksheet ws = GetSheet(workSheet);
        //    ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
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
        //    OperateBook ob = new OperateBook();
        //    Worksheet workSheet = ob.GetSheet(wsn);
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Name = name;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Size = size;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).Font.Color = color;
        //    workSheet.get_Range(workSheet.Cells[Startx, Starty], workSheet.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
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
    }
}
