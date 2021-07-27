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
        internal Model.MultipleFiles.Parser Parser;
        internal Model.MultipleFiles.Handler Handler;

        private void OnStart(object sender, StartupEventArgs e)
        {
            
            Args = e.Args;
            if (Args.Length > 0)
            {
                try
                {
                    Parser = new Model.MultipleFiles.Parser(Args);
                    Handler = new Model.MultipleFiles.Handler(Parser.FilePath);
                    Handler.ParseFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            MainWindow window = new MainWindow(Handler)
            {
                Title = ""
            };
            window.ShowDialog();
        }
    }
}
