using FileExtensionHandler.Core.Model;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
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

namespace FileExtensionHandler.Dialogs
{
    /// <summary>
    /// Interaction logic for AssociationDetails.xaml
    /// </summary>
    public partial class AssociationDetails
    {
        private readonly string SampleFilePath;
        public AssociationDetails(Association association, bool editMode = false, string sampleFilePath = null)
        {
            InitializeComponent();
            if (association != null) LoadInfo(association);
            this.SampleFilePath = sampleFilePath;

            if (editMode)
            {
                stck_optional.Visibility = Visibility.Visible;
                this.Height = 550;
                this.PrimaryButtonText = "Save";
            }
        }

        private void LoadInfo(Association association)
        {
            Title = $"Association \"{association.Node}\"";
            txt_name.Text = association.Name;
            txt_icon.Text = association.Icon;
            num_icon.Text = association.IconIndex.ToString();
            txt_command.Text = association.Command;
            txt_arguments.Text = association.Arguments;
        }

        private void RunCommand(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string filePath = SampleFilePath ?? "";
            string fileName = Environment.ExpandEnvironmentVariables(txt_command.Text);
            string arguments = Environment.ExpandEnvironmentVariables(txt_arguments.Text).Replace("%1", $"\"{filePath}\"");

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = System.IO.Path.GetDirectoryName(fileName)
            };
            Process.Start(processStartInfo);
        }
    }
}
