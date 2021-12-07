using FileExtensionHandler.Core;
using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            CheckSamples();
        }

        private void CheckSamples()
        {
            bool freshInstallation = false;
            while (!freshInstallation)
            {
                if (!Directory.Exists(Vars.DefaultSaveLocation)) freshInstallation = true;
                if (!Directory.Exists(Vars.Dir_FileExtensions)) freshInstallation = true;
                if (!Directory.Exists(Vars.Dir_Associations)) freshInstallation = true;

                if (Directory.Exists(Vars.Dir_FileExtensions))
                    if (!Directory.EnumerateFiles(Vars.Dir_FileExtensions).Any()) freshInstallation = true;

                if (Directory.Exists(Vars.Dir_Associations))
                    if (!Directory.EnumerateFiles(Vars.Dir_Associations).Any()) freshInstallation = true;
                break;
            }

            if (freshInstallation)
            {
                txt_instructions.Text = "Click the button below to generate a few associations for the app:";
                btn_openAppDir.Visibility = Visibility.Collapsed;
                btn_newAssociations.Visibility = Visibility.Visible;
            }
            else
            {
                txt_instructions.Text = "Call this app with a parameter to get started.";
                btn_openAppDir.Visibility = Visibility.Visible;
                btn_newAssociations.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveSamplesToDisk(object sender, RoutedEventArgs e)
        {
            Samples.SaveToDisk(Samples.FileExtensions, Vars.Dir_FileExtensions);
            Samples.SaveToDisk(Samples.Associations, Vars.Dir_Associations);
            CheckSamples();
        }

        private void OpenAppDataDir(object sender, RoutedEventArgs e)
        {
            CheckSamples();
            if (Directory.Exists(Vars.DefaultSaveLocation)) Process.Start(Vars.DefaultSaveLocation);
        }
    }
}
