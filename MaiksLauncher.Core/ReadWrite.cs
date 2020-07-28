using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MaiksLauncher.Core
{
    public class ReadWrite
    {
        private static readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
        public static string ReadAToken()
        {
            string aToken = "";
             
            try
            {
                StreamReader sr = new StreamReader(path + @"ATokenNonEncrypted");
                aToken = sr.ReadToEnd();
            }
            catch (IOException e)
            {
                aToken = "FileNoExist";
            }

            return aToken;
        }
        public static string ReadEncryptedAToken(string decryptPass)
        {
             
            string ATokenDecrypted = "";
            try
            {
                try
                {
                    StreamReader sr = new StreamReader(path + @"ATokenEncrypted.txt");
                    string ATokenEncrypted = sr.ReadToEnd();
                    ATokenDecrypted = EncryptorDecryptor.Decrypt(ATokenEncrypted, decryptPass);
                }
                catch (IOException e)
                {
                    ATokenDecrypted = "IOError";
                }
            }
            catch(Exception e)
            {
                ATokenDecrypted = "Invalid file";

            }
            return ATokenDecrypted;
        }
        public static string ReadConfig(int ConfigIndex)
        {

             
            if (File.Exists(path + @"config.mcf") == false)
            {
                Directory.CreateDirectory(path);
                using (StreamWriter sw = File.CreateText(path + @"config.mcf"))
                {
                    sw.WriteLine("=null");
                    sw.WriteLine("maxRamMB=1024");
                    sw.WriteLine("defaultVersion=");
                    sw.WriteLine("specialPath=");
                    sw.WriteLine("launchArgs=");
                    sw.WriteLine("defaultUser=");
                    sw.WriteLine("saveAccount=");
                    sw.WriteLine("themeForeground=");
                    sw.WriteLine("themeBackground=");
                    sw.WriteLine("defaultServer=");
                    sw.WriteLine("customResWidth=");
                    sw.WriteLine("customResHeight=");
                    sw.WriteLine("PLEASE DO NOT MESS WITH THIS FILE!");
                }
            }
            string Setting;
            try
            {

                using (StreamReader inputFile = new StreamReader(path + @"config.mcf"))
                { for (int i = 1; i < ConfigIndex && ConfigIndex < 14; i++) { inputFile.ReadLine(); } Setting = inputFile.ReadLine(); }

                bool ifFindEqualSign = false;
                char[] unprocessedStringChars = Setting.ToCharArray();
                char[] processedChars = new char[1024];
                int e = 0;
                foreach (char currChar in unprocessedStringChars)
                {
                    if (currChar == '=') { ifFindEqualSign = true; }
                    if (ifFindEqualSign == true) { processedChars[e] = currChar; e++; }
                }
                processedChars = processedChars.Skip(1).ToArray();
                bool ifFindSpace = false;
                if (ConfigIndex != 4)
                {
                    foreach (char currChar in processedChars)
                
                    {
                        if (currChar == ' ') { ifFindSpace = true; }
                        if (ifFindSpace == true) {break; }
                    }
                }

                var pc = processedChars.Take(e);
                Setting = new string(pc.ToArray());
                ifFindEqualSign = false;
            }

            catch (IOException e) { Setting = "Error: " + e.Message; }
            return Setting.Remove(Setting.Length - 1, 1);
        }

        // i forgot about sessions lol
        public static void WriteAToken(string aToken ,bool isEncrypted)
        {
             
            if (File.Exists(path + @"ATokenNonEncrypted.mat") == false || Directory.Exists(path) == false && isEncrypted == false)
            {
                Directory.CreateDirectory(path);
                File.CreateText(path + @"ATokenNonEncrypted.mat");
            }
            File.WriteAllText(path + @"ATokenNonEncrypted.mat", String.Empty);
            TextWriter tw = new StreamWriter(path + @"ATokenNonEncrypted.mat", true);
            tw.WriteLine(aToken);
            tw.Close();
        }
        public static void WriteAToken(string aToken, bool isEncrypted, string pass)
        {
             
            if (File.Exists(path + @"ATokenEncrypted.mat") == false || Directory.Exists(path) == false && isEncrypted == true)
            {
                Directory.CreateDirectory(path);
                File.CreateText(path + @"ATokenEncrypted.mat");
            }
            File.WriteAllText(path + @"ATokenEncrypted.mat", String.Empty);
            TextWriter tw = new StreamWriter(path + @"ATokenEncrypted.mat", true);
            tw.WriteLine(EncryptorDecryptor.Encrypt(aToken,pass));
            tw.Close();
        }

        public static void WriteConfigByLine(string cfg, int index)
        {
             
            try
            {
                string[] AllSettings = File.ReadAllLines(path + @"config.mcf");
                string finalWrite = "";
                switch (index)
                {
                    case 1:
                        AllSettings[index] = "maxRamMB=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 2:
                        AllSettings[index] = "defaultVersion=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 3:
                        AllSettings[index] = "specialPath=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 4:
                        AllSettings[index] = "launchArgs=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 5:
                        AllSettings[index] = "defaultUser=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 6:
                        AllSettings[index] = "saveAccount=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 7:
                        AllSettings[index] = "themeForeground=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 8:
                        AllSettings[index] = "themeBackground=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 9:
                        AllSettings[index] = "defaultServer=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 10:
                        AllSettings[index] = "customResWidth=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                    case 11:
                        AllSettings[index] = "customResHeight=" + cfg;
                        File.WriteAllLines(path + @"config.mcf", AllSettings);
                        break;
                }
            }
            
            
            catch (Exception e)
            {
                cfg = e.Message;
            }
        }

        public static void SaveVersionList(string[] verListArray)
        {
             
            if (File.Exists(path + @"VersionList.mvl") == false)
            {
                Directory.CreateDirectory(path);
                File.CreateText(path + @"VersionList.mvl");
                StreamWriter sw = new StreamWriter(path + @"VersionList.mvl");
                foreach (string version in verListArray)
                {
                    sw.WriteLine(version);
                }
            }
        }

        public static string[] LoadVersionList()
        {
            
            if (File.Exists(path + @"VersionList.mvl"))
            {
                string[] vlist = new string[File.ReadAllLines(path + @"VersionList.mvl").Length];
                int index = 0;
                foreach (string line in File.ReadAllLines(path + @"VersionList.mvl"))
                {
                    vlist[index] = line;
                    index++;
                }
                return vlist;
            }
            else 
            {
                string[] a = { };
                return a;
            }
            
        }
        
        public static void WriteConfigV2(int ConfigIndex, string Config)
        {
            string ConfigLine = File.ReadAllLines(path + @"config.mcf")[ConfigIndex];

            foreach (char currentchar in ConfigLine)
            {
                
            }
        }
    }
}
