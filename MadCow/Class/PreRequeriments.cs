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
using System.Runtime.InteropServices;
using Nini.Config;

namespace MadCow
{
    class PreRequeriments
    {
        public static void FirstRunConfiguration()
        {
            IConfigSource iniSource = new IniConfigSource(Compile.madcowINI);
            string getValues = iniSource.Configs["Settings"].Get("firstrun");
            IConfig config = iniSource.Configs["Settings"];

            if (getValues.Contains("0"))
            {
                Console.WriteLine("你有暗黑3安装在这台电脑吗? (yes/no)");
                string input = Console.ReadLine();
                switch (input.ToLower())
                {
                    case "yes":
                            config.Set("d3installed", 1);
                            config.Set("firstrun", 1);
                            iniSource.Save();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("保持第一次运行配置文件完成");
                            Console.ForegroundColor = ConsoleColor.White;
                        MadCowRunProcedure.RunMadCow(1); //运行D3安装模式
                        break;
                    case "no":
                        config.Set("firstrun", 1);
                        iniSource.Save();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("保持第一次运行配置文件完成");
                        Console.ForegroundColor = ConsoleColor.White;
                        MadCowRunProcedure.RunMadCow(0); //没有D3运行安装模式
                        break;
                    default:
                        Console.WriteLine("选择 yes 或 no");
                        break;
                }
            }
            else
            {
                string D3InstallStatus = iniSource.Configs["Settings"].Get("firstrun");
                int StatusValue = Convert.ToInt32(D3InstallStatus);
                MadCowRunProcedure.RunMadCow(StatusValue);
            }
        }

        public static void CheckPrerequeriments()
        {
            //Boolean checkSqlite = LoadSQLLiteAssembly();
            String checkNet4 = Environment.Version.ToString();

            /*if (checkSqlite == true)
            {
                Console.WriteLine("发现： System.Data.SQLite");
            }
            else
            {
                Console.WriteLine("不存在：System.Data.SQLite");
                Console.ReadKey();
                Environment.Exit(0);
            }*/

            if (checkNet4.StartsWith("4"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("发现：.NET Framework " + checkNet4);
                Console.ForegroundColor = ConsoleColor.White;
            }

            else
            {
                Console.WriteLine("请更新：.NET Framework to" + " version 4!");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static Boolean LoadSQLLiteAssembly()
        {
     
         string response;
         var exist = AssemblyExist("System.Data.SQLite", out response);
         return exist;
            
        }

        private static bool AssemblyExist(string assemblyname, out string response)
        {
            try
            {
                response = QueryAssemblyInfo(assemblyname);
                return true;
            }
            catch (System.IO.FileNotFoundException e)
            {
                response = e.Message;
                return false;
            }
        }

        private static String QueryAssemblyInfo(string assemblyName)
        {
            var assembyInfo = new AssemblyInfo {cchBuf = 512};
            assembyInfo.currentAssemblyPath = new String('C', assembyInfo.cchBuf);

            IAssemblyCache assemblyCache;

            // 得到IAssemblyCache指针
            var hr = GacApi.CreateAssemblyCache(out assemblyCache, 0);
            if (hr == IntPtr.Zero)
            {
                hr = assemblyCache.QueryAssemblyInfo(1, assemblyName, ref assembyInfo);
                if (hr != IntPtr.Zero)
                {
                    Marshal.ThrowExceptionForHR(hr.ToInt32());
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR(hr.ToInt32());
            }
            return assembyInfo.currentAssemblyPath;
        }
    }

    internal class GacApi
    {
        [DllImport("fusion.dll")]
        internal static extern IntPtr CreateAssemblyCache(
            out IAssemblyCache ppAsmCache, int reserved);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
    internal interface IAssemblyCache
    {
        int Dummy1();
        [PreserveSig()]
        IntPtr QueryAssemblyInfo(
            int flags,
            [MarshalAs(UnmanagedType.LPWStr)]
            String assemblyName,
            ref AssemblyInfo assemblyInfo);

        int Dummy2();
        int Dummy3();
        int Dummy4();
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct AssemblyInfo
    {
        public int cbAssemblyInfo;
        public int assemblyFlags;
        public long assemblySizeInKB;

        [MarshalAs(UnmanagedType.LPWStr)]
        public String currentAssemblyPath;

        public int cchBuf;
    }
}
