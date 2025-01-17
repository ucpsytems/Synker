﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

// Using: https://github.com/ucpsytems/UniversalConfig
using UniversalConfig;

namespace Synker
{
    public static class Config
    {
        public static string Name { get => "Synker";  }
        public static string BackupTemp { get => "synker_temp";  }
        public static string BackupName { get => "SynkerBackups";  }
        public static string Path { get => s_LoadedPath + "\\Synker\\"; }
        public static string PrePath { get => s_LoadedPath; }
        public static int RetryDelay { get => i_RetryDelay; }
        public static int Retries { get => i_Retries; }
        public static bool ShowLog { get => b_Showlog; }
        public static bool AskForConsent { get => b_AskForConsent; }


        private static int i_RetryDelay = 0;
        private static int i_Retries = 0;
        private static string s_LoadedPath = "";
        private static bool b_Showlog = false;
        private static bool b_AskForConsent = false;

        public static bool CreateConfig(string s_Path)
        {
            try
            {
                using (UniversalConfigCreator o_Creator = new UniversalConfigCreator(Application.StartupPath + "\\config.ucf"))
                {
                    o_Creator.AppendUnit("settings");
                    o_Creator.AppendRegister("settings", "path", typeof(string));
                    o_Creator.AppendRegister("settings", "retrydelay", typeof(int));
                    o_Creator.AppendRegister("settings", "retries", typeof(int));
                    o_Creator.AppendRegister("settings", "showlog", typeof(bool));
                    o_Creator.AppendRegister("settings", "askforconsent", typeof(bool));
                    o_Creator.Build();
                }

                using (UniversalConfigReader o_Writer = new UniversalConfigReader(Application.StartupPath + "\\config.ucf"))
                {
                    o_Writer.SetValue<string>("settings", "path", s_Path);
                    o_Writer.SetValue<int>("settings", "retrydelay", 500);
                    o_Writer.SetValue<int>("settings", "retries", 3);
                    o_Writer.SetValue<bool>("settings", "showlog", true);
                    o_Writer.SetValue<bool>("settings", "askforconsent", true);

                    o_Writer.SaveConfig();
                }
            }
            catch (Exception)
            {

                return false;
            }

            return ReadConfig();
        } 
        public static bool ReadConfig()
        {
            if (!File.Exists(Application.StartupPath + "\\config.ucf")) return false;
            try
            {
                using (UniversalConfigReader o_Writer = new UniversalConfigReader(Application.StartupPath + "\\config.ucf"))
                {
                    s_LoadedPath = o_Writer.GetRawValue("settings", "path", typeof(string));
                    i_RetryDelay = o_Writer.GetValue<int>("settings", "retrydelay");
                    i_Retries = o_Writer.GetValue<int>("settings", "retries");
                    b_Showlog = o_Writer.GetValue<bool>("settings", "showlog");
                    b_AskForConsent = o_Writer.GetValue<bool>("settings", "askforconsent");
                }
            }
            catch (Exception)
            {
                return false;
            }

            if (i_Retries <= 0 || i_RetryDelay <= 0)
            {
                File.Delete(Application.StartupPath + "\\config.ucf");
                return false;
            }
            return true;
        } 
    }
}
