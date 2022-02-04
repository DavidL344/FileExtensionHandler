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
    public partial class AssociationPreview
    {
        private readonly string SampleFilePath;
        public AssociationPreview(Association association, string sampleFilePath = null)
        {
            InitializeComponent();
            if (association != null) LoadInfo(association);
            this.SampleFilePath = sampleFilePath;
        }

        private void LoadInfo(Association association)
        {
            Title = $"Association \"{association.Node}\"";
            txt_name.Text = association.Name;
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

        private void PreventEditing(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.C:
                case Key.V:
                case Key.A:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) return;
                    break;

                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                case Key.PageUp:
                case Key.PageDown:
                case Key.Home:
                case Key.End:
                    return;
                default:
                    break;
            }
            e.Handled = true;
        }
    }
}
