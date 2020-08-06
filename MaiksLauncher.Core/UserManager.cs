using System;
using Newtonsoft.Json;
using System.IO;
using CmlLib.Core;
using System.Text;

namespace MaiksLauncher.Core
{
    class UserProfileManager
    {
        readonly string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MaiksLauncher\UserProfiles\";
        
      
        /// <summary>
        /// Write config to user, linked to UUID
        /// </summary>
        /// <param name="userIndex">User Index</param>
        /// <param name="ConfigIndex">Config index in config file</param>
        /// <param name="overwrite">Overwrite the selected config index</param>
        public void WriteCfg(int userIndex, int[] ConfigIndex, string[] overwrite)
        {

            if (!File.Exists(path +  userIndex.ToString() + @".uProf") || String.IsNullOrEmpty(File.ReadAllText(path + userIndex.ToString() + @".uProf"))) ;
            {
                User u = new User();
                Directory.CreateDirectory(path);
                StreamWriter sw1 = File.CreateText(path + userIndex.ToString() + @".uProf");
                sw1.Write(JsonConvert.SerializeObject(u));
                sw1.Close();
            }
            string jsonToDeserialize = File.ReadAllText(path + userIndex.ToString() + @".uProf");
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
            StreamWriter sw = new StreamWriter(path + userIndex.ToString() + @".uProf");
            sw.Write(result);
            sw.Close();
        }
        public void WriteCfg(int userIndex, int ConfigIndex, string overwrite)
        {
            if (!string.IsNullOrEmpty(overwrite) && ConfigIndex > 0 && userIndex > 0)
            {
                string jsonToDeserialize = File.ReadAllText(path + userIndex.ToString() + @".uProf");
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
        public MSession Login(User u, string ePassword)
        {
            
            if (u.Credentials.ifOnline)
            {
                MLogin mLogin = new MLogin();
                if (u.Credentials.ifEncrypts)
                {
                    if (!string.IsNullOrEmpty(ePassword)) return mLogin.Authenticate(EncryptorDecryptor.Decrypt(u.Credentials.Username, ePassword), EncryptorDecryptor.Decrypt(u.Credentials.Password, ePassword));
                    else return null;
                }
                else
                {
                    return mLogin.Authenticate(EncryptorDecryptor.Decrypt(u.Credentials.Username, "WW/=rUBVZ=sZu9}Y"), EncryptorDecryptor.Decrypt(u.Credentials.Password, "WW/=rUBVZ=sZu9}Y"));
                }
            }
            else
            {
                return MSession.GetOfflineSession(u.Credentials.Username);
            }
        }
        public User getRawUserInfo(int UserID)
        {
            if (File.Exists(path + UserID.ToString() + @".uProf"))
            {
                StreamReader reader = new StreamReader(path + UserID.ToString() + @".uProf");
                User u = JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
                reader.Close();
                return u;
            }
            else
            {
                throw new FileNotFoundException("User doesn't exist");
            }
            
        }
        public User getDecryptedUserInfo(int UserID, string password)
        {
            if (File.Exists(path + UserID.ToString() + @".uProf"))
            {
                StreamReader reader = new StreamReader(path + UserID.ToString() + @".uProf");
                User u = JsonConvert.DeserializeObject<User>(reader.ReadToEnd());
                reader.Close();
              
                string decryptedUsername = EncryptorDecryptor.Decrypt(u.Credentials.Username, password);
                string decryptedPassword = EncryptorDecryptor.Decrypt(u.Credentials.Password, password);
                u.Credentials.Username = decryptedUsername.Replace(u.Credentials.ESalt, "");
                u.Credentials.Password = decryptedPassword.Replace(u.Credentials.PSalt, "");
                return u;
            }
            else
            {
                throw new FileNotFoundException("User doesn't exist");
            }
        }
        private void saveUserInfo(User u, string ePassword)
        {
            if (ifUserExist(u.UserID))
            {
                StreamWriter writer = new StreamWriter(path + u.UserID.ToString() + @".uProf");
                File.WriteAllText(path + u.UserID.ToString() + @".uProf", "");
                writer.Write(JsonConvert.SerializeObject(u));
                writer.Close();
            }
            else
            {
                StreamWriter sw = File.CreateText(path + u.UserID.ToString() + @".uProf");
                sw.Write(JsonConvert.SerializeObject(u));
                sw.Close();
            }
        }
        private bool ifUserExist(int UserID) { return File.Exists(path + UserID.ToString() + @".uProf"); }
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
    
    public class UserCredentials
    {
        // username == email
        public string Username { get; set; }
        public string Password { get; set; }
        public bool ifEncrypts = false;
        public bool ifOnline = true;
        public string PSalt;
        public string ASalt;
        public string ESalt;
    }
    public class User
    {
        public string UName { get; set; }
        public int UserID { get; set; }
        public USettings uSettings = new USettings();
        public UserCredentials Credentials = new UserCredentials();


    }
}
