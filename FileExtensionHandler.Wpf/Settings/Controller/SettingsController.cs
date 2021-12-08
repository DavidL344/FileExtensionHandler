using FileExtensionHandler.Shared;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Settings.Controller
{
    internal class SettingsController
    {
        internal Model.Settings Settings;
        internal string ChecksumCache;
        internal string Checksum => ChecksumController.GetHash(JsonConvert.SerializeObject(Settings, Formatting.Indented));
        internal bool ChecksumUpdated => ChecksumController.VerifyHash(ChecksumCache, Checksum);
        internal string SettingsPath {
            get
            {
                string settingsPath = $@"{Vars.DefaultSaveLocation}\settings.json";
                string settingsDir = Path.GetDirectoryName(settingsPath);

                if (!Directory.Exists(settingsDir)) Directory.CreateDirectory(settingsDir);
                if (!File.Exists(settingsPath))
                {
                    File.Create(settingsPath).Close();
                    SetDefaults();
                    Save();
                }
                return settingsPath;
            }
            
        }
        internal SettingsController()
        {
            string jsonData = File.ReadAllText(SettingsPath);
            Settings = JsonConvert.DeserializeObject<Model.Settings>(jsonData);
            ChecksumCache = Checksum;
        }

        internal void Save()
        {
            string jsonData = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(SettingsPath, jsonData);
            ChecksumCache = Checksum;
        }

        internal void SetDefaults()
        {
            Settings = new Model.Settings
            {
                GUI = new Model.GUI
                {
                    Navbar_PaneDisplayMode = NavigationViewPaneDisplayMode.Top
                }
            };
        }
    }
}
