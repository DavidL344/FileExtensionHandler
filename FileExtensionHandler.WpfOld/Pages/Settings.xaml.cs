using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Dialogs;
using FileExtensionHandler.Shared;
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
        public Settings()
        {
            InitializeComponent();
            RefreshRegistryCheckBox();
        }

        private async void ChangeRegistryState(object sender, RoutedEventArgs e)
        {
            chk_regProtocol.IsEnabled = false;
            try
            {
                if (!ProtocolController.IsRegistered(Vars.Protocol, Vars.AssemblyLocation))
                {
                    if (await ConfirmRegistration()) ProtocolController.Register(Vars.Protocol, Vars.AssemblyLocation);
                    return;
                }
                ProtocolController.Unregister(Vars.Protocol);
            }
            catch (Exception ex)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (ex is Win32Exception) ? (ex as Win32Exception).NativeErrorCode : ex.HResult;
                if (errorCode != 1223)
                {
                    if (Debugger.IsAttached) throw;
                    MessageBox.Show(String.Format("An unknown error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message), "Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            finally
            {
                RefreshRegistryCheckBox();
                chk_regProtocol.IsEnabled = true;
            }
        }

        private void RefreshRegistryCheckBox()
        {
            chk_regProtocol.IsChecked = ProtocolController.IsRegistered(Vars.Protocol, Vars.AssemblyLocation);
            chk_regProtocol.Content = (bool)chk_regProtocol.IsChecked ? "Registered the app's file protocol" : "Register the app's file protocol";
        }

        private async Task<bool> ConfirmRegistration()
        {
            AppProtocolConfirmation confirmation = new AppProtocolConfirmation(AppProtocolConfirmation.ProtocolAction.Register);
            await confirmation.ShowAsync();
            return confirmation.Continue;
        }
    }
}
