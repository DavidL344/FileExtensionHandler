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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExtensionHandler.Pages
{
    /// <summary>
    /// Interaction logic for AppPicker.xaml
    /// </summary>
    public partial class AppPicker : Page
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
            if (FileInformation.FileExtension.Node == null || FileInformation.FileExtension.DefaultCheckboxHide)
                chk_remember.Visibility = Visibility.Collapsed;
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

            header.Text = $"Please select an application to open the {FileInformation.Type ?? "unassociated file"} with:";
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

            if ((bool)chk_remember.IsChecked)
            {
                FileInformation.FileExtension.DefaultAssociation = FileInformation.Associations[id].Node;
                FileInformation.SaveFileExtensionInfo();
            }
            FileInformation.OpenWith(id);
            Application.Current.Shutdown();
        }

        private void ListBoxLoaded(object sender, RoutedEventArgs e)
        {
            int maxShownRows = 3; // 3-5 rows; default = 3
            int onScreenItems = lb_selection.Items.Count > 5 ? 5 : lb_selection.Items.Count;

            if (maxShownRows < 3 || maxShownRows > 5) maxShownRows = 3;
            double maxHeight = lb_selection.ActualHeight / onScreenItems * maxShownRows;

            lb_selection.Width = lb_selection.ActualWidth;
            if (lb_selection.ActualHeight > maxHeight) lb_selection.Height = maxHeight;
        }
    }
}
