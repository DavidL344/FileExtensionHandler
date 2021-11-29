using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
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
        private readonly bool Debug = true;

        private void OnStart(object sender, StartupEventArgs e)
        {
            Args = Debug ? new string[] { Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\fexth_stuff\fexth.mp3.reg") } : e.Args;
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
                    MessageBox.Show(String.Format("A fatal error has occured.\r\nException type: {0}\r\nException Description: {1}", e.GetType(), ex.Message), "Fatal Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
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
