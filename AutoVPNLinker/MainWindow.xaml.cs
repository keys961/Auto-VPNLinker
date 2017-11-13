using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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

namespace AutoVPNLinker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private delegate void StartDelegate();

        private StartDelegate startDelegate;

        //private delegate void stopDelegate(object sender);

        private Thread thread;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartMonitorAndLink(object sender, RoutedEventArgs e)
        {
            //thread = new Thread(StartMonitorAndLink);
            // thread.Start(sender);
            StartMonitorAndLink();
            
        }

        private void Run()
        {
            Dispatcher.Invoke(startDelegate);
        }

        private void StartMonitorAndLink()
        {
            TextBox vpnNameTextBox = ((TextBox)this.FindName("vpnName"));
            TextBox emailAddressTextBox = ((TextBox)this.FindName("emailAddress"));
            PasswordBox emailPassWordBox = ((PasswordBox)this.FindName("emailPassword"));
            VPNChecker checker = new VPNChecker(vpnNameTextBox.Text);
            checker.EmailSender = new EmailSender(emailAddressTextBox.Text, emailPassWordBox.Password,
                "Reconnected VPN", "");

            
            /*vpnNameTextBox.IsEnabled = false;
            emailAddressTextBox.IsEnabled = false;
            emailPassWordBox.IsEnabled = false;
            ((Button)this.FindName("startButton")).IsEnabled = false;
            ((Button)this.FindName("stopButton")).IsEnabled = true;*/
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Hidden;
            MessageBox.Show("Auto VPN Linker Service has started!");
            while (true)
            {
                checker.MonitorVPN();
                Thread.Sleep(900000); // sleep for 15 min per cycle
            }
        }

        public void StopMonitor(object sender, RoutedEventArgs e)
        {
            if (thread.IsAlive)
                thread.Abort();
            ((Button)sender).IsEnabled = false;
            TextBox vpnNameTextBox = ((TextBox)this.FindName("vpnName"));
            TextBox emailAddressTextBox = ((TextBox)this.FindName("emailAddress"));
            PasswordBox emailPassWordBox = ((PasswordBox)this.FindName("emailPassword"));
            vpnNameTextBox.IsEnabled = true;
            emailAddressTextBox.IsEnabled = true;
            emailPassWordBox.IsEnabled = true;
            ((Button)this.FindName("startButton")).IsEnabled = true;
        }

        
    }
}
