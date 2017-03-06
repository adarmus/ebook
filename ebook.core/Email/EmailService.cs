using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ebook.core.Logging;

namespace ebook.core.Email
{
    public class EmailService
    {
        readonly ITraceLogger _log;
        readonly EmailServiceConfiguration _config;

        public EmailService(ITraceLogger log, EmailServiceConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public void SendHtmlEmail(string subject, string body, byte[] bytes, string attachmentName, IEnumerable<string> recipients)
        {
            try
            {
                MailMessage email = MakeMailMessage(subject, recipients);

                email.IsBodyHtml = true;
                email.Body = body;

                using (var ms = new MemoryStream(bytes))
                {
                    //email.Attachments.Add(new Attachment(ms, attachmentName));

                    SendEmailMessage(email);
                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                ErrorSending(ex);
            }
            catch (SmtpFailedRecipientException ex)
            {
                ErrorSending(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        MailMessage MakeMailMessage(string subject, IEnumerable<string> recipients)
        {
            var email = new MailMessage();

            email.Subject = subject;
            email.From = new MailAddress(_config.FromEmail, _config.FromName);
            email.To.Add(MakeMailAddresses(recipients).ToString());

            return email;
        }

        void SendEmailMessage(MailMessage email)
        {
            SmtpClient client = new SmtpClient(_config.SmtpServer);
            client.EnableSsl = _config.EnableSsl;
            client.Port = _config.SmtpPort;

            if (!string.IsNullOrEmpty(_config.SmtpUsername))
                client.Credentials = new System.Net.NetworkCredential(_config.SmtpUsername, _config.SmtpPassword);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            _log.WriteLineDebug("Sending email to: {0}", email.To.ToString());

            client.Send(email);
        }

        MailAddressCollection MakeMailAddresses(IEnumerable<string> recipients)
        {
            var addresses = new MailAddressCollection();

            foreach (string recipient in recipients)
            {
                addresses.Add(new MailAddress(recipient));
            }

            return addresses;
        }

        void ErrorSending(SmtpFailedRecipientsException ex)
        {
            foreach (SmtpFailedRecipientException recip in ex.InnerExceptions)
            {
                ErrorSending(recip);
            }
        }

        void ErrorSending(SmtpFailedRecipientException ex)
        {
            _log.WriteLineDebug("Error sending email to {0}: {1}", ex.FailedRecipient, ex.Message);
        }
    }
}
