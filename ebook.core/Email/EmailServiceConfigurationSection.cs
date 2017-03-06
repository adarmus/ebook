using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Email
{
    public class EmailServiceConfigurationSection : ConfigurationSection
    {
        const string SECTION_NAME = "emailService";

        public static EmailServiceConfigurationSection GetConfigSection()
        {
            return GetConfigurationSection<EmailServiceConfigurationSection>(SECTION_NAME);
        }

        static T GetConfigurationSection<T>(string sectionName) where T : class
        {
            object osection = ConfigurationManager.GetSection(sectionName);

            if (osection == null)
                throw new ConfigurationErrorsException(string.Format("Section '{0}' not found", sectionName));

            var section = osection as T;

            if (section == null)
                throw new ConfigurationErrorsException(string.Format("Section '{0}' is not of type {1}", sectionName, typeof(T).Name));

            return section;
        }

        public EmailServiceConfiguration GetConfiguration()
        {
            return new EmailServiceConfiguration
            {
                EnableSsl = EnableSsl,
                FromEmail = FromEmail,
                FromName = FromName,
                SmtpPassword = SmtpPass,
                SmtpPort = SmtpPort,
                SmtpServer = SmtpServer,
                SmtpUsername = SmtpUser
            };
        }

        [ConfigurationProperty("smtpServer", IsRequired = true)]
        public string SmtpServer
        {
            get { return (string)this["smtpServer"]; }
            set { this["smtpServer"] = value; }
        }

        [ConfigurationProperty("smtpPort", IsRequired = true)]
        public int SmtpPort
        {
            get { return (int)this["smtpPort"]; }
            set { this["smtpPort"] = value; }
        }

        [ConfigurationProperty("smtpUser", IsRequired = true)]
        public string SmtpUser
        {
            get { return (string)this["smtpUser"]; }
            set { this["smtpUser"] = value; }
        }

        [ConfigurationProperty("smtpPass", IsRequired = true)]
        public string SmtpPass
        {
            get { return (string)this["smtpPass"]; }
            set { this["smtpPass"] = value; }
        }

        [ConfigurationProperty("enableSsl", IsRequired = true)]
        public bool EnableSsl
        {
            get { return (bool)this["enableSsl"]; }
            set { this["enableSsl"] = value; }
        }

        [ConfigurationProperty("fromName", IsRequired = true)]
        public string FromName
        {
            get { return (string)this["fromName"]; }
            set { this["fromName"] = value; }
        }

        [ConfigurationProperty("fromEmail", IsRequired = true)]
        public string FromEmail
        {
            get { return (string)this["fromEmail"]; }
            set { this["fromEmail"] = value; }
        }
    }
}
