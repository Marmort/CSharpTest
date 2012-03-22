C#版的文件及文件夹的压缩解压

最近刚来新公司，投的是java不过好像进来是让做C＋＋，
但由于机器在申请中所以又跑到.net组里的空机子上帮忙去了。
今天刚坐了个C#文件及文件夹的压缩解压。想想以后可能还会用到所以就放上来了。


    using System.Windows.Forms;  
    using System.IO;  
    using ICSharpCode.SharpZipLib;  
    using ICSharpCode.SharpZipLib.Checksums;  
    using ICSharpCode.SharpZipLib.Zip;  
      
    namespace ZipApplication  
    {  
        public partial class Form1 : Form  
        {  
            string fullName = "";  
            string destPath = "";  
            ZipClass zp = new ZipClass();  
            UnZipClass UZp = new UnZipClass();  
            public Form1()  
            {  
                InitializeComponent();  
            }  
     
     
            #region 文件浏览  
            /// <summary>  
            /// 压缩浏览  
            /// </summary>  
            /// <param name="sender"></param>  
            /// <param name="e"></param>  
            private void btnBroswer_Click(object sender, EventArgs e)  
            {  
                //待压缩文件  
      
                folderBrowserDialog1.ShowDialog();  
                fullName = folderBrowserDialog1.SelectedPath;  
                textBox1.Text = fullName;  
                //压缩到的路径  
                destPath = System.IO.Path.GetDirectoryName(fullName);  
                //压缩后的目标文件  
                destPath = destPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fullName) + ".zip";  
                  
                  
      
            }  
      
            /// <summary>  
            /// 解压浏览  
            /// </summary>  
            /// <param name="sender"></param>  
            /// <param name="e"></param>  
            private void btnUzipBro_Click(object sender, EventArgs e)  
            {  
                openFileDialog1.ShowDialog();  
                fullName = openFileDialog1.FileName;  
                textBox2.Text = fullName;  
                //解压到的路径  
                destPath = System.IO.Path.GetDirectoryName(fullName);  
            }  
            #endregion  
     
            #region  压缩  
            /// <summary>  
            /// 压缩  
            /// </summary>  
            /// <param name="sender"></param>  
            /// <param name="e"></param>  
            private void btnZip_Click(object sender, EventArgs e)  
            {  
      
                zp.Zip(fullName, destPath);  
                MessageBox.Show("压缩成功！");  
            }  
            
            #endregion  
      
            #region 解压  
            /// <summary>  
            /// 解压  
            /// </summary>  
            /// <param name="sender"></param>  
            /// <param name="e"></param>  
            private void btnUnZip_Click(object sender, EventArgs e)  
            {  
                  
                UZp.UnZip(fullName,destPath);  
                MessageBox.Show("解压成功！");  
      
            }  
            #endregion  
      
             
        }  
        #region ZipClass压缩类  
      
        /// <summary>  
        /// 压缩类  
        /// </summary>  
        public class ZipClass  
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
      
        #endregion  
     
    #region ZipClass解压类  
        /// <summary>  
        ///  解压类  
        /// </summary>  
        public class UnZipClass  
        {  
            /// <summary>  
            /// 解压功能(解压压缩文件到指定目录)  
            /// </summary>  
            /// <param name="FileToUpZip">待解压的文件</param>  
            /// <param name="ZipedFolder">指定解压目标目录</param>  
            public static void UnZip(string FileToUpZip, string ZipedFolder, string Password)  
            {  
                if (!File.Exists(FileToUpZip))  
                {  
                    return;  
                }  
      
                if (!Directory.Exists(ZipedFolder))  
                {  
                    Directory.CreateDirectory(ZipedFolder);  
                }  
      
                ZipInputStream s = null;  
                ZipEntry theEntry = null;  
      
                string fileName;  
                FileStream streamWriter = null;  
                try  
                {  
                    s = new ZipInputStream(File.OpenRead(FileToUpZip));  
                    s.Password = Password;  
                    while ((theEntry = s.GetNextEntry()) != null)  
                    {  
                        if (theEntry.Name != String.Empty)  
                        {  
                            fileName = Path.Combine(ZipedFolder, theEntry.Name);  
                            ///判断文件路径是否是文件夹  
                            if (fileName.EndsWith("/") || fileName.EndsWith("\\"))  
                            {  
                                Directory.CreateDirectory(fileName);  
                                continue;  
                            }  
      
                            streamWriter = File.Create(fileName);  
                            int size = 2048;  
                            byte[] data = new byte[2048];  
                            while (true)  
                            {  
                                size = s.Read(data, 0, data.Length);  
                                if (size > 0)  
                                {  
                                    streamWriter.Write(data, 0, size);  
                                }  
                                else  
                                {  
                                    break;  
                                }  
                            }  
                        }  
                    }  
                }  
                finally  
                {  
                    if (streamWriter != null)  
                    {  
                        streamWriter.Close();  
                        streamWriter = null;  
                    }  
                    if (theEntry != null)  
                    {  
                        theEntry = null;  
                    }  
                    if (s != null)  
                    {  
                        s.Close();  
                        s = null;  
                    }  
                    GC.Collect();  
                    GC.Collect(1);  
                }  
            }  
        }  
        #endregion  
       }  