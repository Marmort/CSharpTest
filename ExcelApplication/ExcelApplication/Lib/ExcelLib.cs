using System;

namespace ExcelLib
{
    class PreExcel
    {
        /// <param name="filePath">Excel文件路径</param>
        /// <returns></returns>
        public static IExcel GetExcel(string filePath)
        {
            if (filePath.Trim() == "")
                throw new Exception("文件名不能为空");

            if (!filePath.Trim().EndsWith("xls") && !filePath.Trim().EndsWith("xlsx"))
                throw new Exception("不支持该文件类型");

            if (filePath.Trim().EndsWith("xls"))
            {
                //IExcel res = new Excel03(filePath.Trim());
                //return res;
                throw new Exception("不支持Excel97~2003版本文件");
                //return null;
            }
            else if (filePath.Trim().EndsWith("xlsx"))
            {
                IExcel res = new Excel2007(filePath.Trim());
                return res;
            }
            else
                return null;
        }
    }
}
