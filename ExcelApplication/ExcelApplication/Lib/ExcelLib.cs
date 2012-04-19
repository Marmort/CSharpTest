using System;

namespace ExcelLib
{
    class PreExcel
    {
        /// <summary>
        /// 文件类型检查
        /// </summary>
        /// <param name="filePath">路径和文件名</param>
        /// <returns></returns>
        public static IExcel GetExcel(string filePath)
        {
            try
            {
                if (filePath.Trim() != "")
                {
                    IExcel res = null;
                    if (filePath.Trim().EndsWith("xlsx"))
                        res = new Excel2007(filePath.Trim());
                    //else if (filePath.Trim().EndsWith("xls"))
                    //    res = new Excel2003(filePath.Trim());
                    return res;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
