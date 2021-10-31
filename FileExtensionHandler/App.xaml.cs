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
        private readonly bool Debug = false;

        private void OnStart(object sender, StartupEventArgs e)
        {
            Args = Debug ? new string[] { Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\temp.mp3") } : e.Args;
            if (Args.Length > 0)
            {
                try
                {
                    Model.Shared.Arguments arguments = new Model.Shared.Arguments(Args);
                    Model.Shared.OpenedFile openedFile = new Model.Shared.OpenedFile(arguments.FilePath);

                    AppPicker window = new AppPicker(openedFile)
                    {
                        Title = ""
                    };
                    window.ShowDialog();
                }
                catch (Exception ex)
                {
                    if (Debug) throw;
                    MessageBox.Show(ex.Message, "Fatal Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
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
                //MessageBox.Show("Welcome to fexth!\r\nCall this app with a file as a parameter to get started.", "fexth", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Application.Current.Shutdown();
        }
    }
}
