using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Shared;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Page = System.Windows.Controls.Page;

namespace FileExtensionHandler.Pages
{
    /// <summary>
    /// Interaction logic for Entries.xaml
    /// </summary>
    public partial class Entries : Page
    {
        private DataGrid[] DataGrids => new DataGrid[] { dg_associations, dg_fileExtensions };
        private List<Association> Associations { get; set; } = new List<Association>();
        private List<FileExtension> FileExtensions { get; set; } = new List<FileExtension>();
        private DataGridCellEditEndingEventArgs LatestEditArgs;
        private DataGrid ActiveDataGrid
        {
            get
            {
                if (dg_associations.Visibility == Visibility.Visible) return dg_associations;
                if (dg_fileExtensions.Visibility == Visibility.Visible) return dg_fileExtensions;
                return null;
            }
            set
            {
                switch (value.Name)
                {
                    case "dg_associations":
                        dg_fileExtensions.Visibility = Visibility.Collapsed;
                        lbl_header.Text = "Associations:";
                        dg_associations.Visibility = Visibility.Visible;
                        break;
                    case "dg_fileExtensions":
                        dg_associations.Visibility = Visibility.Collapsed;
                        lbl_header.Text = "File Extensions:";
                        dg_fileExtensions.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }
        public Entries()
        {
            InitializeComponent();
        }

        private void AppBarButtonClicked(object sender, RoutedEventArgs e)
        {
            string btn_prefix = "btn_";
            AppBarButton appBarButton = sender as AppBarButton;
            AppBarToggleButton appBarToggleButton = sender as AppBarToggleButton;

            if (appBarButton == null || !appBarButton.Name.StartsWith(btn_prefix))
                if (appBarToggleButton == null || !appBarToggleButton.Name.StartsWith(btn_prefix)) return;

            string stringToSwitch = appBarButton != null ? appBarButton.Name.Substring(btn_prefix.Length) : appBarToggleButton.Name.Substring(btn_prefix.Length);
            switch (stringToSwitch)
            {
                case "add":
                    ComingSoon();
                    break;
                case "remove":
                    ComingSoon();
                    break;
                case "refresh":
                    RefreshView();
                    break;
                case "switch":
                    SwitchView();
                    break;
                case "save":
                    SaveToDisk();
                    break;
                case "locate":
                    if (ActiveDataGrid.Name == "dg_associations") Process.Start(Vars.Dir_Associations);
                    if (ActiveDataGrid.Name == "dg_fileExtensions") Process.Start(Vars.Dir_FileExtensions);
                    break;
                default:
                    break;
            }
        }

        private void SaveToDisk()
        {
            if (!IsGridValid(ActiveDataGrid))
            {
                MessageBox.Show("Unable to perform the save operation!\r\nPlease check the data entered.", "fexth", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                switch (ActiveDataGrid.Name)
                {
                    case "dg_associations":
                        List<Association> associationsToSave = dg_associations.Items.OfType<Association>().ToList();
                        foreach (Association associationToSave in associationsToSave)
                            AssociationsController.SaveToJson(associationToSave, Vars.Dir_Associations);
                        break;
                    case "dg_fileExtensions":
                        List<FileExtension> fileExtensionsToSave = dg_fileExtensions.Items.OfType<FileExtension>().ToList();
                        foreach (FileExtension fileExtensionToSave in fileExtensionsToSave)
                            FileExtensionsController.SaveToJson(fileExtensionToSave, Vars.Dir_FileExtensions);
                        break;
                    default:
                        break;
                }
                MessageBox.Show("Data saved!", "fexth", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to save the data: {e.Message}", "Error | fexth", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshView()
        {
            foreach (DataGrid dataGrid in DataGrids)
                if (dataGrid.Visibility == Visibility.Visible) RefreshView(dataGrid);
        }

        private void RefreshView(DataGrid dataGrid)
        {
            if (!DirectoriesExist()) return;

            switch (dataGrid.Name)
            {
                case "dg_associations":
                    Associations = AssociationsController.GetAssociations(Vars.Dir_Associations);
                    break;
                case "dg_fileExtensions":
                    FileExtensions = FileExtensionsController.GetFileExtensions(Vars.Dir_FileExtensions);
                    break;
                default:
                    return;
            }
            UpdateView(dataGrid);
        }

        private void UpdateView(DataGrid dataGrid)
        {
            if (dataGrid.ItemsSource != null)
            {
                dataGrid.SelectedIndex = -1;
                dataGrid.ItemsSource = null;
            }
            
            switch (dataGrid.Name)
            {
                case "dg_associations":
                    dataGrid.ItemsSource = Associations;
                    break;
                case "dg_fileExtensions":
                    dataGrid.ItemsSource = FileExtensions;
                    break;
                default:
                    return;
            }
            if (!dataGrid.IsEnabled) dataGrid.IsEnabled = true;

            ScrollViewer scrollViewer = External.GetVisualChild<ScrollViewer>(dataGrid);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToTop();
                scrollViewer.ScrollToLeftEnd();
            }
        }

        private void SwitchView()
        {
            if (dg_associations.Visibility == Visibility.Visible)
            {
                ActiveDataGrid = dg_fileExtensions;
                return;
            }

            if (dg_fileExtensions.Visibility == Visibility.Visible)
            {
                ActiveDataGrid = dg_associations;
                return;
            }
        }

        private void ViewerLoaded(object sender, RoutedEventArgs e)
        {
            if (!DirectoriesExist()) return;
            if (!(sender is DataGrid dataGrid)) return;
            if (!dataGrid.IsEnabled) RefreshView(dataGrid);
        }

        private void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            LatestEditArgs = e;
        }

        public bool IsGridValid(DataGrid dataGrid)
        {
            try
            {
                if (LatestEditArgs != null)
                {
                    object cellItem = LatestEditArgs.Row.Item;
                    string serialized;
                    switch (dataGrid.Name)
                    {
                        case "dg_associations":
                            Association associationItem = (Association)cellItem;
                            serialized = AssociationsController.Serialize(associationItem);
                            _ = AssociationsController.Deserialize(serialized);
                            break;
                        case "dg_fileExtensions":
                            FileExtension fileExtensionItem = (FileExtension)cellItem;
                            serialized = FileExtensionsController.Serialize(fileExtensionItem);
                            _ = FileExtensionsController.Deserialize(serialized);
                            break;
                        default:
                            return false;
                    }
                }
                return External.IsBindingValid(dataGrid);
            }
            catch
            {
                return false;
            }
        }

        private bool DirectoriesExist()
        {
            bool exists = Directory.Exists(Vars.DefaultSaveLocation)
             && Directory.Exists(Vars.Dir_Associations)
             && Directory.Exists(Vars.Dir_FileExtensions);

            if (!exists)
            {
                grd_main.Visibility = Visibility.Collapsed;
                lbl_noAssociations.Visibility = Visibility.Visible;
            }
            return exists;
        }

        private void ComingSoon(object sender = null, RoutedEventArgs e = null)
        {
            MessageBox.Show("Coming Soon!", "fexth", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
