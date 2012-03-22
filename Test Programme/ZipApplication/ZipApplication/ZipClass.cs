using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;

namespace ZipApplication
{
    /// <summary>
    /// 
    /// </summary>
    class ZipClass
    {
        #region ZipFileDictory  

        /// <summary>  
        /// 递归压缩文件夹方法  
        /// </summary>  
        /// <param name="FolderToZip"></param>  
        /// <param name="s"></param>  
        /// <param name="ParentFolderName"></param>  
        private  bool ZipFileDictory(string FolderToZip, ZipOutputStream s, string ParentFolderName)  
        {  
            bool res = true;  
            string[] folders, filenames;  
            ZipEntry entry = null;  
            FileStream fs = null;  
            Crc32 crc = new Crc32();  
            try  
            {  
                //创建当前文件夹  
                entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/"));  //加上 “/” 才会当成是文件夹创建  
                s.PutNextEntry(entry);  
                s.Flush();  
                //先压缩文件，再递归压缩文件夹   
                filenames = Directory.GetFiles(FolderToZip);  
                foreach (string file in filenames)  
                {  
                    //打开压缩文件  
                    fs = File.OpenRead(file);  

                    byte[] buffer = new byte[fs.Length];  
                    fs.Read(buffer, 0, buffer.Length);  
                    entry = new ZipEntry(Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip) + "/" + Path.GetFileName(file)));  
                    entry.DateTime = DateTime.Now;  
                    entry.Size = fs.Length;  
                    fs.Close();  
                    crc.Reset();  
                    crc.Update(buffer);  
                    entry.Crc = crc.Value;  
                    s.PutNextEntry(entry);  
                    s.Write(buffer, 0, buffer.Length);  
                }  
            }  
            catch  
            {  
                res = false;  
            }  
            finally  
            {  
                if (fs != null)  
                {  
                    fs.Close();  
                    fs = null;  
                }  
                if (entry != null)  
                    entry = null;  
                GC.Collect();  
                GC.Collect(1);  
            }  
            folders = Directory.GetDirectories(FolderToZip);  
            foreach (string folder in folders)  
            {  
                if (!ZipFileDictory(folder, s, Path.Combine(ParentFolderName, Path.GetFileName(FolderToZip))))  
                    return false;  
            }  
            return res;  
        }  

        #endregion  


        #region ZipFileDictory  

        /// <summary>  
        /// 压缩目录  
        /// </summary>  
        /// <param name="FolderToZip">待压缩的文件夹，全路径格式</param>  
        /// <param name="ZipedFile">压缩后的文件名，全路径格式</param>  
        /// <returns></returns>  
        private  bool ZipFileDictory(string FolderToZip, string ZipedFile, string Password)  
        {  
            bool res;  
            if (!Directory.Exists(FolderToZip))  
                return false;  
            ZipOutputStream s = new ZipOutputStream(File.Create(ZipedFile));  
            s.SetLevel(6);  
            if (!string.IsNullOrEmpty(Password.Trim()))  
                s.Password = Password.Trim();  
            res = ZipFileDictory(FolderToZip, s, "");  
            s.Finish();  
            s.Close();  
            return res;  
        }  

        #endregion  

        #region ZipFile  

        /// <summary>  
        /// 压缩文件  
        /// </summary>  
        /// <param name="FileToZip">要进行压缩的文件名</param>  
        /// <param name="ZipedFile">压缩后生成的压缩文件名</param>  
        /// <returns></returns>  
        private  bool ZipFile(string FileToZip, string ZipedFile, string Password)  
        {  
            //如果文件没有找到，则报错  
            if (!File.Exists(FileToZip))  
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");  
            //FileStream fs = null;  
            FileStream ZipFile = null;  
            ZipOutputStream ZipStream = null;  
            ZipEntry ZipEntry = null;  
            bool res = true;  
            try  
            {  
                ZipFile = File.OpenRead(FileToZip);  
                byte[] buffer = new byte[ZipFile.Length];  
                ZipFile.Read(buffer, 0, buffer.Length);  
                ZipFile.Close();  
                ZipFile = File.Create(ZipedFile);  
                ZipStream = new ZipOutputStream(ZipFile);  
                if (!string.IsNullOrEmpty(Password.Trim()))  
                    ZipStream.Password = Password.Trim();  
                ZipEntry = new ZipEntry(Path.GetFileName(FileToZip));  
                ZipStream.PutNextEntry(ZipEntry);  
                ZipStream.SetLevel(6);  
                ZipStream.Write(buffer, 0, buffer.Length);  
            }  
            catch  
            {  
                res = false;  
            }  
            finally  
            {  
                if (ZipEntry != null)  
                {  
                    ZipEntry = null;  
                }  
                if (ZipStream != null)  
                {  
                    ZipStream.Finish();  
                    ZipStream.Close();  
                }  
                if (ZipFile != null)  
                {  
                    ZipFile.Close();  
                    ZipFile = null;  
                }  
                GC.Collect();  
                GC.Collect(1);  
            }  
            return res;  
        }  

        #endregion  

        #region Zip  

        /// <summary>  
        /// 压缩文件 和 文件夹  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件或文件夹，全路径格式</param>  
        /// <param name="ZipedFile">压缩后生成的压缩文件名，全路径格式</param>  
        /// <param name="Password">压缩密码</param>  
        /// <returns></returns>  
        public  bool Zip(string FileToZip, string ZipedFile, string Password)  
        {  
            if (Directory.Exists(FileToZip))  
            {  
                return ZipFileDictory(FileToZip, ZipedFile, Password);  
            }  
            else if (File.Exists(FileToZip))  
            {  
                return ZipFile(FileToZip, ZipedFile, Password);  
            }  
            else  
            {  
                return false;  
            }  
        }  

        /// <summary>  
        /// 压缩文件 和 文件夹  
        /// </summary>  
        /// <param name="FileToZip">待压缩的文件或文件夹，全路径格式</param>  
        /// <param name="ZipedFile">压缩后生成的压缩文件名，全路径格式</param>  
        /// <returns></returns>  
        public  bool Zip(string FileToZip, string ZipedFile)  
        {  
            return Zip(FileToZip, ZipedFile, "");  
        }  

        #endregion  
    }  
}
