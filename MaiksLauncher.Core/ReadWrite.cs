using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
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
        public static string ReadConfigAtLine(int line)
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\";
            if (File.Exists(path + @"config.mcf") == false)
            {
                Directory.CreateDirectory(path);
                using (StreamWriter sw = File.CreateText(path + @"config.mcf"))
                {
                    sw.WriteLine("maxRamMB=");
                    sw.WriteLine("defaultVersion");
                    sw.WriteLine("specialPath=");
                    sw.WriteLine("launchArgs=");
                    sw.WriteLine("defaultUser=");
                    sw.WriteLine("saveAccount=");
                    sw.WriteLine("themeForeground=");
                    sw.WriteLine("themeBackground=");
                }
            }
            string Setting;
            try
            {

                using (StreamReader inputFile = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Config.mcf")))
                { for (int i = 1; i < line; i++) {inputFile.ReadLine();}  Setting = inputFile.ReadLine(); }
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
            if (File.Exists(path + @"ATokenEncrypted.txt") == false || Directory.Exists(path) == false && isEncrypted == true)
            {
                Directory.CreateDirectory(path);
                File.CreateText(path + @"ATokenEncrypted.txt");
            }
            File.WriteAllText(path, String.Empty);
            TextWriter tw = new StreamWriter(path + @"ATokenEncrypted.txt", true);
            tw.WriteLine(EncryptorDecryptor.Encrypt(aToken,pass));
            tw.Close();
        }

    }
    }
