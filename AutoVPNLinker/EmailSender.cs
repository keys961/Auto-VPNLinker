using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoVPNLinker
{
    public class EmailSender
    {
        private string host;

        private string userName;

        private string password;

        private string title;

        private string body;

        public string Body { get => body; set => body = value; }

        private void ParseHost(string username)
        {
            string[] hosts = userName.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            this.host = "smtp." + hosts[1];
        }

        public EmailSender(string userName, string password, string title, string body)
        {
            this.userName = userName;
            this.password = password;
            this.title = title;
            this.Body = body;
            ParseHost(userName);
        }

        public void SendEmail()
        {
            SmtpClient client = new SmtpClient(host);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.Port = 25;
            client.Credentials = new System.Net.NetworkCredential(userName, password);

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(userName, "Your PC");
            msg.To.Add(userName);

            msg.Subject = title;   
            msg.Body = "Current IP address: " + Body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;   
            msg.IsBodyHtml = true;

            try
            {
                client.Send(msg);
            }
            catch(System.Net.Mail.SmtpException e)
            {
                Console.WriteLine(e.Message, "Error when sending email");
            }
        }
    }
}
