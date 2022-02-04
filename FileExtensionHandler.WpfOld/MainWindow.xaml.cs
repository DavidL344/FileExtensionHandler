using FileExtensionHandler.Core;
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
        internal MainWindow(string page = "Home", FileInformation fileInformation = null)
        {
            InitializeComponent();
            Title = $"{Vars.AssemblyTitle} v{Vars.AssemblyVersionShort}";
            NavigateToPage(page, true, fileInformation);
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

        private void NavigateToPage(string pageName, bool forcedLoad = false, FileInformation fileInformation = null)
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

            if (pageName == "AppPicker")
            {
                if (fileInformation != null)
                {
                    Title = "";
                    nv_main.IsPaneVisible = false;
                    contentFrame.Navigate(new Pages.AppPicker(fileInformation));
                    return;
                }
            }
            else
            {
                Title = $"{Vars.AssemblyTitle} v{Vars.AssemblyVersionShort}";
            }

            pageName = "FileExtensionHandler.Pages." + pageName;
            Type pageType = typeof(Home).Assembly.GetType(pageName);

            if (menuItemIndex != -1 && pageType != null)
            {
                nv_main.SelectedItem = nv_main.MenuItems[menuItemIndex];
                contentFrame.Navigate(pageType);
                return;
            }
            if (forcedLoad && nv_main.MenuItems.Count > 0)
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
            new Dialogs.About().ShowAsync();
        }
    }
}
