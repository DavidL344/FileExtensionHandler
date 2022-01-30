using FileExtensionHandler.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileExtensionHandler.Shared
{
    internal class LicenseParser
    {
        internal enum URL
        {
            Repository,
            License
        }

        internal static readonly List<License> LicenseInformation = new List<License>()
        {
            new License
            {
                Name = Vars.AssemblyTitle,
                RepositoryURL = "https://github.com/DavidL344/FileExtensionHandler/",
                LicenseType = "MIT",
                LicenseURL = "https://raw.githubusercontent.com/DavidL344/FileExtensionHandler/master/FileExtensionHandler.Wpf/LICENSE",
                LicenseResource = Properties.Resources.LICENSE
            },
            new License
            {
                Name = "ModernWpfUI",
                RepositoryURL = "https://github.com/Kinnara/ModernWpf",
                LicenseType = "MIT",
                LicenseURL = "https://raw.githubusercontent.com/Kinnara/ModernWpf/master/LICENSE",
                LicenseResource = Properties.Resources.LICENSE_ModernWpfUI
            },
            new License
            {
                Name = "Newtonsoft.Json",
                RepositoryURL = "https://github.com/JamesNK/Newtonsoft.Json",
                LicenseType = "MIT",
                LicenseURL = "https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md",
                LicenseResource = Properties.Resources.LICENSE_Newtonsoft_Json
            },
            new License
            {
                Name = "CommandLineParser",
                RepositoryURL = "https://github.com/commandlineparser/commandline",
                LicenseType = "MIT",
                LicenseURL = "https://raw.githubusercontent.com/commandlineparser/commandline/master/License.md",
                LicenseResource = Properties.Resources.LICENSE_CommandLineParser
            }
        };

        internal static void AddEntry(TabControl tabControl, License license, string licenseData)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = license.Name;

            TextBox textBox = new TextBox
            {
                Text = licenseData,
                VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                TextWrapping = TextWrapping.Wrap,
                IsReadOnly = true
            };

            Grid grid = new Grid();
            grid.Margin = new Thickness(10);
            grid.Children.Add(textBox);

            tabItem.Content = grid;
            tabControl.Items.Add(tabItem);
        }

        private static async Task LoadLicenseInformation(TabControl tabControl, int separatorLength = 135)
        {
            foreach (License license in LicenseInformation)
            {
                string licenseData = await LoadLicense(license, separatorLength);
                if (licenseData == null) continue;

                TabItem tabItem = new TabItem();
                tabItem.Header = license.Name;

                TextBox textBox = new TextBox
                {
                    Text = licenseData,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true
                };

                Grid grid = new Grid();
                grid.Margin = new Thickness(10);
                grid.Children.Add(textBox);

                tabItem.Content = grid;
                tabControl.Items.Add(tabItem);
            }
        }

        internal static void OpenInBrowser(TabControl tabControl, URL urlType)
        {
            License license = GetCurrentSelection(tabControl);
            string url = urlType == URL.License ? license.LicenseURL : license.RepositoryURL;
            Process.Start(url);
        }

        internal static License GetCurrentSelection(TabControl tabControl)
        {
            string header = (string)((TabItem)tabControl.SelectedItem).Header;
            return LicenseInformation.Find(item => item.Name == header);
        }

        internal static string GetSeparator(int separatorLength = 125)
        {
            string separator = "";
            for (int i = 0; i < separatorLength; i++) separator += "-";
            return separator;
        }

        internal static async Task<string> LoadLicense(License license, int separatorLength)
        {
            string licenseData = $"This component is available under the following license: {license.LicenseType}\r\n{GetSeparator(separatorLength)}\r\n\r\n";
            try
            {
                using (HttpClient webClient = new HttpClient())
                {
                    licenseData += await webClient.GetStringAsync(new Uri(license.LicenseURL));
                }
            }
            catch (Exception e)
            {
                licenseData += $"Unable to load the online license information, using the offline version instead.\r\nError message: {e.Message}\r\n{GetSeparator(separatorLength)}\r\n\r\n";
                if (license.LicenseResource == null)
                {
                    licenseData += "Unable to load the offline license information.";
                    return licenseData;
                }

                using (MemoryStream stream = new MemoryStream(license.LicenseResource))
                using (StreamReader reader = new StreamReader(stream))
                    licenseData += reader.ReadToEnd();
            }
            return licenseData;
        }
    }
}
