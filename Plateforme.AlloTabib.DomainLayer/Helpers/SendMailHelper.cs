using System;
using System.Configuration;
using Plateforme.AlloTabib.DomainLayer.Models;
using EASendMail; //add EASendMail namespace

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class SendMailHelper
    {
        public static void SendEmail(string @from, string to, string subject, string body)
        {
            var initConfiguration = SendMailHelper.InitConfiguration();

            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Your Offic 365 email address
            oMail.From = initConfiguration.EmailSender;//"myid@mydomain";

            // Set recipient email address
            oMail.To = to;//"support@emailarchitect.net";

            // Set email subject
            oMail.Subject = subject;

            // Set email body
            oMail.TextBody = body;

            // Your Office 365 SMTP server address, 
            // You should get it from outlook web access.
            SmtpServer oServer = new SmtpServer(initConfiguration.SmtpServerAddress);

            // user authentication should use your 
            // email address as the user name. 
            oServer.User = initConfiguration.SmtpUsername;
            oServer.Password = initConfiguration.SmtpPassword;

            // Set 587 port
            oServer.Port = 587;

            // detect SSL/TLS connection automatically
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            try
            {
                Console.WriteLine("start to send email over SSL...");
                oSmtp.SendMail(oServer, oMail);
                Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
                throw;
            }
        }

        
        public static EmailConfiguration InitConfiguration()
        {
            return new EmailConfiguration
            {
                SmtpServerAddress = ConfigurationManager.AppSettings["ServerAddress"],
                SmtpPort = ConfigurationManager.AppSettings["ServerPort"],

                EmailSender = ConfigurationManager.AppSettings["ServerSender"],
                SmtpUsername = ConfigurationManager.AppSettings["ServerUsername"],
                SmtpPassword = ConfigurationManager.AppSettings["ServerPassword"],
                WithCredentialsFlag = Convert.ToBoolean(ConfigurationManager.AppSettings["ServerWithCredentials"]),

            };
        }
    }
}
