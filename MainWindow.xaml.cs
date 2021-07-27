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

namespace FileExtensionHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Model.MultipleFiles.Handler Handler;
        internal MainWindow(Model.MultipleFiles.Handler handler)
        {
            InitializeComponent();
            this.Handler = handler;
            if (Handler != null && App.Args.Length > 0) if (LoadAssociations()) return;

            header.Text = "Welcome to fexth!";
            lb_selection.Items.Clear();
            footer.Text = "Call this app with a parameter to get started.";
        }

        private bool LoadAssociations()
        {
            List<Model.MultipleFiles.Association> associationsList = this.Handler.GetAssociations();
            if (associationsList.Count == 0) return false;

            lb_selection.Items.Clear();
            foreach (Model.MultipleFiles.Association fileAssociation in associationsList)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = $"{fileAssociation.Name}\r\n          {fileAssociation.Command} {Environment.ExpandEnvironmentVariables(fileAssociation.Arguments)}"
                };
                lb_selection.Items.Add(listBoxItem);
            }

            header.Text = $"Please select an application to open the {Handler.FileType} with:";
            footer.Text = $"{Handler.FilePath}";
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
            Handler.RunApp(id);
        }
    }
}
