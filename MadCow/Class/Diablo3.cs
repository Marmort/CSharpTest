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
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace MadCow
{
    public class Diablo3
    {
        public static String _d3loc;

        public static void FindDiablo3()
        {
            RegistryKey d3Path = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\");

            if (d3Path == null) //D3未发现
            {
                Console.WriteLine("暗黑3未安装" + "\n请安装Diablo III Beta再运行MadCow程序");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                String[] nameList = d3Path.GetSubKeyNames();
                for (int i = 0; i < nameList.Length; i++)
                {
                    RegistryKey regKey = d3Path.OpenSubKey(nameList[i]);
                    try
                    {
                        if (regKey.GetValue("DisplayName").ToString() == "Diablo III Beta")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("发现暗黑3安装路径");
                            _d3loc = regKey.GetValue("InstallLocation").ToString();
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    catch
                    {
                        //the ball?
                    }
                }
            }
        }

        public static void VerifyVersion()
        {
            FileVersionInfo.GetVersionInfo(Path.Combine(_d3loc, "Diablo III.exe"));
            FileVersionInfo d3Version = FileVersionInfo.GetVersionInfo(_d3loc + "\\Diablo III.exe");

            try
            {
                WebClient client = new WebClient();
                String commitFile = client.DownloadString("https://raw.github.com/mooege/mooege/master/src/Mooege/Common/Versions/VersionInfo.cs");
                Int32 ParsePointer = commitFile.IndexOf("RequiredPatchVersion = ");
                String revision = commitFile.Substring(ParsePointer + 23, 4); //通过Mooege得到所需版本
                int CurrentD3Version = Convert.ToInt32(revision);
                int MooegeD3needs = d3Version.FilePrivatePart;

                if (MooegeD3needs == CurrentD3Version)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("找到正确支持暗黑3的Mooege版本 [" + CurrentD3Version + "]");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mooege需要的暗黑3 版本 [" + revision + "]" + "\n升级或降级暗黑3的版本 [" + MooegeD3needs + "]."
                        + "\n得到正确的版本后尝试MadCow的使用");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadKey();
                    Console.WriteLine("\n请按任意键退出......"); //如果当前的MadCow忘了完成用户d3的版本不满足Mooege要求的版本
                    Environment.Exit(0); //使用这个关闭MadCow(不确定什么正确的方法来终结这一计划）
                }
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.ToString());
                if (webEx.Status == WebExceptionStatus.ConnectFailure)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("错误：确保你的互联网已连接");
                }
            }
        }
    }
}
