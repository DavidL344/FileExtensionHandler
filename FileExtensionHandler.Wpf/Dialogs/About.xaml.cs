using FileExtensionHandler.Model;
using FileExtensionHandler.Shared;
using ModernWpf.Controls;
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

namespace FileExtensionHandler.Dialogs
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            foreach (License license in LicenseParser.LicenseInformation)
            {
                string licenseData = await LicenseParser.LoadLicense(license, (int)(this.ActualWidth / 10));
                if (licenseData == null) continue;
                LicenseParser.AddEntry(tc_licenseInfo, license, licenseData);
            }
            pr_status.IsActive = false;
            tc_licenseInfo.Visibility = Visibility.Visible;
            IsPrimaryButtonEnabled = true;
            IsSecondaryButtonEnabled = true;
        }

        private void OpenRepo(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            LicenseParser.OpenInBrowser(tc_licenseInfo, LicenseParser.URL.Repository);
        }

        private void OpenLicense(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            LicenseParser.OpenInBrowser(tc_licenseInfo, LicenseParser.URL.License);
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = args.Result == ContentDialogResult.Primary || args.Result == ContentDialogResult.Secondary;
        }
    }
}
