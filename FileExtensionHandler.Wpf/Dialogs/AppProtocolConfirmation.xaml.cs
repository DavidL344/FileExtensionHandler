using System;
using System.Collections.Generic;
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

namespace FileExtensionHandler.Dialogs
{
    /// <summary>
    /// Interaction logic for AppProtocolConfirmation.xaml
    /// </summary>
    public partial class AppProtocolConfirmation
    {
        internal bool Continue { get; private set; } = false;
        public AppProtocolConfirmation()
        {
            InitializeComponent();
        }

        private void Confirm(ModernWpf.Controls.ContentDialog sender, ModernWpf.Controls.ContentDialogButtonClickEventArgs args)
        {
            Continue = true;
        }
    }
}
