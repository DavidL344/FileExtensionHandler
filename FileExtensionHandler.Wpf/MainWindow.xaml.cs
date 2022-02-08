using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Wpf.Pages;
using FileExtensionHandler.Wpf.Shared;
using ModernWpf.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FileExtensionHandler.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _pageLoading = false;
        public MainWindow(string page = "Home")
        {
            InitializeComponent();
            Title = $"{Vars.ProductName} v{Vars.ProductVersion}";
            NavigateToPage(page);
        }

        private void NavigateToPage(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (_pageLoading) return;
            _pageLoading = true;
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Pages.Settings));
            }
            else
            {
                NavigationViewItem selectedItem = (NavigationViewItem)args.SelectedItem;
                NavigateToPage((string)selectedItem.Tag);
            }
            _pageLoading = false;
        }

        private void NavigateToPage(string pageName)
        {
            if (_pageLoading) return;
            int menuItemIndex = -1;
            for (int i = 0; i < nv_main.MenuItems.Count; i++)
            {
                if ((string)((NavigationViewItem)nv_main.MenuItems[i]).Tag == pageName)
                {
                    menuItemIndex = i;
                    break;
                }
            }
            Title = $"{Vars.ProductName} v{Vars.ProductVersion}";

            pageName = "FileExtensionHandler.Pages." + pageName;
            Type pageType = typeof(Home).Assembly.GetType(pageName);

            if (menuItemIndex != -1 && pageType != null)
            {
                nv_main.SelectedItem = nv_main.MenuItems[menuItemIndex];
                contentFrame.Navigate(pageType);
                return;
            }
            if (nv_main.MenuItems.Count > 0)
                nv_main.SelectedItem = nv_main.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void NavigateToPage(string pageName, FileInformation fileInformation)
        {
            if (_pageLoading) return;
            int menuItemIndex = -1;
            for (int i = 0; i < nv_main.MenuItems.Count; i++)
            {
                if ((string)((NavigationViewItem)nv_main.MenuItems[i]).Tag == pageName)
                {
                    menuItemIndex = i;
                    break;
                }
            }

            if (pageName == "AppPicker")
            {
                Title = "";
                nv_main.IsPaneVisible = false;
                contentFrame.Navigate(new Pages.AppPicker(fileInformation));
                return;
            }
            else
            {
                Title = $"{Vars.ProductName} v{Vars.ProductVersion}";
            }

            pageName = "FileExtensionHandler.Pages." + pageName;
            Type pageType = typeof(Home).Assembly.GetType(pageName);

            if (menuItemIndex != -1 && pageType != null)
            {
                nv_main.SelectedItem = nv_main.MenuItems[menuItemIndex];
                contentFrame.Navigate(pageType);
                return;
            }
            if (nv_main.MenuItems.Count > 0)
                nv_main.SelectedItem = nv_main.MenuItems.OfType<NavigationViewItem>().First();
        }

        /// <remarks>
        /// An access violation occurs when trying to access the current window:
        /// <code>Exception thrown at 0x00007FFA060C7FCA (Windows.UI.Xaml.dll) in fexth.exe: 0xC0000005: Access violation reading location 0x0000000000000220.</code>
        /// The code that accesses the current window:
        /// <code>if (VisualTreeHelper.GetOpenPopups(Windows.UI.Xaml.Window.Current).Count > 0) return;</code>
        /// The app won't crash when the async method isn't awaited even if there's a dialog open
        /// </remarks>
        private void ShowLicenseInformation(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.F1)) return;
            //new Dialogs.About().ShowAsync();
        }
    }
}
