using NativeWifi;
using System.ComponentModel;
using System.Text;
using Timer = System.Windows.Forms.Timer;

namespace KMS_Helper
{
    public sealed partial class MainForm : Form
    {
        private BindingList<Proxy> proxies = new BindingList<Proxy>();
        private const string shortcutName = "KMS-Helper";
        private Timer timer;
        private Timer loadingTimer;
        private Wlan.Dot11Ssid currentSsid;
        private Wlan.Dot11Ssid standardSsid;
        private bool systemTrayClose = true;
        private bool autoSetWlan;

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
                if (!IsOnScreen())
                    CenterToScreen();
            }

            Settings.jsonHandler.OnSaved += new MyEventHandler(OnSaved);
            autoSetWlan = Settings.autoStart;

            NetworkListBox.DisplayMember = "Value";
            NetworkListBox.ValueMember = "Key";
            
            timer = new Timer();
            timer.Interval = 200;
            timer.Tick += PullNetworkList;
            timer.Start();

            loadingTimer = new Timer();
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
            List<Wlan.Dot11Ssid> dot11Ssids = Program.PullNetworkList();
            FillNetworkList(dot11Ssids);
            if (autoSetWlan)
            {
                foreach (Wlan.Dot11Ssid network in dot11Ssids)
                {
                    if (Program.WlanSsidEqual(network, Settings.standardSsid))
                    {
                        int selectedProxy = 0;
                        RegeditHelper.ChangeInternetSettings(1);
                        if (Settings.selectedProxy == -1)
                            return;
                        else if (Settings.selectedProxy < Settings.proxies.Count)
                            selectedProxy = Settings.selectedProxy;
                        RegeditHelper.SetProxy(Settings.proxies[selectedProxy].proxyHost, Settings.proxies[selectedProxy].proxyPort.ToString());
                        autoSetWlan = false;
                    }
                }
            }
        }

        private async Task FillNetworkList(List<Wlan.Dot11Ssid> networkListOld)
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
            }
            catch { }
            SettingsStandardWlanTextBox.Text = networkname;
            if (ProxyListBox.SelectedIndex != -1)
            {
                SettingsProxyHostTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyHost;
                SettingsProxyPortTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyPort.ToString();
                proxyHostToolStripTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyHost;
                proxyPortToolStripTextBox.Text = Settings.proxies[ProxyListBox.SelectedIndex].proxyPort.ToString();
            }
            else
            {
                SettingsProxyHostTextBox.Text = "";
                SettingsProxyPortTextBox.Text = "";
                proxyHostToolStripTextBox.Text = "";
                proxyPortToolStripTextBox.Text = "";
            }
            SettingsCheckWlanOnStartCheckBox.Checked = Settings.autoStart;
            SettingsRunBgOnStartCheckBox.Checked = Settings.autoStartRunBackground;
            SettingsMinimizeInTrayCheckBox.Checked = Settings.minimizeInTray;
        }
        private void UpdateSettings()
        {
            Settings.standardSsid = standardSsid;
            Settings.proxies = proxies;
            Settings.selectedProxy = ProxyListBox.SelectedIndex;
            Settings.autoStart = SettingsCheckWlanOnStartCheckBox.Checked;
            Settings.autoStartRunBackground = SettingsRunBgOnStartCheckBox.Checked;
            Settings.minimizeInTray = SettingsMinimizeInTrayCheckBox.Checked;

            Settings.windowPosition = Location;

            if (Settings.autoStart)
            {
                Program.RemoveLogonWindowsTask();
                Program.CreateLogonWindowsTask();
            }
            else if (!Settings.autoStart)
            {
                Program.RemoveLogonWindowsTask();
            }
        }
        public void SettingsReset()
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
        public void DeleteConfig()
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
        public void HideForm()
        {
            Hide();
            SystemTrayIcon.Visible = true;
        }
        public void ShowForm()
        {
            UpdateSettingsPanel();
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            SystemTrayIcon.Visible = false;
        }
        public void CloseProgram()
        {
            UpdateSettings();
            Settings.Save(false);
            SystemTrayIcon.Visible = false;
            Application.Exit();
        }
        public bool IsOnScreen()
        {
            Screen[] screens = Screen.AllScreens;
            foreach (Screen screen in screens)
            {
                Rectangle formRectangle = new Rectangle(Left, Top, Width, Height);

                if (screen.WorkingArea.Contains(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }
        #region FormEvents
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
            SettingsReset();
        }
        private void SetProxyButton_Click(object sender, EventArgs e)
        {
            string proxyHost = SettingsProxyHostTextBox.Text;
            if (proxyHost.Length == 0) return;
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
            CloseProgram();
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
            DeleteConfig();
        }
        private void DeselectProxy_Click(object sender, EventArgs e)
        {
            ProxyListBox.SelectedIndex = -1;
        }

        private void SystemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowForm();
            WindowState = FormWindowState.Normal;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Settings.minimizeInTray)
                HideForm();
        }

        #endregion
        #region SystemTrayEvents
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProgram();
        }

        private void showWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateSettings();
            Settings.Save();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Reload();
            UpdateSettingsPanel();
        }

        private void checkWlanOnWindowsStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsCheckWlanOnStartCheckBox.Checked = checkWlanOnWindowsStartToolStripMenuItem.Checked;
            UpdateSettings();
            Settings.Save();
        }

        private void onAutoStartRunBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsCheckWlanOnStartCheckBox.Checked = checkWlanOnWindowsStartToolStripMenuItem.Checked;
            UpdateSettings();
            Settings.Save();
        }

        private void minimizeInSystemTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsMinimizeInTrayCheckBox.Checked = minimizeInSystemTrayToolStripMenuItem.Checked;
            UpdateSettings();
            Settings.Save();
        }

        private void proxyPortToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void setProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string proxyHost = proxyHostToolStripTextBox.Text;
            if (proxyHost.Length == 0) return;
            int proxyPort = -1;
            if (!int.TryParse(proxyPortToolStripTextBox.Text, out proxyPort)) return;

            RegeditHelper.ChangeInternetSettings(1);
            RegeditHelper.SetProxy(proxyHost, proxyPort.ToString());
        }

        private void removeProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegeditHelper.RemoveProxy();
        }

        private void loadKMSProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            proxyHostToolStripTextBox.Text = Settings.kmsHost;
            proxyPortToolStripTextBox.Text = Settings.kmsPort.ToString();
        }

        private void deleteConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteConfig();
        }

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsReset();
        }

        private void SystemTrayContextMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            e.Cancel = !systemTrayClose;
            systemTrayClose = true;
        }

        private void minimizeInSystemTrayToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            systemTrayClose = false;
        }

        private void onAutoStartRunBackgroundToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            systemTrayClose = false;
        }

        private void checkWlanOnWindowsStartToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            systemTrayClose = false;
        }

        private void resetWindowLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterToScreen();
        }
        #endregion
    }
}