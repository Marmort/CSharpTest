using System;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace ExcelClass
{
    class PreRequeriments : IDisposable
    {
        private string checkNet4;
        private Application checkExcel;
        private bool disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PreRequeriments()
        {
            checkNet4 = Environment.Version.ToString();
            checkExcel = new Application();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~PreRequeriments()
        {
            Dispose();
        }        

        /// <summary>
        ///  检查计算机是否安装了.NET Framework和Excel
        /// </summary>
        public void CheckPrerequeriments()
        {
            checkIfDisposed(); //检查对象是否已经释放

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
            }

            Dispose();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public  virtual void Dispose()
        {
            if (!this.disposed)
            {
                try
                {
                    if (checkExcel != null)
                    {
                        checkExcel.Quit();
                        Marshal.ReleaseComObject(checkExcel);
                        checkExcel = null;
                    }
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
