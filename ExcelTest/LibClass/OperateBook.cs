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
    partial class OperateExcel : IDisposable
    {
        public string fileName;
        public Application application;
        public Workbooks workBooks;
        public Workbook workBook;
        public Worksheet workSheet;
        private bool disposed = false;

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

        /// <summary>
        /// 析构函数
        /// </summary>
        ~OperateExcel()
        {
            Dispose();
        }        
        
        #region OprateFile

        /// <summary>
        /// 创建工作簿
       /// </summary>
       /// <param name="fileName"></param>
        public void Create(string fileName)
        {
            checkIfDisposed();
            if(string.IsNullOrEmpty(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel工作簿名称不能为空！");
                Console.ResetColor();
            }
            else if (File.Exists(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(fileName + " 工作簿已经存在！");
                Console.ResetColor();
            }
            else
            {
                SaveAs(fileName);
            }
        }

        /// <summary>
        /// 复制工作簿
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        public void Copy(string oldFileName,string newFileName)
        {
            checkIfDisposed();
            if (File.Exists(oldFileName))
            {
                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }
                File.Copy(oldFileName, newFileName);                    
            }            
        }

        /// <summary>
        /// 重命名工作簿
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        public void ReName(string oldFileName, string newFileName)
        {
            checkIfDisposed();
            if (File.Exists(oldFileName))
            {
                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }
                File.Move(oldFileName, newFileName);
            }
        }

        /// <summary>
        /// 打开工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(string fileName)
        {
            checkIfDisposed();
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
            }
        }

        /// <summary>
        /// 保存工作簿
        /// </summary>
        public void Save()
        {
            checkIfDisposed();
            if(workBook!=null)
                workBook.Save();
        }

        /// <summary>
        /// 另存为工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName)
        {
            checkIfDisposed();
            application.Visible = false;
            workBook = workBooks.Add(true);
            workBook.SaveAs(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        /// <summary>
        /// 删除工作簿
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            checkIfDisposed();
            if (string.IsNullOrEmpty(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Excel工作簿名称不能为空！");
                Console.ResetColor();
            }
            else if (!File.Exists(fileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(fileName + "工作簿不存在！");
                Console.ResetColor();
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None);
                    fs.Close();
                    File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 关闭/销毁一个Excel对象
        /// </summary>
        public void Close()
        {
            checkIfDisposed();
            Save();
            Dispose();
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
        /// <param name="oldSheetName"></param>
        /// <param name="newSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(string oldSheetName, string newSheetName)
        {
            workSheet = (Worksheet)workBook.Worksheets[oldSheetName];
            workSheet.Name = newSheetName;
            return workSheet;
        }

        /// <summary>
        /// 重命名一个工作表
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="newSheetName"></param>
        /// <returns></returns>
        public Worksheet ReNameSheet(Worksheet sheet, string newSheetName)
        {
            sheet.Name = newSheetName;
            return sheet;
        }

        #endregion
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (!disposed)
            {
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
                finally
                {
                    this.disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// 如果已释放抛出异常
        /// </summary>
        private void checkIfDisposed()
        {
            if (this.disposed)
                throw new ObjectDisposedException("注意：对象已经释放");
        }

    }
}
