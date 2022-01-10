using FileExtensionHandler.Core.Controller;
using FileExtensionHandler.Core.Model;
using FileExtensionHandler.Shared;
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

namespace FileExtensionHandler.Dialogs.Entries
{
    /// <summary>
    /// Interaction logic for Add.xaml
    /// </summary>
    public partial class Add
    {
        public FileExtension FileExtension { get; private set; }
        public Association Association { get; private set; }
        public Add(FileExtension fileExtension)
        {
            InitializeComponent();
            this.Title = "Add new file extension";
            ssp_fileExtension.Visibility = Visibility.Visible;
        }

        public Add(Association association)
        {
            InitializeComponent();
            this.Title = "Add new association";
            ssp_association.Visibility = Visibility.Visible;
        }

        private void ValidateData(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;

            if (textBox.Text.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ResetValues();
                tb_association_json.Text = tb_fileExtension_json.Text = "The name contains invalid characters!";
                return;
            }
            
            string fileDir;
            TextBlock textBlock;
            switch (textBox.Tag)
            {
                case "File Extension":
                    if (textBox.Text.StartsWith(".") || textBox.Text.EndsWith("."))
                    {
                        ResetValues();
                        tb_fileExtension_json.Text = "The file extension is invalid!";
                        return;
                    }
                    textBlock = tb_fileExtension_json;
                    fileDir = Vars.Dir_FileExtensions;
                    FileExtension = FileExtensionsController.Create($".{textBox.Text}");
                    FileExtension.Name = $"{textBox.Text.ToUpper()} File";
                    Association = null;
                    break;
                case "Association":
                    textBlock = tb_association_json;
                    fileDir = Vars.Dir_Associations;
                    Association = AssociationsController.Create(textBox.Text);
                    Association.Name = textBox.Text;
                    FileExtension = null;
                    break;
                default:
                    ResetValues();
                    return;
            }
            string conditionalDot = (FileExtension != null && !String.IsNullOrWhiteSpace(textBox.Text)) ? "." : "";
            textBlock.Text =  $"Will be saved as \"{conditionalDot}{textBox.Text}.json\"";

            if (File.Exists($@"{fileDir}\{textBox.Text}.json"))
            {
                ResetValues();
                tb_association_json.Text = tb_fileExtension_json.Text = "The file already exists!";
                return;
            }
            IsPrimaryButtonEnabled = true;
        }

        private void ResetValues()
        {
            FileExtension = null;
            Association = null;
            IsPrimaryButtonEnabled = false;
        }
    }
}
