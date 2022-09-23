using KMS_Toolkit;
using NativeWifi;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace KMS_Helper
{
    public sealed partial class MainForm : Form
    {
        private WlanClient client = new WlanClient();
        private List<Wlan.Dot11Ssid> networkList = new List<Wlan.Dot11Ssid>();
        private BindingList<Proxy> proxies = new BindingList<Proxy>();
        private Dictionary<Wlan.Dot11Ssid, string> networkDictionary = new Dictionary<Wlan.Dot11Ssid, string>();
        private const string shortcutName = "KMS-Helper";
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Timer loadingTimer;
        private Wlan.Dot11Ssid currentSsid;
        private Wlan.Dot11Ssid standardSsid;

        public MainForm()
        {
            InitializeComponent();

            if (Settings.windowPosition.IsEmpty)
            {
                CenterToScreen();
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
                Location = Settings.windowPosition;
            }

            Settings.jsonHandler.OnSaved += new MyEventHandler(OnSaved);

            NetworkListBox.DisplayMember = "Value";
            NetworkListBox.ValueMember = "Key";

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 200;
            timer.Tick += PullNetworkList;
            timer.Start();

            loadingTimer = new System.Windows.Forms.Timer();
            loadingTimer.Interval = 200;
            loadingTimer.Tick += LoadingSpinner;

            currentSsid = Settings.standardSsid;
            standardSsid = Settings.standardSsid;

            int selectedProxy = Settings.selectedProxy;
            proxies = Settings.proxies;
            ProxyListBox.DataSource = proxies;
            ProxyListBox.DisplayMember = "ToString";
            if (selectedProxy >= ProxyListBox.Items.Count)
                ProxyListBox.SelectedIndex = 0;
            else
                ProxyListBox.SelectedIndex = selectedProxy;
            UpdateSettingsPanel();
        }

        private void OnSaved(object source, MyEventArgs e)
        {
            if (Settings.preventSave) return;
            LoadingSpinnerPictureBox.Visible = false;
            loadingTimer.Stop();
            SettingsSavedLabel.Text = e.GetInfo();
            SettingsSavedLabel.Visible = true;
        }

        private void LoadingSpinner(object sender, EventArgs e)
        {
            Image img = LoadingSpinnerPictureBox.Image;
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            LoadingSpinnerPictureBox.Image = img;
        }

        private void PullNetworkList(object sender, EventArgs e)
        {
            List<Wlan.Dot11Ssid> networkListOld = new List<Wlan.Dot11Ssid>(networkList);
            networkList.Clear();
            Wlan.Dot11Ssid ssidOld;
            Wlan.Dot11Ssid ssidNew;
            ssidOld.SSID = new byte[1];

            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
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
            FillNetworkList(networkListOld);
        }

        private async Task FillNetworkList(List<Wlan.Dot11Ssid> networkListOld)
        {
            if (networkList.Count != 0 && !ListsEqual(networkList, networkListOld))
            {
                int selected = NetworkListBox.SelectedIndex;
                networkDictionary.Clear();
                foreach (Wlan.Dot11Ssid network in networkList)
                {
                    networkDictionary.Add(network, Encoding.ASCII.GetString(network.SSID, 0, (int)network.SSIDLength));
                }
                NetworkListBox.DataSource = new BindingSource(networkDictionary, null);
                NetworkListBox.SelectedIndex = selected;
            }
        }

        private bool ListsEqual(List<Wlan.Dot11Ssid> _networkList1, List<Wlan.Dot11Ssid> _networkList2)
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

        private void AddShortcutInStartmenu()
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + shortcutName + ".url"))
            {
                string app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=file:///" + app);
                writer.WriteLine("IconIndex=0");
                string icon = app.Replace('\\', '/');
                writer.WriteLine("IconFile=" + icon);
            }
        }

        private void RemoveShortcutInStartmenu()
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (File.Exists(deskDir + "/" + shortcutName + ".url"))
            {
                File.Delete(deskDir + "/" + shortcutName + ".url");
            }
        }

        private void UpdateSettingsPanel()
        {
            string networkname = "";
            try
            {
                networkname = Encoding.ASCII.GetString(Settings.standardSsid.SSID, 0, (int)Settings.standardSsid.SSIDLength);
            } catch { }
            SettingsStandardWlanTextBox.Text = networkname;
            if (ProxyListBox.SelectedIndex != -1)
            {
                SettingsProxyHostTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyHost;
                SettingsProxyPortTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyPort.ToString();
            }
            else
            {
                SettingsProxyHostTextBox.Text = "";
                SettingsProxyPortTextBox.Text = "";
            }
            SettingsCheckWlanOnStartCheckBox.Checked = Settings.autoStart;
            SettingsRunBgOnStartCheckBox.Checked = Settings.autoStartRunBackground;
        }
        private void UpdateSettings()
        {
            Settings.standardSsid = standardSsid;
            Settings.proxies = proxies;
            Settings.selectedProxy = ProxyListBox.SelectedIndex;
            Settings.autoStart = SettingsCheckWlanOnStartCheckBox.Checked;
            Settings.autoStartRunBackground = SettingsRunBgOnStartCheckBox.Checked;

            Settings.windowPosition = Location;
        }

        private void NetworkListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NetworkListBox.SelectedIndex == -1 || NetworkListBox.Items.Count == 0) return;
            KeyValuePair<Wlan.Dot11Ssid, string> wlan = (KeyValuePair<Wlan.Dot11Ssid, string>)NetworkListBox.SelectedItem;
            currentSsid = wlan.Key;
            //string networkname = Encoding.ASCII.GetString(currentSsid.SSID, 0, (int)currentSsid.SSIDLength);
            CurrentWlanTextBox.Text = wlan.Value;
        }

        private void SetStandardWlanButton_Click(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
            if (!Settings.preventSave)
            {
                loadingTimer.Start();
                LoadingSpinnerPictureBox.Visible = true;
            }
            standardSsid = currentSsid;
            Settings.standardSsid = standardSsid;
            Settings.Save();
            UpdateSettingsPanel();
        }

        private void SettingsSaveButton_Click(object sender, EventArgs e)
        {
            if (!Settings.preventSave)
            {
                loadingTimer.Start();
                LoadingSpinnerPictureBox.Visible = true;
            }
            UpdateSettings();
            Settings.Save();
        }

        private void SettingsKMSStandardButton_Click(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
            string proxyHost = SettingsProxyHostTextBox.Text;
            int proxyPort = -1;
            int.TryParse(SettingsProxyPortTextBox.Text, out proxyPort);

            if (!proxies.ToList().Exists(p => p.proxyHost == proxyHost && p.proxyPort == proxyPort))
            {
                proxies.Add(new Proxy { proxyHost = proxyHost, proxyPort = proxyPort });
                //ProxyListBox.DataSource = proxies;
                //ProxyListBox.Update();
                /*
                ProxyListBox.Items.Clear();
                foreach (Proxy proxy in proxies)
                {
                    ProxyListBox.Items.Add(proxy);
                }
                */
            }
        }

        private void SettingsResetButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset the settings to default?", "Reset Settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                SettingsSavedLabel.Visible = false;
                loadingTimer.Start();
                LoadingSpinnerPictureBox.Visible = true;
                Settings.Reset();
                Settings.Save();
                UpdateSettingsPanel();
            }
        }

        private void SetProxyButton_Click(object sender, EventArgs e)
        {
            string proxyHost = SettingsProxyHostTextBox.Text;
            int proxyPort = -1;
            if (!int.TryParse(SettingsProxyPortTextBox.Text, out proxyPort)) return;

            RegeditHelper.ChangeInternetSettings(1);
            RegeditHelper.SetProxy(proxyHost, proxyPort.ToString());
        }

        private void RemoveProxyButton_Click(object sender, EventArgs e)
        {
            RegeditHelper.RemoveProxy();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateSettings();
            Settings.Save();
        }

        private void SettingsProxyHostTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
        }

        private void SettingsProxyPortTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
        }

        private void SettingsCheckWlanOnStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
        }

        private void SettingsRunBgOnStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingsSavedLabel.Visible = false;
        }

        private void ProxyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSettings();
            UpdateSettingsPanel();
        }

        private void RemoveProxyListButton_Click(object sender, EventArgs e)
        {
            if (proxies[ProxyListBox.SelectedIndex].proxyHost == Settings.kmsHost && proxies[ProxyListBox.SelectedIndex].proxyPort == Settings.kmsPort)
            {
                MessageBox.Show("You can't remove the Default Proxy.", "Remove Proxy Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            proxies.RemoveAt(ProxyListBox.SelectedIndex);
        }

        private void SettingsProxyPortTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SettingsDeleteConfigButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete the config file?\nThis will reset all Settings to their default value", "Delete Config?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Settings.preventSave = true;
                SettingsSavedLabel.Visible = false;
                JsonHandler jsonHandler = new JsonHandler();
                jsonHandler.DeleteSettings();
            }
        }
    }
}