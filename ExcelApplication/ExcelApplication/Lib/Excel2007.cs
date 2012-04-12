using System;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;

namespace ExcelLib
{
    class Excel2007 : IExcel
    {
        private string filePath = "";
        private ExcelPackage ep = null;
        private ExcelWorkbook book = null;
        private bool ifOpen = false;
        private ExcelWorksheet currentSheet = null;
        private int sheetCount = 0;
        private int currentSheetIndex = 0;
        private bool ifSave = false;

        public Excel2007()
        { }

        public Excel2007(string path)
        { filePath = path; }

        public ExcelVersion Version
        { get { return ExcelVersion.Excel2007; } }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public bool IfOpen
        { get { return ifOpen; } }

        public int SheetCount
        { get { return sheetCount; } }

        public int CurrentSheetIndex
        {
            get { return currentSheetIndex; }
            set
            {
                if (value != currentSheetIndex)
                {
                    if (value >= sheetCount)
                        throw new Exception("Beyond the scope of work table serial number!");
                    currentSheetIndex = value;
                    currentSheet = book.Worksheets[currentSheetIndex + 1];
                }
            }
        }

        public bool IfSave
        { get { return ifSave; } }

        public bool Open()
        {
            try
            {
                ep = new ExcelPackage(new FileInfo(filePath));

                if (ep == null) return false;
                book = ep.Workbook;
                sheetCount = book.Worksheets.Count;
                currentSheetIndex = 0;
                currentSheet = book.Worksheets[1];
                ifOpen = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Open file fail,information: " + ex.Message);
            }
            return true;
        }        

        public bool Save(string newSheetName, string[,] array)
        {
            try
            {
                ep = new ExcelPackage(new FileInfo(filePath));

                if (ep == null) return false;
                book = ep.Workbook;
                if (newSheetName != null)
                {
                    currentSheet = book.Worksheets.Add(newSheetName);
                }

                for (int i = 0; i < array.GetLongLength(0); i++)
                {
                    for (int j = 0; j < array.GetLongLength(1); j++)
                    {
                        currentSheet.Cells[i + 1, j + 1].Value = array[i, j];
                    }
                }
                using (var range = currentSheet.Cells[1, 1, 1, (int)array.GetLongLength(1)])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

                ep.Save();
                ifSave = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Save file fail,information: " + ex.Message);
            }
           return true;
        }

        public string[] GetWorkSheets()
        {
            string[] sheets = new string[book.Worksheets.Count];
            try
            {
                for (int i = 0; i < book.Worksheets.Count; i++)
                {
                    sheets[i] = book.Worksheets[i + 1].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Read Sheets fail,information: " + ex.Message);
            }
            return sheets;
        }

        public int GetRowCount()
        {
            if (currentSheet == null) return 0;
            return currentSheet.Dimension.End.Row;
        }

        public int GetColumnCount()
        {
            if (currentSheet == null) return 0;
            return currentSheet.Dimension.End.Column;
        }

        public int GetCellCountInRow(int Row)
        {
            if (currentSheet == null) return 0;
            if (Row >= currentSheet.Dimension.End.Row) return 0;
            return currentSheet.Dimension.End.Column;
        }

        public string GetCellValue(int Row, int Col)
        {
            if (currentSheet == null) return "";
            if (Row > currentSheet.Dimension.End.Row || Col > currentSheet.Dimension.End.Column) return "";
            object tmpO = currentSheet.GetValue(Row, Col);
            if (tmpO == null) return "";
            return tmpO.ToString();
        }

        public void Close()
        {
            if (!ifOpen || !ifSave || ep == null) return;
            ep.Dispose();
        }

    }
}
