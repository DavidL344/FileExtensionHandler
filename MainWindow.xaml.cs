using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace FileExtensionHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //string arg = @"%userprofile%\Desktop\temp.flac";
            //LoadAssociations(arg);
            if (App.Args.Length == 1) LoadAssociations(App.Args[0]);
        }

        private void LoadAssociations(string filePath)
        {
            Model.Handler handler = new Model.Handler();
            if (handler.Data == null) handler.GenerateSomeAssociations();

            string fileExtension = Path.GetExtension(filePath);
            List<Model.Association> associationsList = handler.Data[fileExtension].Associations;

            lb_selection.Items.Clear();
            foreach (Model.Association fileAssociation in associationsList)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = $"{fileAssociation.Name}\r\n          {fileAssociation.Path} {fileAssociation.Arguments}",
                    Tag = fileAssociation.Path
                };
                lb_selection.Items.Add(listBoxItem);
            }
            footer.Text = $"{Path.GetFileName(filePath)}";
        }
    }
}
