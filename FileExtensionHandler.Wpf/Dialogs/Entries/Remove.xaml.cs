using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExtensionHandler.Dialogs.Entries
{
    /// <summary>
    /// Interaction logic for Remove.xaml
    /// </summary>
    public partial class Remove
    {
        private readonly Timer Timer = new Timer();
        private readonly int TimerSeconds = 3;
        private int TimeElapsed = 0;
        public Remove(string name = null, string filePath = null)
        {
            InitializeComponent();
            if (name != null) Title = $"Delete \"{name}\"";
            if (filePath != null) tb_text.Text = $"This will delete \"{filePath}\"\r\n\r\nThis action cannot be undone!";
            SecondaryButtonText = $"Confirm ({TimerSeconds})";
            TimerSeconds--;
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Interval = 1000;
            Timer.Elapsed += TimerUpdate;
            Timer.Start();
        }

        private void TimerUpdate(object source, ElapsedEventArgs e)
        {
            bool timerFinished = TimeElapsed == TimerSeconds;
            Dispatcher.Invoke(new Action(() =>
            {
                SecondaryButtonText = timerFinished ? $"Confirm" : $"Confirm ({TimerSeconds - TimeElapsed})";
                if (timerFinished)
                {
                    IsSecondaryButtonEnabled = true;
                    Timer.Stop();
                    Timer.Dispose();
                }
            }));
            TimeElapsed++;
        }
    }
}
