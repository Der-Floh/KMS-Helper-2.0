using Microsoft.Win32;

namespace KMS_Helper
{
    public static class RegeditHelper
    {
        public static void ChangeInternetSettings(int newValue)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string keyName = "ProxySettingsPerUser";
            //adding/editing a value 
            key.SetValue(keyName, newValue);
            key.Close();
        }

        public static string ReadInternetSettings()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            //getting the value
            string data = key.GetValue("ProxySettingsPerUser").ToString();  //returns the text found in 'someValue'

            key.Close();
            return data;
        }

        public static string ReadCurrentUserKeyValue(in string path, in string keyName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true);

            //getting the value
            string data = key.GetValue(keyName).ToString();  //returns the text found in 'someValue'

            key.Close();
            return data;
        }

        public static string ReadLocalMachineKeyValue(in string path,in string keyName)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);

            //getting the value
            string data = key.GetValue(keyName).ToString();  //returns the text found in 'someValue'

            key.Close();
            return data;
        }
 
        public static void SetProxy(string Host, string Port)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string ProxyServerName = "ProxyServer";
            //adding/editing a value 
            key.SetValue(ProxyServerName, Host + ":" + Port);
            key.Close();

            key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string ProxyEnableName = "ProxyEnable";
            //adding/editing a value 
            key.SetValue(ProxyEnableName, 1);
            key.Close();
        }

        public static string GetProxy()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string KeyName = "ProxyServer";
            //adding/editing a value 
            string proxy = key.GetValue(KeyName).ToString();
            key.Close();
            return proxy;
        }

        public static void RemoveProxy()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string ProxyServerName = "ProxyServer";
            //adding/editing a value 
            key.SetValue(ProxyServerName, "");
            key.Close();

            key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
            string ProxyEnableName = "ProxyEnable";
            //adding/editing a value 
            key.SetValue(ProxyEnableName, 0);
            key.Close();
        }
    }
}
