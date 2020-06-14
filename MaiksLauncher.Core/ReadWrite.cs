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
        public static string ReadAToken()
        {
            string aToken = "";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
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

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
            if (File.Exists(path + @"config.mcf") == false)
            {
                Directory.CreateDirectory(path);
                using (StreamWriter sw = File.CreateText(path + @"config.mcf"))
                {
                    sw.WriteLine("maxRamMB=");
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
                { for (int i = 1; i < ConfigIndex && ConfigIndex < 10; i++) { inputFile.ReadLine(); } Setting = inputFile.ReadLine(); }

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
                Setting = new String(processedChars);
                ifFindEqualSign = false;

            }
            catch (IOException e) { Setting = "Error: " + e.Message; }

            Console.ReadLine();
            return Setting;
        }
        // i forgot about sessions lol
        public static void WriteAToken(string aToken ,bool isEncrypted)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
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
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
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
    }
    }
