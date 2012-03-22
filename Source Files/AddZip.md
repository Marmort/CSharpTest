利用ICSharpCode.SharpZipLib对压缩文件添加新文件
作者：freemobile | 出处：博客园 | 2011/11/16 19:53:17 | 阅读37次

    看了很多关于ICSharpCode.SharpZipLib进行压缩的文章，都是将一个文件和目录压缩成一个全新的ZIP文件，即ZipOutputStream s = new ZipOutputStream(File.Create("c:""d.zip")))。而我要实现的是对存在的zip文件添加新的文件，我采用ZipOutputStream s = new ZipOutputStream(File.OpenCreate("c:\\d.zip")))方式，程序执行没有问题，但打开d.zip并没有我添加的文件。

    想了个变通的方法，产生个新的zip文件，把原来zip中的文件和要添加的新文件，统一添加到新的zip中，代码如下：

        public void AddZip(string from,string toZip)
        {
            try
            {
                using (ZipOutputStream s = new ZipOutputStream(File.Create(toZip)))
                {
                    //s.SetLevel(9);
                    ZipEntry entry = null;
                    ZipInputStream p = new ZipInputStream(File.OpenRead(from));
                    while((entry = p.GetNextEntry()) != null)
                    {
                        ZipEntry zipEntry = new ZipEntry(entry.Name);
                        zipEntry.DateTime = DateTime.Now;
                        s.PutNextEntry(zipEntry);
                        byte[] data = new byte[204800];
                        int sourceBytes;
                        do
                        {
                            sourceBytes = p.Read(data, 0, data.Length);
                            s.Write(data, 0, sourceBytes);
                        } while (sourceBytes > 0);

                        //p.Read(data, 0, data.Length);http://www.joymo.cn
                        //s.Write(data, 0, data.Length);
                    }
                    s.Finish();
                    s.Close();
                    p.Close();
                }
            }
            catch (Exception ex)
            {
                throw new System.NotImplementedException(ex.Message);
            }
        }

     功能是实现了，感觉上不如我用VC做的joymobiler中ZipArchive压缩速度快。