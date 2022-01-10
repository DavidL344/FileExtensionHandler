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
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information
    {
        public Information()
        {
            InitializeComponent();
        }

        public Information(string text)
        {
            InitializeComponent();
            this.tb_text.Text = text;
        }

        public Information(string text, string title)
        {
            InitializeComponent();
            this.tb_text.Text = text;
            this.Title = title;
        }
    }
}
