using FileExtensionHandler.Core;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Shared;
using Microsoft.Win32;
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
using Path = System.IO.Path;

namespace FileExtensionHandler.Pages
{
    /// <summary>
    /// Interaction logic for LivePreview.xaml
    /// </summary>
    public partial class LivePreview : Page
    {
        private FileInformation FileInformation;
        private readonly string SampleFilePath = Environment.ExpandEnvironmentVariables(@"%userprofile%\Desktop\Test.mp3");
        private SolidColorBrush DefaultAssociationBrush => (SolidColorBrush)(new BrushConverter().ConvertFrom("#67BF40")); // alternative: #40BF4D
        private readonly string EmptyFileExtension = "(empty)";
        private readonly string UnknownFileExtension = "(unknown)";
        private bool Processing = false;
        public LivePreview()
        {
            InitializeComponent();
            LoadAllFileExtensions();
            LoadFileAssociations();
        }

        private void LoadAllFileExtensions()
        {
            Processing = true;
            List<string> loadedFileExtensionsList = new List<string>();

            if (!File.Exists($@"{Vars.Dir_FileExtensions}\.json")) loadedFileExtensionsList.Add(EmptyFileExtension);
            loadedFileExtensionsList.Add(UnknownFileExtension);

            DirectoryInfo directoryInfo = new DirectoryInfo(Vars.Dir_FileExtensions);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string fileExtension = Path.GetFileNameWithoutExtension(file.FullName);
                loadedFileExtensionsList.Add(fileExtension == "" ? EmptyFileExtension : fileExtension);
            }

            string[] loadedFileExtensions = loadedFileExtensionsList.ToArray();
            Array.Sort(loadedFileExtensions);

            cb_fileExtensions.Items.Clear();
            foreach (string fileExtension in loadedFileExtensions)
                cb_fileExtensions.Items.Add(fileExtension);

            cb_fileExtensions.SelectedIndex = 0;
            Processing = false;
            txt_sampleFilePath.Text = Path.ChangeExtension(SampleFilePath, null);
        }

        private void LoadFileAssociations(object sender = null, SelectionChangedEventArgs e = null)
        {
            string filePath = (!String.IsNullOrEmpty(txt_sampleFilePath.Text)) ? txt_sampleFilePath.Text : "";
            FileInformation = new FileInformation(filePath, Vars.Dir_Associations, Vars.Dir_FileExtensions);

            List<Association> associationsList = FileInformation.Associations;
            lb_viewer.Items.Clear();
            if (associationsList.Count == 0) return;

            foreach (Association fileAssociation in associationsList)
            {
                ListBoxItem listBoxItem = new ListBoxItem
                {
                    Content = $"{fileAssociation.Name}\r\n          {fileAssociation.Command} {Environment.ExpandEnvironmentVariables(fileAssociation.Arguments)}"
                };
                lb_viewer.Items.Add(listBoxItem);
            }
            if (FileInformation.DefaultAssociationIndex != -1) (lb_viewer.Items[FileInformation.DefaultAssociationIndex] as ListBoxItem).Background = DefaultAssociationBrush;
        }

        private void ListBoxLoaded(object sender, RoutedEventArgs e)
        {
            lb_viewer.Height = lb_viewer.ActualHeight / lb_viewer.Items.Count * 3;
        }

        private void OpenSampleFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files|*.*"
            };
            if (openFileDialog.ShowDialog() == true) txt_sampleFilePath.Text = openFileDialog.FileName;
        }

        private void DataUpdated(object sender, object e)
        {
            if (Processing) return;
            Processing = true;
            string fileExtension;
            switch (sender.GetType().Name)
            {
                case "ComboBox":
                    fileExtension = cb_fileExtensions.SelectedItem.ToString();
                    if (fileExtension == UnknownFileExtension)
                    {
                        do
                        {
                            fileExtension = GetRandomFileExtension();
                        } while (cb_fileExtensions.Items.Contains(fileExtension));
                    }
                    txt_sampleFilePath.Text = Path.ChangeExtension(txt_sampleFilePath.Text, (fileExtension == EmptyFileExtension) ? null : fileExtension);
                    break;
                case "TextBox":
                    fileExtension = Path.GetExtension(txt_sampleFilePath.Text);
                    if (cb_fileExtensions.Items.Contains(fileExtension))
                    {
                        cb_fileExtensions.SelectedItem = fileExtension;
                    }
                    else
                    {
                        cb_fileExtensions.SelectedItem = (fileExtension == "") ? EmptyFileExtension : UnknownFileExtension;
                    }
                    break;
                default:
                    Processing = false;
                    return;
            }
            LoadFileAssociations();
            Processing = false;
        }

        public static string GetRandomFileExtension(int length = 3)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        private void OpenAssociationInformation(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = lb_viewer.SelectedIndex;
            if (selectedIndex == -1) return;
            Association associationFromIndex = FileInformation.Associations[selectedIndex];

            new Dialogs.AssociationPreview(associationFromIndex, txt_sampleFilePath.Text).ShowAsync();
            lb_viewer.SelectedIndex = -1;
        }
    }
}
