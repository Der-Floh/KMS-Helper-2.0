using Microsoft.Win32.TaskScheduler;
using NativeWifi;
using System.Text;
using Timer = System.Threading.Timer;

namespace KMS_Helper
{
    public static class Program
    {
        private static WlanClient client = new WlanClient();
        public static List<Wlan.Dot11Ssid> networkList = new List<Wlan.Dot11Ssid>();
        public static Dictionary<Wlan.Dot11Ssid, string> networkDictionary = new Dictionary<Wlan.Dot11Ssid, string>();
        private static TaskService taskService = new TaskService();
        private static TaskDefinition taskDefinition;
        public static bool TaskExists;
        private static Timer scanTimer;
        private static Timer proxyCheckTimer;
        private static int maxScanCount = 40;
        private static int scanInvokeCount = 0;
        private static string logonTaskName = "KMS-Helper-WlanCheck";

        [STAThread]
        static void Main()
        {
            Settings.Init();
            string[] args = Environment.GetCommandLineArgs();
            /*
            if (Settings.autoStartRunBackground && Settings.autoStart && Array.Exists(args, x => x == "runbg"))
            {
                scanTimer = new Timer(ScanNetworks, null, 500, Timeout.Infinite);
            }
            else
            {
                ApplicationConfiguration.Initialize();
                MainForm mainForm = new MainForm();
                mainForm.ShowForm();
            }*/
            ApplicationConfiguration.Initialize();
            MainForm mainForm = new MainForm();
            if (Settings.autoStartRunBackground && Settings.autoStart && Array.Exists(args, x => x == "runbg"))
                mainForm.HideForm();
            else
                mainForm.ShowForm();
            Application.Run();
        }

        private static void ScanNetworks(object stateInfo)
        {
            scanInvokeCount++;
            PullNetworkList();
            networkDictionary.Clear();
            foreach (Wlan.Dot11Ssid network in networkList)
            {
                networkDictionary.Add(network, Encoding.ASCII.GetString(network.SSID, 0, (int)network.SSIDLength));
                if (WlanSsidEqual(network, Settings.standardSsid))
                {
                    scanInvokeCount = maxScanCount;
                    int selectedProxy = 0;
                    RegeditHelper.ChangeInternetSettings(1);
                    if (Settings.selectedProxy == -1)
                        return;
                    else if (Settings.selectedProxy < Settings.proxies.Count)
                        selectedProxy = Settings.selectedProxy;
                    RegeditHelper.SetProxy(Settings.proxies[selectedProxy].proxyHost, Settings.proxies[selectedProxy].proxyPort.ToString());
                    Application.Exit();
                }
            }
            if (scanInvokeCount < maxScanCount)
            {
                scanTimer.Change(500, Timeout.Infinite);
            }
        }

        public static List<Wlan.Dot11Ssid> PullNetworkList()
        {
            List<Wlan.Dot11Ssid> networkListOld = new List<Wlan.Dot11Ssid>(networkList);
            networkList.Clear();
            Wlan.Dot11Ssid ssidOld;
            Wlan.Dot11Ssid ssidNew;
            ssidOld.SSID = new byte[1];

            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                wlanIface.Scan();
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    ssidNew = network.dot11Ssid;
                    if (!ssidNew.SSID.SequenceEqual(ssidOld.SSID))
                    {
                        ssidOld = ssidNew;
                        string networkname = Encoding.ASCII.GetString(ssidNew.SSID, 0, (int)ssidNew.SSIDLength);
                        if (networkname != "")
                        {
                            networkList.Add(ssidNew);
                        }
                    }
                }
            }
            return networkListOld;
        }

        public static bool ListsEqual(List<Wlan.Dot11Ssid> _networkList1, List<Wlan.Dot11Ssid> _networkList2)
        {
            Wlan.Dot11Ssid[] networkList1 = _networkList1.ToArray();
            Wlan.Dot11Ssid[] networkList2 = _networkList2.ToArray();
            for (int i = 0; i < networkList1.Length; i++)
            {
                for (int j = 0; j < networkList1[i].SSID.Length; j++)
                {
                    try
                    {
                        if (networkList1[i].SSID[j] != networkList2[i].SSID[j]) return false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool WlanSsidEqual(Wlan.Dot11Ssid _ssid1, Wlan.Dot11Ssid _ssid2)
        {
            if (_ssid1.SSIDLength != _ssid2.SSIDLength) return false;
            byte[] ssid1 = _ssid1.SSID;
            byte[] ssid2 = _ssid2.SSID;
            for (int i = 0; i < ssid1.Length; i++)
            {
                if (ssid1[i] != ssid2[i]) return false;
            }
            return true;
        }

        public static void CreateLogonWindowsTask()
        {
            taskDefinition = taskService.NewTask();

            //taskDefinition.Settings.RunOnlyIfLoggedOn = true;
            taskDefinition.Settings.AllowDemandStart = true;
            taskDefinition.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;

            taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
            taskDefinition.Principal.LogonType = TaskLogonType.InteractiveToken;

            taskDefinition.RegistrationInfo.Description = "Checks if the current Wlan is the set standard and if so enables associated Proxy";
            taskDefinition.RegistrationInfo.Author = "KMS-Helper 2.0";

            taskDefinition.Triggers.Add(new LogonTrigger());

            string taskArgs = "";
            if (Settings.autoStartRunBackground) taskArgs = "runbg";
            ExecAction action = new ExecAction(Application.ExecutablePath, taskArgs);
            taskDefinition.Actions.Add(action);

            taskService.RootFolder.RegisterTaskDefinition(logonTaskName, taskDefinition);
            TaskExists = true;
        }

        public static void RemoveLogonWindowsTask()
        {
            try
            {
                taskService.RootFolder.DeleteTask(logonTaskName);
            }
            catch { }
            TaskExists = false;
        }

        public static bool LogonTaskExists()
        {
            try
            {
                TaskCollection taskCollection = taskService.RootFolder.Tasks;
                return taskCollection.Exists(logonTaskName);
            }
            catch { return false; }
        }
        public static bool LogonTaskRunBG()
        {
            try
            {
                TaskCollection taskCollection = taskService.RootFolder.Tasks;
                Microsoft.Win32.TaskScheduler.Task task = taskCollection.First(x => x.Name == logonTaskName);
                ExecAction action = (ExecAction)task.Definition.Actions.Find(x => ((ExecAction)x).Path == Application.ExecutablePath);
                return action.Arguments == "runbg";
            }
            catch { return false; }
        }
    }
}