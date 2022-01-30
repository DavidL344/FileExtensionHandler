using FileExtensionHandler.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string AppTitle => $"{Vars.AssemblyTitle} {Vars.AssemblyVersionShort}";
        public Settings()
        {
            InitializeComponent();
        }

        private void OpenProtocolMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            // Can't use due to multiple dialogs being present at the same time
            new Dialogs.Settings.ProtocolRegistration().ShowAsync();
        }
    }
}
