﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebook.core.Email
{
    public class EmailServiceConfiguration
    {
        public string SmtpServer { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool EnableSsl { get; set; }

        public int SmtpPort { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }
    }
}
