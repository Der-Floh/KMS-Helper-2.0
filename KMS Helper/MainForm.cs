using KMS_Toolkit;
using NativeWifi;
using System.ComponentModel;
using System.Text;

namespace KMS_Helper
{
    public sealed partial class MainForm : Form
    {
        private BindingList<Proxy> proxies = new BindingList<Proxy>();
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
            FillNetworkList(Program.PullNetworkList());
        }

        private async System.Threading.Tasks.Task FillNetworkList(List<Wlan.Dot11Ssid> networkListOld)
        {
            if (Program.networkList.Count != 0 && !Program.ListsEqual(Program.networkList, networkListOld))
            {
                int selected = NetworkListBox.SelectedIndex;
                Program.networkDictionary.Clear();
                foreach (Wlan.Dot11Ssid network in Program.networkList)
                {
                    Program.networkDictionary.Add(network, Encoding.ASCII.GetString(network.SSID, 0, (int)network.SSIDLength));
                }
                NetworkListBox.DataSource = new BindingSource(Program.networkDictionary, null);
                NetworkListBox.SelectedIndex = selected;
            }
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

            if (Settings.autoStart && !Program.TaskExists)
            {
                Program.CreateLogonWindowsTask();
            }
            else if (!Settings.autoStart)
            {
                Program.RemoveLogonWindowsTask();
            }
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
            Settings.proxies = proxies;
            Settings.selectedProxy = ProxyListBox.SelectedIndex;
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

        private void DeselectProxy_Click(object sender, EventArgs e)
        {
            ProxyListBox.SelectedIndex = -1;
        }
    }
}