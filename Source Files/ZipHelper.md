我在AgileIM的开发中解决视频/音频会话功能时，发现传输的音/视频数据量太大，
通过一些格式转换（如BMP->JPG、或 帧间预测编码）可以适当减少带宽的需求，
但是仍然不能满足需求，于是我想到了数据压缩，经过我测试、普通的音/视频数据经压缩后可以只有原来的一半大小，
基本可以满足需要了。压缩/解压功能我借助了ICSharpCode.SharpZipLib 类库，
为了更方便易用，我封装了ZipHelper类，实现如下：

public static class ZipHelper
{
public static byte[] Zip(byte[] data)
{
MemoryStream mstream = new MemoryStream();
BZip2OutputStream zipOutStream = new BZip2OutputStream(mstream);
zipOutStream.Write(data, 0, data.Length);
zipOutStream.Finalize();
zipOutStream.Close();

byte[] result = mstream.ToArray();
mstream.Close();

return result;
}

public static byte[] Unzip(byte[] data)
{
MemoryStream mstream = new MemoryStream(data);
BZip2InputStream zipInputStream = new BZip2InputStream(mstream);
byte[] byteUncompressed = new byte[zipInputStream.Length];
zipInputStream.Read(byteUncompressed, 0, (int)byteUncompressed.Length);

zipInputStream.Close();
mstream.Close();

return byteUncompressed;
}
}


在我前面的“压缩与解压缩 ZipHelper ”一文中提到了使用ICSharpCode.SharpZipLib.dll库

的BZip2OutputStream和BZip2InputStream 来进行数据流的压缩。

这几天在我的AgileIM的测试中发现使用BZip2OutputStream和BZip2InputStream 来进行压缩/解压缩并不可靠，

有时会出现这样的情况：对A进行压缩得到B，然后对B解压缩，得到C的却与A不等，通常的情况是，

C只是A的首部的一部分。我没有研究ICSharpCode.SharpZipLib的源码，所以还没发现是什么原因造成了这种情况。
   
幸运的是，我发现了一个更好的替代品－－仍然是ICSharpCode.SharpZipLib类库中的一个类－－BZip2，

这个类的使用更方便，而且经过我的不完全测试，其运行的结果是可靠的。所以将ZipHelper的实现修正如下：
 

1 public class ZipHelper
2 {
3 public static byte[] Zip(byte[] data)
4 {
5 return Zip(data ,0 ,data.Length) ;
6 }
7
8 public static byte[] Unzip(byte[] data)
9 {
10 return Unzip(data ,0 ,data.Length) ;
11 }
12
13 public static byte[] Zip(byte[] data ,int offset ,int size)
14 {
15 MemoryStream inStream = new MemoryStream(data ,offset ,size);
16 MemoryStream outStream = new MemoryStream() ;
17 BZip2.Compress(inStream ,outStream ,size) ;
18
19 byte[] result = outStream.ToArray() ;
20 inStream.Close() ;
21 outStream.Close() ;
22
23 return result ;
24 }
25
26 public static byte[] Unzip(byte[] data ,int offset ,int size)
27 {
28 MemoryStream inStream = new MemoryStream(data ,offset ,size);
29 MemoryStream outStream = new MemoryStream() ;
30 BZip2.Decompress(inStream ,outStream ) ;
31
32 byte[] result = outStream.ToArray() ;
33 inStream.Close() ;
34 outStream.Close() ;
35
36 return result ;
37 }
38 }