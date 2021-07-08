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
        internal Model.Parser Parser;

        private void OnStart(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                Args = e.Args;
                Parser = new Model.Parser(Args[0]);
                Parser.ParseFile();
            }

            MainWindow window = new MainWindow(Parser)
            {
                Title = ""
            };
            window.ShowDialog();
        }
    }
}
