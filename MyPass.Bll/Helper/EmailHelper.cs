using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyPass.Bll.Helper
{
    public class EmailHelper
    {
        public void SendEmail(string subject, string message, string to)
        {
            string appName = ConfigHelper.GetSetting<string>("AppName");

            string host = ConfigHelper.GetSetting<string>("MailHost");
            int port = ConfigHelper.GetSetting<int>("MailPort");
            string user = ConfigHelper.GetSetting<string>("MailUser");
            string password = ConfigHelper.GetSetting<string>("MailPassword");

            SmtpClient client = new SmtpClient
            {
                Host = host,
                Port = port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password)
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(user, appName);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            client.Send(mailMessage);
        }

        
    }
}
