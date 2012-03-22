用ICSharpCode.SharpZipLib.Zip实现下载整个文件目录

using ICSharpCode.SharpZipLib.Zip;
using System.IO;

public partial class Default2 : System.Web.UI.Page
{
     ZipOutputStream zos = null;
     String strBaseDir = "";

     protected void Page_Load(object sender, EventArgs e)
     {
         dlZipDir(Server.MapPath("test"), "test");
         //网站根目录建立一个test文件夹,里面随便放几个文件进去,子文件夹也行
     }

     protected void dlZipDir(string strPath, string strFileName)
     {
         MemoryStream ms = null;
         Response.ContentType = "application/octet-stream";
         strFileName = HttpUtility.UrlEncode(strFileName).Replace('+', ' ');
         Response.AddHeader("Content-Disposition", "attachment; filename=" + strFileName + ".zip");
         ms = new MemoryStream();
         zos = new ZipOutputStream(ms);
         strBaseDir = strPath + "//";
         addZipEntry(strBaseDir);
         zos.Finish();
         zos.Close();
         Response.Clear();
         Response.BinaryWrite(ms.ToArray());
         Response.End();
     }

     protected void addZipEntry(string PathStr)
     {
         DirectoryInfo di = new DirectoryInfo(PathStr);
         foreach (DirectoryInfo item in di.GetDirectories())
         {
             addZipEntry(item.FullName);
         }
         foreach (FileInfo item in di.GetFiles())
         {
             FileStream fs = File.OpenRead(item.FullName);
             byte[] buffer = new byte[fs.Length];
             fs.Read(buffer, 0, buffer.Length);
             string strEntryName = item.FullName.Replace(strBaseDir, "");
             ZipEntry entry = new ZipEntry(strEntryName);
             zos.PutNextEntry(entry);
             zos.Write(buffer, 0, buffer.Length);
             fs.Close();
         }
     }
}

用递归的方式，调用addZipEntry遍历目标文件夹中所有的文件或子文件夹，

将压缩数据暂时放入MemoryStream内存流中，然后用Response.BinaryWrite输出到页面
