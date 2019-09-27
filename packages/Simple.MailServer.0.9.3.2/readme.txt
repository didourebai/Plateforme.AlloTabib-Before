// Introduction
// ============
//
// To get you started, here an example implementation that saves incoming e-mails into files.
//
// https://github.com/hanswolff/simple.mailserver
// 
// Mail Server Example:
// ====================

using Simple.MailServer.Logging;
using Simple.MailServer.Smtp;
using Simple.MailServer.Smtp.Config;
using System;
using System.IO;
using System.Text;

namespace Simple.MailServer.Example
{
    class Program
    {
        private const string RootMailDir = @"C:\Temp\Mail\";

        static void Main()
        {
            // to inject your own logging implement IMailServerLogger
            MailServerLogger.Set(new MailServerConsoleLogger(MailServerLogLevel.Debug));

            using (StartSmtpServer())
            {
                Console.WriteLine("Hit ENTER to return.");
                Console.ReadLine();
            }
        }

        private static SmtpServer StartSmtpServer()
        {
            var smtpServer = new SmtpServer
            {
                Configuration =
                {
                    DefaultGreeting = "Simple.MailServer Example"
                }
            };
            smtpServer.DefaultResponderFactory = 
                new DefaultSmtpResponderFactory<ISmtpServerConfiguration>(smtpServer.Configuration)
            {
                DataResponder = new ExampleDataResponder(smtpServer.Configuration, RootMailDir)
                // ... inject other responders here as needed (or leave default)
            };

            smtpServer.BindAndListenTo(IPAddress.Loopback, 25);
            return smtpServer;
        }
    }

    class ExampleDataResponder : DefaultSmtpDataResponder<ISmtpServerConfiguration>
    {
        private readonly string _mailDir;

        public ExampleDataResponder(ISmtpServerConfiguration configuration, string mailDir)
            : base(configuration)
        {
            _mailDir = mailDir;
            EnsureDirExists(mailDir);
        }

        private static void EnsureDirExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public override SmtpResponse DataStart(SmtpSessionInfo sessionInfo)
        {
            Console.WriteLine("Start receiving mail: {0}", GetFileNameFromSessionInfo(sessionInfo));
            return SmtpResponse.DataStart;
        }

        private string GetFileNameFromSessionInfo(SmtpSessionInfo sessionInfo)
        {
            var fileName = sessionInfo.CreatedTimestamp.ToString("yyyy-MM-dd_HHmmss_fff") + ".eml";
            var fullName = Path.Combine(_mailDir, fileName);
            return fullName;
        }

        public override SmtpResponse DataLine(SmtpSessionInfo sessionInfo, byte[] lineBuf)
        {
            var fileName = GetFileNameFromSessionInfo(sessionInfo);

            Console.WriteLine("{0} <<< {1}", fileName, Encoding.UTF8.GetString(lineBuf));

            using (var stream = File.OpenWrite(fileName))
            {
                stream.Seek(0, SeekOrigin.End);
                stream.Write(lineBuf, 0, lineBuf.Length);

                stream.WriteByte(13);
                stream.WriteByte(10);
            }

            return SmtpResponse.None;
        }

        public override SmtpResponse DataEnd(SmtpSessionInfo sessionInfo)
        {
            var fileName = GetFileNameFromSessionInfo(sessionInfo);
            var size = GetFileSize(fileName);

            Console.WriteLine("Mail received ({0} bytes): {1}", size, fileName);

            var successMessage = String.Format("{0} bytes received", size);
            var response = SmtpResponse.OK.CloneAndChange(successMessage);

            return response;
        }

        private long GetFileSize(string fileName)
        {
            return new FileInfo(fileName).Length;
        }
    }
}
