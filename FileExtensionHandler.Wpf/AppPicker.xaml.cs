using FileExtensionHandler.Core;
using FileExtensionHandler.Core.Model;
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
using System.Windows.Shapes;

namespace FileExtensionHandler
{
    /// <summary>
    /// Interaction logic for AppPicker.xaml
    /// </summary>
    public partial class AppPicker : Window
    {
        internal FileInformation FileInformation;
        internal AppPicker(FileInformation fileInformation)
        {
            InitializeComponent();
            this.FileInformation = fileInformation;
            LoadAssociations();
        }

        private void LoadAssociations()
        {
            List<Association> associationsList = FileInformation.Associations;
            if (associationsList.Count == 0) return;

            lb_selection.Items.Clear();
            foreach (Association fileAssociation in associationsList)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = $"{fileAssociation.Name}\r\n          {fileAssociation.Command} {Environment.ExpandEnvironmentVariables(fileAssociation.Arguments)}"
                };
                lb_selection.Items.Add(listBoxItem);
            }

            header.Text = $"Please select an application to open the {FileInformation.Type} with:";
            footer.Text = $"{FileInformation.Location}";
            return;
        }

        private void AppSelected(object sender, MouseButtonEventArgs e)
        {
            RunAnAppOfChoice();
        }

        private void AppSelected(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Down) && lb_selection.SelectedIndex == -1) lb_selection.SelectedIndex = 0;
            if (Keyboard.IsKeyDown(Key.Enter)) RunAnAppOfChoice();
        }

        private void RunAnAppOfChoice(int id = -1)
        {
            if (id == -1) id = lb_selection.SelectedIndex;
            if (Keyboard.IsKeyDown(Key.Escape)) id = -1;
            if (id == -1) return;
            FileInformation.OpenWith(id);
            this.Close();
        }
    }
}
