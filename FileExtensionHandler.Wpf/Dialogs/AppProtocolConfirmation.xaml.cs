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

namespace FileExtensionHandler.Dialogs
{
    /// <summary>
    /// Interaction logic for AppProtocolConfirmation.xaml
    /// </summary>
    public partial class AppProtocolConfirmation
    {
        internal bool Continue { get; private set; } = false;
        private readonly Timer Timer = new Timer();
        private readonly int TimerSeconds = 3;
        private int TimeElapsed = 0;
        public AppProtocolConfirmation()
        {
            InitializeComponent();
            SecondaryButtonText = $"Yes ({TimerSeconds})";
            TimerSeconds--;
        }

        private void Confirm(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Continue = true;
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
                SecondaryButtonText = timerFinished ? $"Yes" : $"Yes ({TimerSeconds - TimeElapsed})";
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
