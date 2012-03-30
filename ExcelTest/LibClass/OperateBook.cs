using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ExcelClass
{
    partial class OperateExcel
    {
        public string fileName;
        public Application application = null;
        public Workbooks workBooks = null;
        public Workbook workBook = null;
        public Worksheet workSheet = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OperateExcel()
        {
            if (application == null)
            {
                application = new Application();
                workBooks = application.Workbooks;
            }
        }
        
        #region OprateFile

        /// <summary>
        /// 创建fileName工作簿
       /// </summary>
       /// <param name="fileName"></param>
        public void Create(string fileName)
        {    
            if(string.IsNullOrEmpty(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel工作簿名称不能为空！");
                Console.ResetColor();
                Close();
            }
            else if (File.Exists(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(fileName + "工作簿已经存在！");
                Console.ResetColor();
                Close();
            }
            else
            {
                SaveAs(fileName);
                Close();
            }
        }

        /// <summary>
        /// 另存为fileName工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(object fileName)
        {
            try
            {
                application.Visible = false;
                workBook = workBooks.Add(true);
                workBook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Close();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }

        /// <summary>
        /// 删除fileName工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {           
            if (string.IsNullOrEmpty(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel工作簿名称不能为空！");
                Console.ResetColor();
                Close();
            }
           else if (!File.Exists(fileName))
           {
               Console.ForegroundColor = ConsoleColor.Red;
               Console.WriteLine(fileName + "工作簿不存在！");
               Console.ResetColor();
               Close();
           }
           else
           {
               try
               {
                   FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                   fs.Close();
                   File.Delete(fileName);
                   Close();
               }
               catch (Exception ex)
               {
                   Debug.Write(ex.ToString());
               }
           }
        }

        /// <summary>
        /// 打开fileName工作簿
        /// </summary>
        /// <param name="fileName">工作簿路径和名称</param>
        public void Open(string fileName)
        {
             // 判断文件是否被其他进程使用
            try 
            {                
                workBook = workBooks.Open(fileName, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("打开Excel工作簿！" + workBook.Name);
                Console.ResetColor();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel文件处于打开状态，请保持关闭！");
                Console.ResetColor();
                Close();
            }
            this.fileName = fileName;
        }

        /// <summary>
        /// 保存fileName工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {                
                try
                {
                    workBook.Save();
                    //Close();
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }
            }
        }

        #endregion

        #region OperateSheets

        /// <summary>
        /// 获得所有表的名称
        /// </summary>
        public void GetAllSheets()
        {
            int nCount = workBook.Worksheets.Count;
            string[] sheetNames = new string[nCount];
           
            for (int i = 0; i < nCount; i++) 
            {
                sheetNames[i] = ((Worksheet)workBook.Worksheets[i + 1]).Name;
                Console.WriteLine(sheetNames[i]);
            }
        }

        /// <summary>
        /// 获取一个指定工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public Worksheet GetSheet(string sheetName)
        {
            if (workSheet != null)
            {
                workSheet = (Worksheet)workBook.Worksheets[sheetName];
                return workSheet;
            }
            return null;
        }

        /// <summary>
        /// 添加一个工作表
        /// </summary>
        /// <param name="sheetName">表名称</param>
        /// <returns></returns>
        public Worksheet AddSheet(string sheetName)
        {
            if (!string.IsNullOrEmpty(sheetName))
            {
                if (workSheet != null)
                {
                    workSheet = (Worksheet)workBook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    workSheet.Name = sheetName;
                    return workSheet;
                }
            }
            return null;           
        }

        /// <summary>
        /// 删除一个工作表
        /// </summary>
        /// <param name="sheetName"></param>
        public void DelSheet(string sheetName)
        {
            ((Worksheet)workBook.Worksheets[sheetName]).Delete();
        }

        /// <summary>
        /// 重命名一个工作表
        /// </summary>
        /// <param name="OldSheetName"></param>
        /// <param name="NewSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(string OldSheetName, string NewSheetName)
        {
            workSheet = (Worksheet)workBook.Worksheets[OldSheetName];
            workSheet.Name = NewSheetName;
            return workSheet;
        }

        /// <summary>
        /// 重命名一个工作表
        /// </summary>
        /// <param name="Sheet"></param>
        /// <param name="NewSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(Worksheet Sheet, string NewSheetName)
        {
            Sheet.Name = NewSheetName;
            return Sheet;
        }

        #endregion

        /// <summary>
        /// 关闭一个Excel对象，销毁对象
        /// </summary>
        public void Close()
        {
            Save(fileName);
            //Clipboard.Clear();
            try
            {
                if (workBook != null)
                {
                    workBook.Close(Type.Missing, Type.Missing, Type.Missing);
                    Marshal.ReleaseComObject(workBook);
                    workBook = null;
                }
                if (workBooks != null)
                {
                    workBooks.Close();
                    Marshal.ReleaseComObject(workBooks);
                    workBooks = null;
                }
                if (application != null)
                {                    
                    application.Quit();
                    Marshal.ReleaseComObject(application);
                    application = null;
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
    }
}
