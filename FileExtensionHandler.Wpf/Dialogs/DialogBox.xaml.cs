using ModernWpf.Controls;
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

namespace FileExtensionHandler.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogBox
    {
        public string Text { set => tb_text.Text = value; }
        public ContentDialogResult[] CancelCloseEventButtons { get; set; } = new ContentDialogResult[] { };
        public DialogBox()
        {
            InitializeComponent();
        }

        public DialogBox(string text, Target target = Target.Title)
        {
            InitializeComponent();
            switch (target)
            {
                case Target.Title:
                    Title = text;
                    break;
                case Target.Text:
                    Text = text;
                    break;
                default:
                    break;
            }
        }

        public DialogBox(string text, string title)
        {
            InitializeComponent();
            Text = text;
            Title = title;
        }

        public DialogBox(Exception e)
        {
            InitializeComponent();
            Text = $"{e.Message}\r\n{e.StackTrace}";
            Title = "An exception has occured";
            SecondaryButtonText = "Copy to clipboard";
            SecondaryButtonClick += CopyToClipboard;
            CancelCloseEventButtons = new ContentDialogResult[] { ContentDialogResult.Secondary };
        }

        public enum Target
        {
            Title,
            Text
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = CancelCloseEventButtons.Contains(args.Result);
        }

        private void CopyToClipboard(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Clipboard.SetText($"{Title}:\r\n{tb_text.Text}");
        }
    }
}
