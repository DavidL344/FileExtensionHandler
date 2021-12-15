using FileExtensionHandler.Shared;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
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
        private static readonly string Protocol = "fexth";
        private readonly string ProtocolFullPath = $@"{Protocol}\shell\open\command";
        private bool IsAdministrator => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        private bool IsProtocolRegistered
        {
            get
            {
                try
                {
                    bool isRegistered = true;

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(Protocol))
                    {
                        if (key == null)
                        {
                            isRegistered = false;
                        }
                        else
                        {
                            if ((string)key.GetValue("URL Protocol") != "") isRegistered = false;
                        }
                    }

                    using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(ProtocolFullPath))
                    {
                        if (key == null)
                        {
                            isRegistered = false;
                        }
                        else
                        {
                            string regPath = (string)key.GetValue("");
                            string[] delimiter = new string[] { " %1" };
                            string[] quotesDelimiter = new string[] { "\"" };
                            regPath = regPath.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)[0].Split(quotesDelimiter, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (regPath != Vars.AssemblyLocation) isRegistered = false;
                        }
                    }
                        

                    return isRegistered;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

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
                if (!IsProtocolRegistered)
                {
                    FexthProtocolAdd();
                    return;
                }
                FexthProtocolRemove();
            }
            catch (Exception ex)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (ex is Win32Exception) ? (ex as Win32Exception).NativeErrorCode : ex.HResult;
                if (errorCode != 1223) MessageBox.Show(String.Format("An unknown error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message));
            }
            finally
            {
                RefreshRegistryCheckBox();
                chk_regProtocol.IsEnabled = true;
            }
        }

        private void FexthProtocolAdd()
        {
            if (IsProtocolRegistered) return;
            string regCommand = $"\"{Vars.AssemblyLocation}\" %1";
            string regCommandCmd = $"\\\"{Vars.AssemblyLocation}\\\" %1";

            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg add \"HKCR\\{Protocol}\" /v \"URL Protocol\" /t REG_SZ /f" +
                    $"&&reg add \"HKCR\\{ProtocolFullPath}\" /ve /d \"{regCommandCmd}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }
            RegistryKey fexth_root = Registry.ClassesRoot.CreateSubKey(Protocol);
            fexth_root.SetValue("URL Protocol", "");

            RegistryKey fexth_command = Registry.ClassesRoot.CreateSubKey(ProtocolFullPath);
            fexth_command.SetValue("", regCommand);
        }

        private void FexthProtocolRemove()
        {
            if (!IsProtocolRegistered) return;
            if (!IsAdministrator)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c reg delete \"HKCR\\{Protocol}\" /f",
                    Verb = "runas",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return;
            }
            Registry.ClassesRoot.DeleteSubKeyTree(Protocol);
        }

        private void RefreshRegistryCheckBox()
        {
            chk_regProtocol.IsChecked = IsProtocolRegistered;
            chk_regProtocol.Content = (bool)chk_regProtocol.IsChecked ? "Registered the app's file protocol" : "Register the app's file protocol";
        }
    }
}
