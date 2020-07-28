using System;
using Newtonsoft.Json;
using System.IO;

namespace MaiksLauncher.Core
{
    class UserProfileManager
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\UserProfilesJson\";
        /// <summary>
        /// Add a user, if user exists, throws an exeception.
        /// </summary>
        /// <param name="index">User's Index</param>
        /// <param name="UUID">User's unique ID</param>
        void AddUser(int index, string UUID)
        {
            if (File.Exists(path + @"uProf." + index.ToString()))
            {
                throw new Exception("User already exist");
            }
            else
            {
                User u = new User();
                u.UserID = index;
                StreamWriter sw = File.CreateText(path + @"uProf." + index.ToString());
                sw.Write(JsonConvert.SerializeObject(u));
                sw.Close();
            }
        }
        
        /// <summary>
        /// Write config to user, linked to UUID
        /// </summary>
        /// <param name="userIndex">User Index</param>
        /// <param name="ConfigIndex">Config index in config file</param>
        /// <param name="overwrite">Overwrite the selected config index</param>
        public void WriteCfg(int userIndex, int[] ConfigIndex, string[] overwrite)
        {

            if (!File.Exists(path + @"uProf." + userIndex.ToString()) || String.IsNullOrEmpty(File.ReadAllText(path + @"uProf." + userIndex.ToString()))) ;
            {
                User u = new User();
                Directory.CreateDirectory(path);
                StreamWriter sw1 = File.CreateText(path + @"uProf." + userIndex.ToString());
                sw1.Write(JsonConvert.SerializeObject(u));
                sw1.Close();
            }
            string jsonToDeserialize = File.ReadAllText(path + @"uProf." + userIndex.ToString());
            User user = JsonConvert.DeserializeObject<User>(jsonToDeserialize);
            if (ConfigIndex.Length > 0 && overwrite.Length == ConfigIndex.Length)
            {
                int i = 0;
                foreach (int index in ConfigIndex)
                {
                    switch (index)
                    {
                        case 1:
                            user.uSettings.maxRamMb = overwrite[i];
                            break;
                        case 2:
                            user.uSettings.defaultVersion = overwrite[i];
                            break;
                        case 3:
                            user.uSettings.specialPath = overwrite[i];
                            break;
                        case 4:
                            user.uSettings.launchArgs = overwrite[i];
                            break;
                        case 5:
                            user.uSettings.defaultServer = overwrite[i];
                            break;
                        case 6:
                            user.uSettings.customResW = overwrite[i];
                            break;
                        case 7:
                            user.uSettings.customResH = overwrite[i];
                            break;
                    }
                    i++;
                }
            }
            else throw new Exception("Invalid input");

            string result = JsonConvert.SerializeObject(user);
            StreamWriter sw = new StreamWriter(path + @"uProf." + userIndex.ToString());
            sw.Write(result);
            sw.Close();
        }
        public void WriteCfg(int userIndex, int ConfigIndex, string overwrite)
        {
            if (!string.IsNullOrEmpty(overwrite) && ConfigIndex > 0 && userIndex > 0)
            {
                string jsonToDeserialize = File.ReadAllText(path + @"uProf." + userIndex.ToString());
                User user = JsonConvert.DeserializeObject<User>(jsonToDeserialize);
                switch (ConfigIndex)
                {
                    case 1:
                        user.uSettings.maxRamMb = overwrite;
                        break;
                    case 2:
                        user.uSettings.defaultVersion = overwrite;
                        break;
                    case 3:
                        user.uSettings.specialPath = overwrite;
                        break;
                    case 4:
                        user.uSettings.launchArgs = overwrite;
                        break;
                    case 5:
                        user.uSettings.defaultServer = overwrite;
                        break;
                    case 6:
                        user.uSettings.customResW = overwrite;
                        break;
                    case 7:
                        user.uSettings.customResH = overwrite;
                        break;
                }
            }
        }
    }
    public class USettings
    {
        public string maxRamMb { get; set; }
        public string defaultVersion { get; set; }
        public string specialPath { get; set; }
        public string launchArgs { get; set; }
        public string defaultServer { get; set; }
        public string customResW { get; set; }
        public string customResH { get; set; }

    }
    public class User
    {
        public string UName { get; set; }
        public USettings uSettings = new USettings();
        public int UserID { get; set; }

    }
}
