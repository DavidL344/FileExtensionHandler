using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FileExtensionHandler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] Args { get; private set; } = new string[] { };
        private readonly bool Debug = false;

        private void OnStart(object sender, StartupEventArgs e)
        {
            Args = Debug ? new string[] { Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\temp.mp3") } : e.Args;
            if (Args.Length > 0)
            {
                try
                {
                    Handler handler = new Handler(Args);
                    handler.Start();
                }
                catch (Exception ex)
                {
                    if (Debug) throw;
                    int errorCode = (ex is Win32Exception) ? (ex as Win32Exception).NativeErrorCode : ex.HResult;
                    
                    // Suppress the exception when the user cancels the UAC prompt
                    if (errorCode != 1223) MessageBox.Show(String.Format("A fatal error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message), "Fatal Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                MainWindow window = new MainWindow
                {
                    Title = ""
                };
                window.ShowDialog();
            }
            Application.Current.Shutdown();
        }
    }
}
