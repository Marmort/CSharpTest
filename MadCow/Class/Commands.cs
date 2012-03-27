﻿/*********************************************************************
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
    class Commands
    {
        public static void CommandReader()
        {
            String command = "";
            do
            {
                Console.Write("键入以！为前缀的命令>> ");
                command = Console.ReadLine();

                if (command == "!updatempq")
                {
                    if (Directory.Exists(Program.programPath + "/MPQ"))
                    {
                        Directory.Delete(Program.programPath + "/MPQ", true);
                        Console.WriteLine("删除当前 MPQ MadCow文件夹成功");
                        Directory.CreateDirectory(Program.programPath + "/MPQ");
                        Console.WriteLine("创建一个新的MPQ MadCow文件夹成功");
                        MPQprocedure.MpqTransfer();
                    }
                    else
                    {
                        Directory.CreateDirectory(Program.programPath + "/MPQ");
                        MPQprocedure.MpqTransfer();
                    }
                }

                if (command != "!updatempq" && command != "!update" && command != "!help" && command != "!autoupdate")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("无效命令");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (command != "!exit");
        }
    }
}