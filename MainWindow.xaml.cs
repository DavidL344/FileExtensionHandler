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
        internal Model.Parser Parser;
        internal MainWindow(Model.Parser parser)
        {
            InitializeComponent();

            if (App.Args.Length == 1)
            {
                Parser = new Model.Parser(App.Args[0]);
                if (LoadAssociations(App.Args[0])) return;
            }
            header.Text = "Welcome to fexth!";
            lb_selection.Items.Clear();
            footer.Text = "Call this app with a parameter to get started.";
        }

        private bool LoadAssociations(string filePath)
        {
            List<Model.Association> associationsList = Parser.AssociationsList;
            if (associationsList.Count == 0) return false;

            lb_selection.Items.Clear();
            foreach (Model.Association fileAssociation in associationsList)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = $"{fileAssociation.Name}\r\n          {fileAssociation.Path} {Environment.ExpandEnvironmentVariables(fileAssociation.Arguments)}"
                };
                lb_selection.Items.Add(listBoxItem);
            }
            footer.Text = $"{Path.GetFileName(filePath)}";
            return true;
        }

        private void AppSelected(object sender, MouseButtonEventArgs e)
        {
            RunAnAppOfChoice();
        }

        private void AppSelected(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter)) RunAnAppOfChoice();
        }

        private void RunAnAppOfChoice(int id = -1)
        {
            if (id == -1) id = lb_selection.SelectedIndex;
            if (Keyboard.IsKeyDown(Key.Escape)) id = -1;
            if (id == -1) return;
            Parser.RunApp(id);
        }
    }
}
