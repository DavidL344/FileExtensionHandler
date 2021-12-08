using FileExtensionHandler.Settings.Controller;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        internal SettingsController SettingsController;
        private string SavedSettingsChecksum;
        internal Settings(SettingsController settingsController)
        {
            InitializeComponent();
            this.SettingsController = settingsController;
            this.SavedSettingsChecksum = this.SettingsController.Checksum;
            this.Background = Brushes.Transparent; // temp
            //ShowValues();
            Load();
        }

        private void ShowValues()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Style = FindResource("TextKey") as Style;
            textBlock.Text = "Navbar pane display mode";

            ComboBox comboBox = new ComboBox();
            comboBox.Name = "cb_navbar_PaneDisplayMode";
            comboBox.Items.Insert(0, "Auto");
            comboBox.Items.Insert(1, "Left");
            comboBox.Items.Insert(2, "Top");
            comboBox.Items.Insert(3, "Left (compact)");
            comboBox.Items.Insert(4, "Left (minimal)");
            comboBox.SelectedIndex = (int)SettingsController.Settings.GUI.Navbar_PaneDisplayMode;
            comboBox.SelectionChanged += new SelectionChangedEventHandler(Save);

            Grid.SetRow(textBlock, 0);
            Grid.SetColumn(textBlock, 0);
            
            Grid.SetRow(comboBox, 0);
            Grid.SetColumn(comboBox, 1);

            grd_main.Children.Add(textBlock);
            grd_main.Children.Add(comboBox);
        }

        private void Load()
        {
            cb_Navbar_PaneDisplayMode.SelectedIndex = (int)SettingsController.Settings.GUI.Navbar_PaneDisplayMode;
        }

        private void Save(object sender = null, SelectionChangedEventArgs e = null)
        {
            if (!ChecksumController.VerifyHash(SavedSettingsChecksum, SettingsController.Checksum))
            {
                SettingsController.Settings.GUI.Navbar_PaneDisplayMode = (ModernWpf.Controls.NavigationViewPaneDisplayMode)cb_Navbar_PaneDisplayMode.SelectedIndex;
                SettingsController.Save();
                SavedSettingsChecksum = SettingsController.Checksum;
            }
        }

        private void Reset()
        {
            SettingsController.SetDefaults();
        }

        private void ResetAndSave()
        {
            SettingsController.SetDefaults();
            Save();
        }
    }
}
