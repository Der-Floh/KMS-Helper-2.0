using NativeWifi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS_Helper
{
    public sealed class Setting
    {
        public WlanSsid standardSsid { get; set; }
        public BindingList<Proxy> proxies { get; set; }
        public int selectedProxy { get; set; }
        public bool autoStart { get; set; }
        public bool autoStartRunBackground { get; set; }
        public Point windowPosition { get; set; }

        public Setting()
        {
            standardSsid = new WlanSsid();
            proxies = new BindingList<Proxy>();
            windowPosition = new Point();
            proxies.Add(new Proxy { proxyHost = Settings.kmsHost, proxyPort = Settings.kmsPort });
        }
    }
}
