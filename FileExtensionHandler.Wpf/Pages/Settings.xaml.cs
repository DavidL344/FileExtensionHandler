using FileExtensionHandler.Core;
using FileExtensionHandler.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        private readonly AppRegistry AppRegistry = new AppRegistry(Vars.AssemblyLocation);
        public Settings()
        {
            InitializeComponent();
            RefreshRegistryCheckBox();
        }

        private void ChangeRegistryState(object sender, RoutedEventArgs e)
        {
            chk_regProtocol.IsEnabled = false;
            try
            {
                if (!AppRegistry.IsProtocolRegistered)
                {
                    AppRegistry.RegisterProtocol();
                    return;
                }
                AppRegistry.UnregisterProtocol();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (ex is Win32Exception) ? (ex as Win32Exception).NativeErrorCode : ex.HResult;
                if (errorCode != 1223) MessageBox.Show(String.Format("An unknown error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message), "Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                RefreshRegistryCheckBox();
                chk_regProtocol.IsEnabled = true;
            }
        }

        private void RefreshRegistryCheckBox()
        {
            chk_regProtocol.IsChecked = AppRegistry.IsProtocolRegistered;
            chk_regProtocol.Content = (bool)chk_regProtocol.IsChecked ? "Registered the app's file protocol" : "Register the app's file protocol";
        }
    }
}
