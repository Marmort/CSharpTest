C#使用ICSharpCode SharpZipLib 压缩 解压缩 可以压缩文件夹
作者：skyaspnet | 出处：博客园 | 2011/12/3 4:32:25 | 阅读49次

网上找了找，都差不多，而且很多是错的，只能遍历根目录的第一级文件夹，而且冗长。

下面给出正确的代码。

解压缩代码：

using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace DRMEncryption
{
/// ;summary;
/// UnZip 类用于解压缩一个 zip 文件。
/// ;/summary;
public class UnZipDir
{

//解压缩以后的文件名和路径，压缩前的路径
public static Boolean UNZipFile(string FileToZip, string ZipedFile)
{
try
{
FastZip fastZip = new FastZip();
fastZip.ExtractZip(FileToZip, ZipedFile, "");
return true;
}
catch {
return false;
}

}
}
}



压缩文件代码：

using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.GZip;


namespace DRMEncryption
{
public class ZipClass
{

public static Boolean ZipFile(string FileToZip, string ZipedFile)
{

try
{
FastZip fastZip = new FastZip();

bool recurse = true;

//压缩后的文件名，压缩目录 ，是否递归

fastZip.CreateZip(FileToZip, ZipedFile, recurse, "");
return true;
}
catch { return false; }

}

}
}




那个返回方法是用来另外一个页面判断解压是否成功用的，可以不要;

解压缩：UnZipDir.UNZipFile(文件名, 路径)；

压缩：ZipClass.ZipFile(文件名, 路径)；