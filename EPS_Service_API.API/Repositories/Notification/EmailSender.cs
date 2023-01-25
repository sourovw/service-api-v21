//using EASendMail;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EPS_Service_API.Repositories
{
    public class EmailSender : IEmailSender
    {
        private string SMTPHost;
        private int Port;
        private bool SSL;
        private bool Authentication;
        private string DisplayName;
        private string DisplayEmail;
        private string ReplyToEmail;
        private string Password;
        static bool enableSSL = true;





        public EmailSender(IConfiguration config)
        {
            SMTPHost = config["EmailSender:SMTPHost"];
            Port = Convert.ToInt32(config["EmailSender:Port"]);
            SSL = Convert.ToBoolean(config["EmailSender:SSL"]);
            Authentication = Convert.ToBoolean(config["EmailSender:Authentication"]);
            DisplayName = config["EmailSender:DisplayName"];
            DisplayEmail = config["EmailSender:DisplayEmail"];
            ReplyToEmail = config["EmailSender:ReplyToEmail"];
            Password = config["EmailSender:Password"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await Task.Run(() => SendEmail(email, subject, message));
        //    await Task.Run(() => SendMailGmail(email, subject, message));
        }

        private void SendEmail(string email, string subject, string message)
        {
            /*
            MailMessage objMessage;
            SmtpClient objClient;

            objMessage = new MailMessage(new MailAddress(DisplayEmail, DisplayName), new MailAddress(email));
            objMessage.ReplyToList.Add(ReplyToEmail);
            objMessage.IsBodyHtml = true;
            objMessage.Subject = subject;
            objMessage.Body = message;
            objMessage.Priority = MailPriority.Normal;

            objClient = new SmtpClient();
            objClient.Port = Port;
            objClient.EnableSsl = SSL;
            objClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            objClient.UseDefaultCredentials = Authentication;
            objClient.Timeout = 30000;
            objClient.Host = SMTPHost;
            if (Authentication)
                objClient.Credentials = new NetworkCredential(DisplayEmail, Password);
            objClient.Send(objMessage);
            */
            try { 
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(DisplayEmail);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(SMTPHost, Port))
                {
                    smtp.Credentials = new NetworkCredential(DisplayEmail, Password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
        
        

        /*
        private void SendMailGmail(string email, string subject, string message)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                // Your gmail email address
                oMail.From = DisplayName;
                // Set recipient email address
                oMail.To = email;
                // Set email subject
                oMail.Subject = subject;
                // Set email body
     
                oMail.HtmlBody = message;
                //oMail.TextBody = message;
                // Gmail SMTP server address
                SmtpServer oServer = new SmtpServer(SMTPHost);
                // Gmail user authentication
                // For example: your email is "gmailid@gmail.com", then the user should be the same
                oServer.User = DisplayName;
                oServer.Password = Password;
                // Set 465 port
                oServer.Port = 465;
                // detect SSL/TLS automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                Console.WriteLine("start to send email over SSL ...");
                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                oSmtp.SendMail(oServer, oMail);
                Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
        */
        


    }
}