/*using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Description;

namespace JobPortal.Forgot
{
    public class EmailSend
    {
        public static object SntpDeliveryMethod { get; private set; }

        public static bool SendEmail(string SenderEmail,string subject,string Message,bool isBodyHtml = false)
        {
            bool status = false;
            try
            {
                string HostAddress = ConfigurationManager.AppSettings["Host"].ToString();
                string FormEmailId = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string Password = ConfigurationManager.AppSettings["Password"].ToString();
                string Port = ConfigurationManager.AppSettings["Port"].ToString();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FormEmailId);
                mailMessage.Subject = subject;
                mailMessage.Body = Message;
                mailMessage.IsBodyHtml = isBodyHtml;
                mailMessage.To.Add(new MailAddress(SenderEmail));
                smtpClient smtp = new smtpClient();
                smtp.Host = HostAddress;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                NetworkCredential networkCredential = new NetworkCredential();
                networkCredential.UserName = mailMessage.From.Address;
                networkCredential.Password = Password;
                smtp.Credentials = networkCredential;
                smtp.Port = Convert.ToInt32(Port);
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
                status = true;
                return status;
            }
            catch(Exception e)
            {
                return status;
            }
        }
       
       
       

   
}

}*/
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace JobPortal.Forgot
{
    public class EmailSend
    {
        public static bool SendEmail(string recipientEmail, string subject, string message, bool isBodyHtml = false)
        {
            try
            {
                // Retrieve email configuration from Web.config
                string host = ConfigurationManager.AppSettings["Host"];
                string senderEmail = ConfigurationManager.AppSettings["MailFrom"];
                string password = ConfigurationManager.AppSettings["Password"];
                int port = int.Parse(ConfigurationManager.AppSettings["Port"]);

                // Configure SMTP client
                using (var smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(senderEmail, password);
                    smtp.EnableSsl = true;

                    // Create email message
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = isBodyHtml
                    };
                    mailMessage.To.Add(recipientEmail);

                    // Send email
                    smtp.Send(mailMessage);
                }

                return true;
            }
            catch (Exception)
            {
                // Log error if necessary
                return false;
            }
        }
    }
}
