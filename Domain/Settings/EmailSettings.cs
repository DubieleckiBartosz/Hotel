using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Settings
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}
