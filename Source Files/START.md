ICSharpCode.SharpZipLib 初级使用

最近做的一个项目需要使用到在线解压缩的过程。

需求是这样的，用户可以将所有需要上传的文件进行打包然后上传到服务器，

服务器将压缩包进行解压，然后对其中的文件进行逐个处理。

其中将压缩包进行服务器端解压的过程就是通过ICSharpCode.SharpZipLib.dll来实现的。

对于这个dll文件，可以通过搜索这个dll文件的名字下载到。

原来没有使用过，所以拿来帮助文档依葫芦画瓢。

1. 在项目中添加对ICSharpCode.SharpZipLib.dll的引用；

2. 在需要使用到ICSharpCode.SharpZipLib中定义的类的编码界面中将其导入(Imports)

(在C#中是using);

3. 在选择命名空间的时候，你会发现在有这样几个同级的命名空间：

ICSharpCode.SharpZipLib.BZip2;

ICSharpCode.SharpZipLib.GZip;

ICSharpCode.SharpZipLib.Tar;

ICSharpCode.SharpZipLib.Zip。

这四个命名空间就对应着四种文件压缩方式，其中我们用的较多的是Zip的压缩方式，

因为通过WinRAR软件就可以将文件压缩成.Zip的压缩包。这里有两点需要说明一下：

1) 目前还没有发现提供对.RAR方式压缩文件操作的方法；

2) BZip2, GZip 这两种压缩算法仅仅针对一个文件的压缩，

如果你的压缩包要包含许多文件，并需要将这些文件解压出来进行操作的话，最好选用Tar和Zip的压缩方式。

关于这四种压缩算法的介绍可以从维基百科上得到，这里就不再赘述了。

好了，下面看看在服务器端对Zip压缩文件进行解压缩的过程，下面是其帮助文档中提供的例子。

由于这里最需要注意的就是ZipInputStream类的使用，所以你可以通过在帮助文档中搜索ZipInputStream找到这个例子。

using System;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

class MainClass
{
    public static void Main(string[] args)
    {
       //这里通过File.OpenRead方法读取指定文件，并通过其返回的FileStream构造ZipInputStream对象；
        using ( ZipInputStream s = new ZipInputStream(File.OpenRead(args[0]))) {

           //每个包含在Zip压缩包中的文件都被看成是ZipEntry对象，并通过ZipInputStream的GetNextEntry方法
     //依次遍历所有包含在压缩包中的文件。
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null) {
                int size = 2048;
                byte[] data = new byte[2048];
                //然后以文件数据块的方式将数据打印在控制台上；
                Console.Write("Show contents (y/n) ?");
                if (Console.ReadLine() == "y") {
                    while (true) {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0) {
                            Console.Write(new ASCIIEncoding().GetString(data, 0, size));
                        } else {
                            break;
                        }
                    }
                }
            }
        }
    }
}

这里需要注意的几个属性是：

1.  ZipEntry.Name, 可以得到包含在要所报文件中的文件名；

2.  ZipEntry.CompressedSize, 是指当前文件被压缩后的大小；

3.  ZipEntry.Size 是指当前文件原本大小。

这两个大小(Size)需要指明清楚，否则可能在写入文件的时候会出现文件内容被截断的现象。