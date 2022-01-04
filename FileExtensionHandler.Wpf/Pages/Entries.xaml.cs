using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Shared;
using ModernWpf.Controls;
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
            if (appBarButton == null || !appBarButton.Name.StartsWith(btn_prefix)) return;
            switch (appBarButton.Name.Substring(btn_prefix.Length))
            {
                case "add":
                    break;
                case "remove":
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
                case "more":
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
            //MessageBox.Show(((FileExtension)dataGrid.Items[3]).Name);
            //MessageBox.Show(FileExtensions[3].Name);
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
            UpdateView(dataGrid, false);
        }

        private void UpdateView(DataGrid dataGrid, bool keepEdits = false)
        {
            if (keepEdits)
            {
                dataGrid.CommitEdit();
                dataGrid.CommitEdit();
            }
            else
            {
                dataGrid.CancelEdit();
                dataGrid.CancelEdit();
            }

            if (dataGrid.ItemsSource != null)
            {
                dataGrid.SelectedIndex = -1;
                //dataGrid.Items.Refresh();
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
            ActiveDataGrid.CommitEdit();
            ActiveDataGrid.CommitEdit();
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
                return IsBindingValid(dataGrid);
            }
            catch
            {
                return false;
            }
        }

        private bool IsBindingValid(DependencyObject parent)
        {
            if (Validation.GetHasError(parent)) return false;
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsBindingValid(child)) return false;
            }
            return true;
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
    }
}
