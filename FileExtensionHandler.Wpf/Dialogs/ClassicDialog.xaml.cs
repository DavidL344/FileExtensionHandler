using ModernWpf.Controls;
using System;
using System.Linq;
using System.Windows;

namespace FileExtensionHandler.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for ClassicDialog.xaml
    /// </summary>
    public partial class ClassicDialog
    {
        private string _title;

        public string CheckboxText
        {
            get => (string)chk_box.Content;
            set => chk_box.Content = value;
        }

        public bool CheckboxValue => chk_box.IsChecked ?? false;

        public Visibility CheckboxVisibility
        {
            get => chk_box.Visibility;
            set => chk_box.Visibility = value;
        }

        public string DialogText
        {
            get => tb_text.Text;
            set => tb_text.Text = value;
        }

        public string DialogTitle
        {
            get => (string)Title;
            set => Title = _title = value;
        }

        public bool NoTitle
        {
            set
            {
                if (value)
                {
                    Title = null;
                    tb_text.Margin = new Thickness(0, 20, 0, 20);
                    tb_text.FontSize = 18;
                    tb_text.HorizontalAlignment = HorizontalAlignment.Center;
                    tb_text.VerticalAlignment = VerticalAlignment.Center;
                }
                else
                {
                    Title = _title;
                    tb_text.Margin = new Thickness(0);
                    tb_text.FontSize = 12;
                    tb_text.HorizontalAlignment = HorizontalAlignment.Left;
                    tb_text.VerticalAlignment = VerticalAlignment.Top;
                }
            }
        }

        public Buttons DialogButtons
        {
            set
            {
                PrimaryButtonText = null;
                SecondaryButtonText = null;
                CloseButtonText = null;
                DefaultButton = ContentDialogButton.None;

                switch (value)
                {
                    case Buttons.OK:
                        CloseButtonText = "OK";
                        DefaultButton = ContentDialogButton.Close;
                        break;
                    case Buttons.YesNo:
                        PrimaryButtonText = "Yes";
                        SecondaryButtonText = "No";
                        DefaultButton = ContentDialogButton.Primary;
                        break;
                    case Buttons.OKCancel:
                        PrimaryButtonText = "OK";
                        CloseButtonText = "Cancel";
                        DefaultButton = ContentDialogButton.Primary;
                        break;
                    case Buttons.OKClipboard:
                        PrimaryButtonText = "OK";
                        SecondaryButtonText = "Copy to Clipboard";
                        DefaultButton = ContentDialogButton.Primary;
                        SecondaryButtonClick += CopyToClipboard;
                        ClosePreventionButtons = new ContentDialogResult[] { ContentDialogResult.Secondary };
                        break;
                    case Buttons.YesNoCancel:
                        PrimaryButtonText = "Yes";
                        SecondaryButtonText = "No";
                        CloseButtonText = "Cancel";
                        DefaultButton = ContentDialogButton.Primary;
                        break;
                    default:
                        break;
                }
            }
        }

        private void CopyToClipboard(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string data = $"{DialogTitle}\r\n{DialogText}";
            Clipboard.SetText(data);
        }

        public ContentDialogButton DefaultDialogButton
        {
            get => DefaultButton;
            set => DefaultButton = value;
        }

        public ContentDialogResult[] ClosePreventionButtons { get; set; } = Array.Empty<ContentDialogResult>();

        public enum Buttons
        {
            OK,
            YesNo,
            OKCancel,
            OKClipboard,
            YesNoCancel
        }

        public ClassicDialog()
        {
            InitializeComponent();
            _title = DialogTitle;
            chk_box.Visibility = Visibility.Collapsed;
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            args.Cancel = ClosePreventionButtons.Contains(args.Result);
        }
    }
}
