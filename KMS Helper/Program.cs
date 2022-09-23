using NativeWifi;
using System.Text;

namespace KMS_Helper
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Settings.Init();
            if (Settings.autoStartRunBackground && Settings.autoStart)
            { }
            else
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainForm());
            }
        }

        static void SetProxy()
        {

        }
        static void GetAvailableWifi()
        {
            List<string> networkList = new List<string>();
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    Wlan.Dot11Ssid ssid = network.dot11Ssid;
                    string networkname = Encoding.ASCII.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
                    if (networkname != "")
                    {
                        networkList.Add(networkname.ToString());
                    }
                }
            }
        }
    }
}