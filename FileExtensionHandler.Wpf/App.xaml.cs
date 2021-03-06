using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
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
        private readonly bool InjectArgs = false;
        private readonly string[] InjectedArgs = new string[] { Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\temp.mp3") };

        private void OnStart(object sender, StartupEventArgs e)
        {
            try
            {
                Args = InjectArgs ? InjectedArgs : e.Args;
                if (Args.Length > 0)
                {
                    Handler handler = new Handler(Args);
                    handler.Start();
                }
                else
                {
                    MainWindow window = new MainWindow();
                    window.ShowDialog();
                }
                this.Close();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (ex is Win32Exception) ? (ex as Win32Exception).NativeErrorCode : ex.HResult;
                if (errorCode != 1223) MessageBox.Show(String.Format("A fatal error has occured.\r\nException type: {0}\r\nException Description: {1}", ex.GetType(), ex.Message), "Fatal Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Close()
        {
            if (Application.Current != null) Application.Current.Shutdown();
        }
    }
}
