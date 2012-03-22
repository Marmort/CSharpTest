zz使用ICSharpCode.SharpZipLib.dll

默认分类   2006-11-09 10:40   阅读279   评论0  

字号： 大大  中中  小小

使用ICSharpCode.SharpZipLib.dll；  

  下载地址  

  http://www.icsharpcode.net/OpenSource/SharpZipLib/Download.aspx  

   

  下面是对#ZipLib进行.net下的解压缩的方法的介绍。  

   

        1.BZip2    

          加入ICSharpCode.SharpZipLib.dll的引用，在#Develop的安装目录下的\SharpDevelop\bin目录下。然后在程序中使用using语句把BZip2  

   

  类库包含进来。    

  压缩：使用BZip2的静态方法Compress。    

          它的第一个参数是所要压缩的文件所代表的输入流，可以使用System.IO.File的静态方法OpenRead。    

          第二个参数是要建立的压缩文件所代表的输出流，可以使用System.IO.File的静态方法Create创建，压缩文件名是所要压缩文件的文件名  

   

  加上压缩后缀.bz（同样你也可以取其他的文件名）。    

          第三个参数是要压缩的块大小（一般为2048的整数）。    

   

  解压:使用BZip2的静态方法Decompress。    

          它的第一个参数是所要解压的压缩文件所代表的输入流，可以使用System.IO.File的静态方法OpenRead。    

          第二个参数是要建立的解压文件所代表的输出流，可以使用System.IO.File的静态方法Create创建，因为解压文件的文件名是去掉了压缩  

   

  文件扩展名的压缩文件名（你也可以做成解压文件与压缩文件不同名的）。    

  编译你的程序，然后在命令行方式下输入bzip2   文件名（假设建立的C#文件是bzip2，就可以生成压缩文件；输入bzip2   -d   文件名，就会解压  

   

  出文件来（-d是用来表示解压，你也可以使用其他的符号）。    

  呵呵，原来做压缩可以这么简单的，压缩效果也可以啊。    

  using   System;    

  using   System.IO;    

  using   ICSharpCode.SharpZipLib.BZip2;    

   

  class   MainClass    

  {    

        public   static   void   Main(string[]   args)    

        {    

              if   (args[0]   ==   "-d")   {   //   解压    

                    BZip2.Decompress(File.OpenRead(args[1]),   File.Create(Path.GetFileNameWithoutExtension(args[1])));    

              }   else   {   //压缩    

                    BZip2.Compress(File.OpenRead(args[0]),   File.Create(args[0]   +   ".bz"),   4096);    

              }    

        }    

  }    

  2.GZip      

        加入ICSharpCode.SharpZipLib.dll的引用，在#Develop的安装目录下的\SharpDevelop\bin目录下。然后在程序中使用using语句把GZip类  

   

  库包含进来。      

        由于GZip没有BZip2的简单解压缩方法，因此只能使用流方法来进行解压缩。具体的方法见程序的说明。    

        编译程序，然后在命令行方式下输入GZip   文件名（假设建立的C#文件是GZip，就可以生成压缩文件；输入GZip   -d   文件名，就会解压出文  

   

  件来（-d是用来表示解压，你也可以使用其他的符号）。      

   

  using   System;    

  using   System.IO;    

   

  using   ICSharpCode.SharpZipLib.GZip;    

   

  class   MainClass    

  {    

        public   static   void   Main(string[]   args)    

        {    

              if   (args[0]   ==   "-d")   {   //   解压    

                    Stream   s   =   new   GZipInputStream(File.OpenRead(args[1]));    

                    //生成一个GZipInputStream流，用来打开压缩文件。    

                  //因为GZipInputStream由Stream派生，所以它可以赋给Stream。    

                      //它的构造函数的参数是一个表示要解压的压缩文件所代表的文件流    

                    FileStream   fs   =   File.Create(Path.GetFileNameWithoutExtension(args[1]));    

                    //生成一个文件流，它用来生成解压文件    

                    //可以使用System.IO.File的静态函数Create来生成文件流    

                    int   size   =   2048;//指定压缩块的大小，一般为2048的倍数    

                    byte[]   writeData   =   new   byte[size];//指定缓冲区的大小    

                    while   (true)   {    

                          size   =   s.Read(writeData,   0,   size);//读入一个压缩块    

                          if   (size   >   0)   {    

                                fs.Write(writeData,   0,   size);//写入解压文件代表的文件流    

                          }   else   {    

                                break;//若读到压缩文件尾，则结束    

                          }    

                    }    

                    s.Close();    

              }   else   {   //   压缩    

                    Stream   s   =   new   GZipOutputStream(File.Create(args[0]   +   ".gz"));    

                    //生成一个GZipOutputStream流，用来生成压缩文件。    

                                                  //因为GZipOutputStream由Stream派生，所以它可以赋给Stream。    

                      FileStream   fs   =   File.OpenRead(args[0]);    

                    /生成一个文件流，它用来打开要压缩的文件    

                    //可以使用System.IO.File的静态函数OpenRead来生成文件流    

                    byte[]   writeData   =   new   byte[fs.Length];    

                    //指定缓冲区的大小    

                    fs.Read(writeData,   0,   (int)fs.Length);    

                    //读入文件    

                    s.Write(writeData,   0,   writeData.Length);    

                    //写入压缩文件    

                    s.Close();    

                    //关闭文件    

              }    

        }    

  }   