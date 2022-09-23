using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS_Helper
{
    public sealed class Proxy
    {
        public string proxyHost { get; set; }
        public int proxyPort { get; set; }

        public override string ToString()
        {
            return "Host: " + proxyHost + "  |  " + "Port: " + proxyPort;
        }
    }
}
