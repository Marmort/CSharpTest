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
using Nini.Config;
using System.Text.RegularExpressions;

namespace MadCow
{
    class Compile
    {
        public static String currentMooegeExePath = Program.programPath + @"\mooege-mooege-" + Program.lastRevision + @"\src\Mooege\bin\Debug\Mooege.exe";
        public static String currentMooegeDebudFolderPath = Program.programPath + @"\mooege-mooege-" + Program.lastRevision + @"\src\Mooege\bin\Debug\";
        public static String mooegeINI = Program.programPath + @"\mooege-mooege-" + Program.lastRevision + @"\src\Mooege\bin\Debug\config.ini";
        public static String compileArgs = Program.programPath + @"\mooege-mooege-" + Program.lastRevision + @"\build\Mooege-VS2010.sln";
        public static String msbuildPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.System) + @"\..\Microsoft.NET\Framework\v4.0.30319\msbuild.exe";

        public static void ExecuteCommandSync(String command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInformation = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + command);
            
                procStartInformation.RedirectStandardOutput = false;
                procStartInformation.UseShellExecute = true;
                procStartInformation.CreateNoWindow = true;
                
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInformation;
                Console.WriteLine("编译最新Mooege的源代码......");
                proc.Start();
                proc.WaitForExit();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("编译最新Mooege的源代码完成");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception objException)
            {
                Console.WriteLine(objException);
            }
        }

        public static void ModifyMooegeINI()
        {
            try
            {
                IConfigSource source = new IniConfigSource(Compile.mooegeINI);
                string fileName = source.Configs["Storage"].Get("MPQRoot");
                if (fileName.Contains("${Root}"))
                {
                    Console.WriteLine("修改Mooege MPQ存储文件夹......");
                    IConfig config = source.Configs["Storage"];
                    config.Set("MPQRoot", Program.programPath + @"\MPQ");
                    source.Save();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("修改Mooege MPQ存储文件夹完成");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //The problem is, while passing args to ExecuteCommandSync(String command)
                //If the argument its too long due to the current mooege folder path (Program.programPath)
                //msbuild.exe won't be able to recieve the complete arguments and compiling will fail.
                Console.WriteLine("\n长路径错误：不能编译Mooege来源，"
                                  + "\n请使用较短的文件夹的路径移动MadCow文件到（例如： C:/MadCow/）");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n按任意键退出......");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static public void WriteVbsPath()
        {
            String vbsPath = (Program.programPath + "\\Tools\\ShortcutCreator.vbs");
            StreamReader reader = new StreamReader(vbsPath);
            string content = reader.ReadToEnd();
            reader.Close();
 
            content = Regex.Replace(content, "MODIFY", Compile.currentMooegeExePath);
            content = Regex.Replace(content, "WESKO", Compile.currentMooegeDebudFolderPath);
            StreamWriter writer = new StreamWriter(vbsPath);
            writer.Write(content);
            writer.Close();

            //建立快捷键
            if (File.Exists(Program.desktopPath + "\\Mooege.lnk"))
            {
                File.Delete(Program.desktopPath + "\\Mooege.lnk");
                System.Diagnostics.Process.Start(Program.programPath + "\\Tools\\ShortcutCreator.vbs");
            }
            else
                System.Diagnostics.Process.Start(Program.programPath + "\\Tools\\ShortcutCreator.vbs");
        }
    }
}
