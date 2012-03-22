C#利用SharpZipLib解压或压缩文件夹实例操作
时间:2009-3-8 1:33:07 来源:www.cnblogs.com 作者:执着的奋斗着……

最近要做一个项目涉及到C#中压缩与解压缩的问题的解决方法，大家分享。
这里主要解决文件夹包含文件夹的解压缩问题。
1）下载SharpZipLib.dll，在http://www.icsharpcode.net/OpenSource/SharpZipLib/Download.aspx中有最新免费版本，“Assemblies for .NET 1.1, .NET 2.0, .NET CF 1.0, .NET CF 2.0: Download [297 KB] ”点击Download可以下载，解压后里边有好多文件夹，因为不同的版本，我用的FW2.0。
2）引用SharpZipLib.dll，在项目中点击项目右键-->添加引用-->浏览，找到要添加的DLL-->确认
3）改写了文件压缩和解压缩的两个类，新建两个类名字为ZipFloClass.cs,UnZipFloClass.cs
源码如下
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

/// <summary>
/// ZipFloClass 的摘要说明
/// </summary>
public class ZipFloClass
{
    public void ZipFile(string strFile, string strZip)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            strFile += Path.DirectorySeparatorChar;
        ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
        s.SetLevel(6); // 0 - store only to 9 - means best compression
        zip(strFile, s, strFile);
        s.Finish();
        s.Close();
    }


    private void zip(string strFile, ZipOutputStream s, string staticFile)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
        Crc32 crc = new Crc32();
        string[] filenames = Directory.GetFileSystemEntries(strFile);
        foreach (string file in filenames)
        {

            if (Directory.Exists(file))
            {
                zip(file, s, staticFile);
            }

            else // 否则直接压缩文件
            {
                //打开压缩文件
                FileStream fs = File.OpenRead(file);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempfile);

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
    }

}

、、、、、、、、、、、、、、、

using System;
using System.Data;
using System.Web;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Checksums;

/// <summary>
/// UnZipFloClass 的摘要说明
/// </summary>
public class UnZipFloClass
{

    public string unZipFile(string TargetFile, string fileDir)
    {
        string rootFile = " ";
        try
        {
            //读取压缩文件(zip文件)，准备解压缩
            ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
            ZipEntry theEntry;
            string path = fileDir;                   
            //解压出来的文件保存的路径

            string rootDir = " ";                        
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
                rootDir = Path.GetDirectoryName(theEntry.Name);                          
                //得到根目录下的第一级子文件夹的名称
                if (rootDir.IndexOf("\\") >= 0)
                {
                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                }
                string dir = Path.GetDirectoryName(theEntry.Name);                    
                //根目录下的第一级子文件夹的下的文件夹的名称
                string fileName = Path.GetFileName(theEntry.Name);                    
                //根目录下的文件名称
                if (dir != " " )                                                        
                    //创建根目录下的子文件夹,不限制级别
                {
                    if (!Directory.Exists(fileDir + "\\" + dir))
                    {
                        path = fileDir + "\\" + dir;                                                
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")                                              
                    //根目录下的文件
                {
                    path = fileDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")                                              
                    //根目录下的第一级子文件夹下的文件
                {
                    if (dir.IndexOf("\\") > 0)                                                            
                        //指定文件保存的路径
                    {
                        path = fileDir + "\\" + dir;
                    }
                }

                if (dir == rootDir)                                                                                  
                    //判断是不是需要保存在根目录下的文件
                {
                    path = fileDir + "\\" + rootDir;
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
                    FileStream streamWriter = File.Create(path + "\\" + fileName);

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

                    streamWriter.Close();
                }
            }
            s.Close();

            return rootFile;
        }
        catch (Exception ex)
        {
            return "1; " + ex.Message;
        }
    }   
}

4）引用，新建一个页面，添加两个按钮，为按钮添加Click事件

源码如下
 protected void Button1_Click(object sender, EventArgs e)
    {
        string[] FileProperties = new string[2];
        FileProperties[0] = "D:\\unzipped\\";//待压缩文件目录
        FileProperties[1] = "D:\\zip\\a.zip";  //压缩后的目标文件
        ZipFloClass Zc = new ZipFloClass();
        Zc.ZipFile(FileProperties[0], FileProperties[1]);

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string[] FileProperties = new string[2];
        FileProperties[0] = "D:\\zip\\b.zip";//待解压的文件
        FileProperties[1] = "D:\\unzipped\\";//解压后放置的目标目录
        UnZipFloClass UnZc = new UnZipFloClass();
        UnZc.unZipFile(FileProperties[0], FileProperties[1]);
    }

5）一切OK，可以测试一下，我是可以运行的