using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KMS_Helper
{
    public sealed class WlanSsid
    {
        public byte[] Ssid { get; set; }

        public WlanSsid()
        {
            Ssid = new byte[32];
        }

        public Wlan.Dot11Ssid ToSsid()
        {
            Wlan.Dot11Ssid wlanSsid = new Wlan.Dot11Ssid();
            if (Ssid == null) return wlanSsid;
            wlanSsid.SSID = Ssid;
            wlanSsid.SSIDLength = Convert.ToUInt32(Ssid.Count(x => x != 0));
            return wlanSsid;
        }
    }
}
