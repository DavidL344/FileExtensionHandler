using ModernWpf.Controls;
using System.Threading.Tasks;

namespace FileExtensionHandler.Wpf.Dialogs
{
    internal class Dialog
    {
        public static void Show(string message)
        {
            new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = ClassicDialog.Buttons.OK,
                NoTitle = true
            }.ShowAsync();
        }

        public static void Show(string message, string title)
        {
            new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = ClassicDialog.Buttons.OK
            }.ShowAsync();
        }

        public static void ShowOKClipboard(string message)
        {
            new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = ClassicDialog.Buttons.OKClipboard,
                NoTitle = true
            }.ShowAsync();
        }

        public static void ShowOKClipboard(string message, string title)
        {
            new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = ClassicDialog.Buttons.OKClipboard
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, ClassicDialog.Buttons dialogButton)
        {
            return await new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = dialogButton,
                NoTitle = true
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, string title, ClassicDialog.Buttons dialogButton)
        {
            return await new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = dialogButton
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, ClassicDialog.Buttons dialogButton, ContentDialogButton defaultDialogButton)
        {
            return await new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = dialogButton,
                NoTitle = true,
                DefaultDialogButton = defaultDialogButton
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, string title, ClassicDialog.Buttons dialogButton, ContentDialogButton defaultDialogButton)
        {
            return await new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = dialogButton,
                DefaultDialogButton = defaultDialogButton
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, ClassicDialog.Buttons dialogButton, ContentDialogResult[] closePreventionButtons)
        {
            return await new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = dialogButton,
                NoTitle = true,
                ClosePreventionButtons = closePreventionButtons
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, string title, ClassicDialog.Buttons dialogButton, ContentDialogResult[] closePreventionButtons)
        {
            return await new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = dialogButton,
                ClosePreventionButtons = closePreventionButtons
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, ClassicDialog.Buttons dialogButton, ContentDialogButton defaultDialogButton, ContentDialogResult[] closePreventionButtons)
        {
            return await new ClassicDialog()
            {
                DialogText = message,
                DialogButtons = dialogButton,
                NoTitle = true,
                DefaultDialogButton = defaultDialogButton,
                ClosePreventionButtons = closePreventionButtons
            }.ShowAsync();
        }

        public static async Task<ContentDialogResult> ShowAsync(string message, string title, ClassicDialog.Buttons dialogButton, ContentDialogButton defaultDialogButton, ContentDialogResult[] closePreventionButtons)
        {
            return await new ClassicDialog()
            {
                DialogTitle = title,
                DialogText = message,
                DialogButtons = dialogButton,
                DefaultDialogButton = defaultDialogButton,
                ClosePreventionButtons = closePreventionButtons
            }.ShowAsync();
        }
    }
}
