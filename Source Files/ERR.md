前段时间用到了ICSharpCode.SharpZipLib.dll来实现压缩和解压缩。

先说下使用，首先是在“引用”中添加这个dll，然后就可以使用它了，举个例子：

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

 

 

        /// <summary>
        /// 利用DLL压缩
        /// </summary>
        /// <param name="strSourceFilePath">源数据的所在路径</param>
        /// <param name="strSaveZipPath">压缩文件的路径及文件名</param>
        /// <returns></returns>
        public void ZipAll(string strSourceFilePath, string strSaveZipPath)
        {
            // 新建一个list数组,用于存储压缩后的文件的名字
            //ArrayList list = new ArrayList();
            try
            {
                string[] filenames = Directory.GetFiles(strSourceFilePath);

                //list.Add(filenames);
                Crc32 crc = new Crc32();//新建Checksums的Crc32类对象

                ZipOutputStream ZipStream = new ZipOutputStream(File.Create(strSaveZipPath));
                ZipStream.Password = ClsOverAllVar.GetClsOverAllVar().strZipPassword;

                foreach (string file in filenames)
                {
                    //打开要压缩的文件

                    FileStream fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    //获取压缩文件的相对路径

                    string files = file.Substring(file.LastIndexOf("//"));
                    ZipEntry entry = new ZipEntry(files);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;

                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    ZipStream.PutNextEntry(entry);
                    ZipStream.Write(buffer, 0, buffer.Length);
                }

                ZipStream.Finish();
                ZipStream.Close();
                //return list;
            }
            catch (Exception ee)
            {
                    throw ee;
            }
        }

 

 

 

上面这个方法就实现了利用dll压缩一个文件夹的功能。

 

然后说说我的问题，一天更新程序，突然这个dll就不好使了，总是出现——

Could not load file or assembly 'ICSharpCode.SharpZipLib, Version=0.85.1.271,
 Culture=neutral, PublicKeyToken=1b03e6acf1164f73' or one of its dependencies.
The located assembly's manifest definition does not match the assembly reference.
 (Exception from HRESULT: 0x80131040)

 

的错误，为了解决这个问题，我网上搜了很久，尝试了很多方法，都以失败告终，但是还是得到点启示，有位先人说要把这个dll注册到OS中，这样系统才能找到dll并使用。

 

我注册了但是到GAC这一步失败了。原因是：

程序在引用这个dll的时候是否是强命名

解决问题的方法：

********如果不是强命名，那只需要把release后的exe和这个dll放到一起，就可以使用，不需要什么注册，系统会自动去寻找这个dll

********如果是强命名可以像那位高人所说去注册下，然后使用

 

********注册方法是，开始----->.net framework---->启动命令窗口，输入命令

RegAsm /tlb ICSharpCode.SharpZipLib.dll
GACUTIL /i ICSharpCode.SharpZipLib.dll

后面这个dll 如果不在net framework的命令窗口所在的文件夹下，那就要写入全路径。

注册完成，可以使用

 

 

*************************

顺便说下，对于我这等初级中的初级选手，如何看一个引用的dll是强命名？只需要在那个引用---->*.dll点右键，查看属性项“强命名”的值即可。TRUE和FALSE