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
        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartMonitorAndLink(object sender, RoutedEventArgs e)
        {
            VPNChecker checker = new VPNChecker(((TextBox)this.FindName("vpnName")).Text);
            checker.EmailSender = new EmailSender(((TextBox)this.FindName("emailAddress")).Text,
                ((PasswordBox)this.FindName("emailPassword")).Password,
                "Reconnected VPN", "");
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Hidden;
            MessageBox.Show("Auto VPN Linker Service has started!");
            while (true)
            {
                checker.MonitorVPN();
                Thread.Sleep(900000); // sleep for 15 min per cycle
            }
        }
    }
}
