/*********************************************************************
 * Copyright (C) 2011 Iker Ruiz Arnauda (Wesko)
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*********************************************************************/

using System;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

namespace MadCow
{
    class MPQprocedure
    {
        //目录\base\ 下的文件 MD5's.
        public static String[] MD5ValidPool = {"39765d908accf4f37a4c2dfa99b8cd52"//7170
                                                                   ,"7148ee45696c84796f3ca16729b9aadc"   //7200
                                                                   ,"7ee326516f3da2c8f8b80eba6199deef"   //7318
                                                                   ,"68c43ae976872a1fa7f5a929b7f21b58"   //7338
                                                                   ,"751b8bf5c84220688048c192ab23f380"   //7447
                                                                   ,"d5eba8a2324cdc815b2cd5b92104b7fa"   //7728
                                                                   ,"5eb4983d4530e3b8bab0d6415d8251fa"   //7841
                                                                   ,"3faf4efa2a96d501c9c47746cba5a7ad"}; //7841

        public static void ValidateMD5()
        {
            DateTime startTime = DateTime.Now;
            String src = FindDiablo3.FindDiabloLocation() + "\\Data_D3\\PC\\MPQs\\base";
            String[] filePaths = Directory.GetFiles(src, "*.*", SearchOption.TopDirectoryOnly);
            int fileCount = Directory.GetFiles(src, "*.*", SearchOption.TopDirectoryOnly).Length;
            int trueCounter = 0;

            Parallel.ForEach(filePaths, dir =>
            {
                string md5Filecheck = Md5Validate.GetMD5HashFromFile(dir);

                for (int i = 0; i < MD5ValidPool.Length; i++)
                {
                    if (md5Filecheck.Contains(MD5ValidPool[i]) == true)
                    {
                        trueCounter += 1;
                    }
                }
            });

            if (fileCount == trueCounter)
            {
                DateTime stopTime = DateTime.Now;
                TimeSpan duration = stopTime - startTime;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("MD5校验MPQ的哈希完成--{0}",duration.Milliseconds);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("MD5校验MPQ的哈希失败" + "\n 请重新安装暗黑3客户端或试着用D3 Launcher来修复问题");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public static void MpqTransfer()
        {
            Console.WriteLine("复制MPQ文件到MadCow 文件夹......");
            String Src = FindDiablo3.FindDiabloLocation() + "\\Data_D3\\PC\\MPQs";
            String Dst = Program.programPath +"\\MPQ";
            copyDirectory(Src, Dst);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("复制过程已经完成");
            Console.WriteLine("检查你的桌面的Mooege快捷键");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void copyDirectory(String Src, String Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) 
                Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (String Element in Files)
            {
                // 建立子目录， 如果不需要的MPQ's需要则不复制
                if (Directory.Exists(Element) && Element.Contains("enUS") || Element.Contains("Cache") 
                    || Element.Contains("Win") || Element.Contains("enUS_Audio") 
                    || Element.Contains("enUS_Cutscene") || Element.Contains("enUS_Text") 
                    || Element.Contains("Sound") || Element.Contains("Texture")
                    || Element.Contains("HLSLShaders"))
                {
                    Console.WriteLine("忽略： " + Path.GetFileName(Element));
                }
                else if (Directory.Exists(Element))
                {
                    copyDirectory(Element, Dst + Path.GetFileName(Element));
                    Console.WriteLine("创建目录： " + Path.GetFileName(Element));
                }
                else
                {
                    Console.WriteLine("复制：" + Path.GetFileName(Element) + "......");
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
                    Console.WriteLine("复制：" + Path.GetFileName(Element) + " 完成");
                }
            } 
            Console.WriteLine("复制MPQ文件到MadCow文件夹结束");
        }
    }
}