using FileExtensionHandler.Pages;
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

namespace FileExtensionHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainWindow(string page = "Home")
        {
            InitializeComponent();
            Title = $"{Vars.AssemblyTitle} v{Vars.AssemblyVersionShort}";
            NavigateToPage(page, true);
        }

        private void NavigateToPage(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Pages.Settings));
            }
            else
            {
                NavigationViewItem selectedItem = (NavigationViewItem)args.SelectedItem;
                NavigateToPage((string)selectedItem.Tag);
            }
        }

        private void NavigateToPage(string pageName, bool forcedLoad = false)
        {
            int menuItemIndex = -1;
            for (int i = 0; i < nv_main.MenuItems.Count; i++)
            {
                if ((string)((NavigationViewItem)nv_main.MenuItems[i]).Tag == pageName)
                {
                    menuItemIndex = i;
                    break;
                }
            }

            if (pageName == "AppPicker") nv_main.Visibility = Visibility.Collapsed;
            pageName = "FileExtensionHandler.Pages." + pageName;
            Type pageType = typeof(Home).Assembly.GetType(pageName);

            if (menuItemIndex != -1 && pageType != null)
            {
                nv_main.SelectedItem = nv_main.MenuItems[menuItemIndex];
                contentFrame.Navigate(pageType);
                return;
            }
            if (forcedLoad) nv_main.SelectedItem = nv_main.MenuItems.OfType<NavigationViewItem>().First();
        }
    }
}
