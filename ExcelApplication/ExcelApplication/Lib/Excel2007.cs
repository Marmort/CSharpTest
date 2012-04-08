using System;
using OfficeOpenXml;
using System.IO;
using System.Windows.Forms;

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
                        throw new Exception("工作表序号超出范围");
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
                throw new Exception("打开文件失败，详细信息：" + ex.Message);
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
                if (newSheetName =="")
                {
                    currentSheet = book.Worksheets.Add("WSheet");
                }
                else
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
                
                ep.Save();
                ifSave = true;
            }
            catch (Exception ex)
            {
                throw new Exception("保存文件失败，详细信息：" + ex.Message);
            }
           //return currentSheet.ToString();
           return true;
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
