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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MadCow
{
    class MadCowRunProcedure
    {
        public static void RunMadCow(int runType) //0 - 没有D3客户端 //1 - 客户端已安装
        {
            if (runType == 1)
            {
                PreRequeriments.CheckPrerequeriments();
                Diablo3.FindDiablo3();
                Diablo3.VerifyVersion();
                DownloadRevision.DownloadLatest();
                Uncompress.UncompressFiles();
                Compile.ExecuteCommandSync(Compile.msbuildPath + " " + Compile.compileArgs);
                Compile.ModifyMooegeINI();
                Compile.WriteVbsPath();
                MPQprocedure.ValidateMD5();
                MPQprocedure.MpqTransfer();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n程序已经编译成功，请检查桌面Mooege快捷键");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                PreRequeriments.CheckPrerequeriments();
                DownloadRevision.DownloadLatest();
                Uncompress.UncompressFiles();
                Compile.ExecuteCommandSync(Compile.msbuildPath + " " + Compile.compileArgs);
                Compile.ModifyMooegeINI();
                Compile.WriteVbsPath();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n 如果不能自动复制MPQ请手动复制MPQ到..//MadCow/MPQ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n程序已经编译成功，请检查桌面Mooege快捷键");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
