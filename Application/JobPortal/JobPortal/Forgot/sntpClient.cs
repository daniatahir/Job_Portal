using System;
using System.Net;
using System.Net.Mail;

namespace JobPortal.Forgot
{
    internal class smtpClient
    {
        public string Host { get; internal set; }
        public bool UseDefaultCredentials { get; internal set; }
        public object DeliveryMethod { get; internal set; }
        public NetworkCredential Credentials { get; internal set; }
        public int Port { get; internal set; }
        public bool EnableSsl { get; internal set; }

        internal void Send(MailMessage mailMessage)
        {
            throw new NotImplementedException();
        }
    }
}