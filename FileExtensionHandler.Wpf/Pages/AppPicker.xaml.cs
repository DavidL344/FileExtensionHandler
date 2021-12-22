using FileExtensionHandler.Core;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            RunAnAppOfChoice();
        }

        private void LoadAssociations()
        {
            if (FileInformation.FileExtension == null
                || FileInformation.FileExtension.Node == null
                || FileInformation.Associations.Count == 0
                || FileInformation.DefaultAssociation != null
                || FileInformation.FileExtension.DefaultCheckboxHide)
                chk_remember.Visibility = Visibility.Collapsed;
            List<Association> associationsList = FileInformation.Associations.Count > 0 ? FileInformation.Associations : new List<Association> {
                null
            };

            lb_selection.Items.Clear();
            foreach (Association fileAssociation in associationsList)
            {
                string boxContent = fileAssociation != null
                    ? $"{fileAssociation.Name}\r\n          {fileAssociation.Command} {Environment.ExpandEnvironmentVariables(fileAssociation.Arguments)}"
                    : "Let Windows decide what to do with the file extension";
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = boxContent
                };
                lb_selection.Items.Add(listBoxItem);
            }

            if (FileInformation.DefaultAssociation != null)
                lb_selection.SelectedIndex = FileInformation.DefaultAssociationIndex;

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

        private async void RunAnAppOfChoice(int id = -1)
        {
            if (id == -1) id = lb_selection.SelectedIndex;
            if (Keyboard.IsKeyDown(Key.Escape)) id = -1;
            if (id == -1) return;

            if (!await FexthProtocolConfirm())
            {
                lb_selection.SelectedIndex = (FileInformation.DefaultAssociation != null) ? FileInformation.DefaultAssociationIndex : - 1;
                return;
            }

            if ((bool)chk_remember.IsChecked && chk_remember.IsVisible)
            {
                FileInformation.FileExtension.DefaultAssociation = FileInformation.Associations[id].Node;
                FileInformation.SaveFileExtensionInfo();
            }

            id = (FileInformation.Associations.Count == 0) ? -1 : id;
            try
            {
                FileInformation.OpenWith(id);
            }
            catch (Exception e) when (!Debugger.IsAttached)
            {
                // Suppress the exception when the user cancels the UAC prompt
                int errorCode = (e is Win32Exception) ? (e as Win32Exception).NativeErrorCode : e.HResult;
                if (errorCode != 1223) MessageBox.Show(String.Format("An error has occured.\r\nException type: {0}\r\nException Description: {1}", e.GetType(), e.Message), "Error | fexth", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }

        private async Task<bool> FexthProtocolConfirm()
        {
            if (FileInformation.CalledFromAppProtocol)
            {
                grd_main.IsEnabled = false;
                AppProtocolConfirmation confirmation = new AppProtocolConfirmation();
                await confirmation.ShowAsync();
                grd_main.IsEnabled = true;
                return confirmation.Continue;
            }
            return true;
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
