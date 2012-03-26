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

namespace MadCow
{
    class MPQprocedure
    {
        public static void MpqTransfer()
        {
            Console.WriteLine("复制MPQ文件到MadCow 文件夹......");
            String Src = FindDiablo3.FindDiabloLocation() + "\\Data_D3\\PC\\MPQs";
            String Dst = Program.programPath +"\\MPQ";
            copyDirectory(Src, Dst);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("过程已经成功完成");
            Console.WriteLine("检查你的桌面为Mooege快捷键");
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
                    || Element.Contains("HLSLShaders") || Element.Contains("patch"))
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