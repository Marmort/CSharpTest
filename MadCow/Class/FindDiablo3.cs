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


namespace MadCow
{
    public class FindDiablo3
    {
        public static String FindDiabloLocation()
        {
            RegistryKey d3Path = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\");
            String[] nameList = d3Path.GetSubKeyNames();
            for (int i = 0; i < nameList.Length; i++)
            {
                RegistryKey regKey = d3Path.OpenSubKey(nameList[i]);
                try
                {
                    if (regKey.GetValue("DisplayName").ToString() == "Diablo III Beta")
                    {
                        return regKey.GetValue("InstallLocation").ToString();
                    }
                }
                catch {}
            }
            Console.WriteLine("暗黑3未安装" + "\n请安装Diablo III Beta再运行MadCow程序");
            Console.ReadLine();
            Environment.Exit(0);
            return "";
        }
    }
}
