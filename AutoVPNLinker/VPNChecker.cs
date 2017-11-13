using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace AutoVPNLinker
{
    public class VPNChecker
    {
        private EmailSender emailSender;

        public EmailSender EmailSender { set => emailSender = value; }

        private string COMMAND = Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\" + "rasdial.exe";

        private string vpnName;

        public string VpnName { get => vpnName; }
        

        public VPNChecker(string vpnName)
        {
            this.vpnName = vpnName;
        }

        private NetworkInterface GetVPNInterface()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.Name.Equals(vpnName))
                    return adapter;
            }
            return null;
        }

        private bool LinkVPN()
        {
            try
            {
                ProcessStartInfo process = new ProcessStartInfo(COMMAND, vpnName);
                process.CreateNoWindow = true;
                process.UseShellExecute = false;
                Process.Start(process);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void MonitorVPN()
        {
            NetworkInterface vpnAdapter = null;
            int counter = 0;
            string ipAddress = null;
            while ((vpnAdapter = this.GetVPNInterface()) == null)
            {
                this.LinkVPN();
                counter++;
                Thread.Sleep(500);
            }
            try
            {
                IPInterfaceProperties properties = vpnAdapter.GetIPProperties();
                if (properties.UnicastAddresses.Count > 0)
                {

                    ipAddress = properties.UnicastAddresses[0].Address.ToString();
                    Console.WriteLine("IP Address：" + ipAddress);
                    emailSender.Body = ipAddress;
                }

                if(counter > 0) //send email if connection failed
                {
                    emailSender.SendEmail();
                }
            }
            catch
            { 
                //do nothing 
            }
        }
    }
}
