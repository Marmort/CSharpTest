今天是倒数第2次架构课了，老师重点讲了关于WEB2.0 的技术和模式，其中也附带提到了一个新的名词“开源代码”！

或许一节课上下来很多人都还是没弄明白什么是“开源代码”。也想知道咱“肖总”一直追求的到底是个啥东东！

后来在百度上收索了一下，找到了一个很典型的开源网站http://www.cn.redhat.com/opensourcenow/，

看了之后我了解到：开源代码是大势所趋。它把对程序代码的控制权完全交给用户—您可以研究它；修改它；学习它。

其中的错误会被很快地发现并被改正。如果用户想改变他们的服务供应商，

他们完全可以选择他们所需要的厂商而不需要重新修改他们的系统。这意味着，没有专制的价格；

没有被套牢在一个专门的技术中；没有更多的垄断。

或许这样说很抽象！也很虚，例举一下我在网上看到的一段代码来研究一下什么叫“开源” 吧，

看到一篇帖子，发贴人说他在看DNN时发现了一个很酷的功能：

能通过IE浏览器实现对Zip文件的压缩和生成Zip文件压缩包的功能。

在仔细看过程序以后发现它是调用的SharpZipLib.dll类库中的内容实现的压缩与解压功能。

上网查了一下SharpZipLib，发现它居然是开源的，在http://www.icsharpcode.net网站上有下。

在网站里关于SharpZipLib的源文件和调用演示包括帮助文档都有下，不过当然全是E文的。

在SharpZipLib中实现解压的方法（演示代码）：

using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;


class MainClass
{           
    // 在控制命令行下输入要解压的Zip文件名
    public static void Main(string[] args)
    {
        // 创建读取Zip文件对象
        ZipInputStream s = new ZipInputStream(File.OpenRead(args[0]));
        // Zip文件中的每一个文件
        ZipEntry theEntry;
        // 循环读取Zip文件中的每一个文件
        while ((theEntry = s.GetNextEntry()) != null) {
           
            Console.WriteLine(theEntry.Name);
           
            string directoryName = Path.GetDirectoryName(theEntry.Name);
            string fileName      = Path.GetFileName(theEntry.Name);
           
            // create directory
            Directory.CreateDirectory(directoryName);
           
            if (fileName != String.Empty) {
                // 解压文件
                FileStream streamWriter = File.Create(theEntry.Name);
               
                int size = 2048;
                byte[] data = new byte[2048];
                while (true) {
                    // 写入数据
                    size = s.Read(data, 0, data.Length);
                    if (size > 0) {
                        streamWriter.Write(data, 0, size);
                    } else {
                        break;
                    }
                }
               
                streamWriter.Close();
            }
        }
        s.Close();
    }
}
在SharpZipLib中实现压缩的方法：
using System;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;

class MainClass
{
    // 输入要压缩的文件夹和要创建的Zip文件文件名称
    public static void Main(string[] args)
    {
        string[] filenames = Directory.GetFiles(args[0]);
       
        Crc32 crc = new Crc32();
        // 创建输出Zip文件对象
        ZipOutputStream s = new ZipOutputStream(File.Create(args[1]));
        // 设置压缩等级
        s.SetLevel(6); // 0 - store only to 9 - means best compression
        // 循环读取要压缩的文件，写入压缩包       
        foreach (string file in filenames) {
            FileStream fs = File.OpenRead(file);
           
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            ZipEntry entry = new ZipEntry(file);
           
            entry.DateTime = DateTime.Now;
           
            entry.Size = fs.Length;
            fs.Close();
           
            crc.Reset();
            crc.Update(buffer);
           
            entry.Crc  = crc.Value;
           
            s.PutNextEntry(entry);
           
            s.Write(buffer, 0, buffer.Length);
           
        }
       
        s.Finish();
        s.Close();
    }
}
    .NET和JAVA专业的可能会看的比较有眉目，而我们WEB专业的可能仅能拿我们那点C的小基础看出个小苗头罢了！

     不过呢我想我大概也明白了，所谓的开源就是各个软件和操作系统生产商的源代码在网上公开，
     
     然后由一部分对此代码感兴趣的群体，去研究这些代码，并在这些源码的基础上增加功能和找出BUG并做修改。
     
     可能理解的还很浅显！不过我大概搞明白开源到底是做啥用的东东啦！我想也许“番茄XP”算是开源的某种产物吧！
     
     还有大大小小的个人博客，BBS。也该算是！因为没有源码的公开，可能就全是技术垄断，也就没有这么多的个人博客，
     
     和形形色色的BBS啦  ！  开源真是我们滴福音哦！鄙视技术垄断！不过提到开源的种种好处也不尽想到一个问题，
     
     都开源了，那程序员做出来的软件还怎么卖钱呢？他做出来的东西把源代码一公开，我们全都免费拿来用了，
     
     那所谓的“专利”又有么司意思呢？昏迷哦？？ 矛盾矛盾！所以盲目开源是不行的 