using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Wpf.Shared;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace FileExtensionHandler.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args { get; private set; } = Array.Empty<string>();

        private void Main(object sender, StartupEventArgs e)
        {
            try
            {
                Args = e.Args;
                if (Args.Length > 0)
                {
                    FileInformationController fileInformationController = new(Vars.DefaultOptions);
                    FileInformation fileInformation = fileInformationController.Parse(Args);

                    if (!fileInformation.AppProtocolUsed && fileInformation.Associations.Count < 2)
                    {
                        fileInformationController.OpenFile(fileInformation, fileInformation.DefaultAssociationIndex);
                        return;
                    }

                    new MainWindow(/*"AppPicker", fileInformation*/)
                    {
                        Title = ""
                    }.ShowDialog();
                }
                else
                {
                    new MainWindow().ShowDialog();
                }
                Close();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (ex is Win32Exception) ? (ex as Win32Exception ?? new Win32Exception()).NativeErrorCode : ex.HResult;
                if (errorCode != 1223) MessageBox.Show(String.Format("A fatal error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message), "Fatal Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        public static void Close()
        {
            if (Application.Current != null) Application.Current.Shutdown();
        }
    }
}
