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
        //public static EmailSenderVariables EmailSenderVals;
        /*private string HtmlBody = @"
        fasdgasfgdf
";*/
        public static void SendEmail(string email, string callbackUrl)
        {
            try
            {
                //Smtp
                SmtpClient smtpClient = new SmtpClient()
                {
                    //Host = "smtp.yandex.com",
                    Host = "smtp.office365.com",
                    //Host="smtp.expert-qs.com",
                    //Port = 587,
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 60000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = false,
                    //TargetName = "STARTTLS/outlook.office365.com"
                    Credentials = new NetworkCredential("noreply@expert-qs.com", "Yak10236")
                    //Credentials = new NetworkCredential("bugra.kaya@many-points.com", "bugra-ajans2021")
                };
                MailMessage msg = new MailMessage();
                msg.To.Add(email);
                msg.From = new MailAddress("noreply@expert-qs.com", "Expert Quality Services");
                //msg.From = new MailAddress("bugra.kaya@many-points.com", "Expert Quality Services");
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
