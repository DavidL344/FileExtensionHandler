using FileExtensionHandler.Model;
using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExtensionHandler.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            foreach (License license in LicenseParser.LicenseInformation)
            {
                string licenseData = await LicenseParser.LoadLicense(license, (int)(this.ActualWidth / 6));
                if (licenseData == null) continue;
                LicenseParser.AddEntry(tc_licenseInfo, license, licenseData);
            }
            pr_status.IsActive = false;
            tc_licenseInfo.Visibility = Visibility.Visible;
            ssp_buttons.Visibility = Visibility.Visible;
        }

        private void OpenRepo(object sender, RoutedEventArgs e)
        {
            LicenseParser.OpenInBrowser(tc_licenseInfo, LicenseParser.URL.Repository);
        }

        private void OpenLicense(object sender, RoutedEventArgs e)
        {
            LicenseParser.OpenInBrowser(tc_licenseInfo, LicenseParser.URL.License);
        }
    }
}
