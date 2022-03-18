using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using QSCustomer.Utility;

namespace QSCustomer.Extensions
{
    public static class EmailSenderExtension
    {
        public static void SendEmail(string email, string callbackUrl)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient()
                {
                    Host = "smtp-relay.sendinblue.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 60000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential("bugrakaya16@gmail.com", "EyXmRh4HCsYagPpt")
                };
                MailMessage msg = new MailMessage();
                msg.To.Add(email);
                msg.From = new MailAddress("bugrakaya16@gmail.com", "Expert Quality Services");
                msg.Subject = "Expert QS Account Verifying";
                msg.Body = EmailSenderVariables.HtmlBodyBefore+ $"Please confirm your account on <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>This Link</a>." + EmailSenderVariables.HtmlBodyAfter;
                msg.IsBodyHtml = true;
                smtpClient.Send(msg);
                Console.WriteLine("Smtp Gönderildi: " + email);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n\n Error:\n"+e.Message);
            }
        }
        public static void TestRun(string email, string callbackUrl)
        {
            SendEmail(email, callbackUrl);
        }
    }
}
