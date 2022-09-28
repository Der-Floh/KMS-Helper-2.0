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
