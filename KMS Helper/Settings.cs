using NativeWifi;
using System.ComponentModel;

namespace KMS_Helper
{
    public static class Settings
    {
        public static JsonHandler jsonHandler;
        public static string kmsHost { get; } = "10.0.0.2";
        public static int kmsPort { get; } = 800;
        public static bool preventSave { get; set; }
        private static Setting setting;
        public static Wlan.Dot11Ssid standardSsid { get { return setting.standardSsid.ToSsid(); } set { setting.standardSsid.Ssid = value.SSID; } }
        public static BindingList<Proxy> proxies { get { return setting.proxies; } set { setting.proxies = value; } }
        public static int selectedProxy { get { return setting.selectedProxy; } set { setting.selectedProxy = value; } }
        public static bool autoStart { get { return setting.autoStart; } set { setting.autoStart = value; } }
        public static bool autoStartRunBackground { get { return setting.autoStartRunBackground; } set { setting.autoStartRunBackground = value; } }
        public static Point windowPosition { get { return setting.windowPosition; } set { setting.windowPosition = value; } }

        public static void Init()
        {
            jsonHandler = new JsonHandler();
            setting = jsonHandler.GetSettings();
            if (setting == null)
            {
                setting = new Setting();
                Save();
            }
            else if (proxies.Count == 0)
            {
                proxies.Add(new Proxy { proxyHost = kmsHost, proxyPort = kmsPort });
            }
        }

        public static void Save()
        {
            if (preventSave) return;
            jsonHandler.WriteSettings(setting);
        }
        public static void Save(bool async)
        {
            if (preventSave) return;
            jsonHandler.WriteSettings(setting, async);
        }

        public static void Reset()
        {
            setting = new Setting();
        }

        public static void Reload()
        {
            Setting newSetting = jsonHandler.GetSettings();
            if (newSetting == null)
                newSetting = setting;
            else if (newSetting.proxies.Count == 0)
                newSetting.proxies.Add(new Proxy { proxyHost = kmsHost, proxyPort = kmsPort });

            setting = newSetting;
            Save();
        }
    }
}
