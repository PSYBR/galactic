using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Galactic
{
    public partial class SettingsConfigurationWindow : Window
    {
        private string _deviceId;

        public SettingsConfigurationWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;
            LoadSettings();
        }

        private void LoadSettings()
        {
            string usbDebug = SamsungSDKHelper.GetDeviceProperty(_deviceId, "persist.sys.usb.config");
            string showTaps = SamsungSDKHelper.GetDeviceProperty(_deviceId, "debug.layout");
            SettingsText.Text = $"Current Settings:\nUSB Config: {usbDebug}\nDebug Layout: {showTaps}\n";
            UsbDebuggingCheckBox.IsChecked = usbDebug.ToLower().Contains("adb");
            ShowTapsCheckBox.IsChecked = showTaps.ToLower().Contains("true");
        }

        private void SynchronizeButton_Click(object sender, RoutedEventArgs e)
        {
            string usbSetting = UsbDebuggingCheckBox.IsChecked == true ? "adb" : "none";
            string tapsSetting = ShowTapsCheckBox.IsChecked == true ? "true" : "false";

            string resultUsb = SamsungSDKHelper.SetDeviceProperty(_deviceId, "persist.sys.usb.config", usbSetting);
            string resultTaps = SamsungSDKHelper.SetDeviceProperty(_deviceId, "debug.layout", tapsSetting);

            MessageBox.Show("Settings synchronized successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadSettings();
        }

        private void OpenLogsFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string logsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logsFolder))
            {
                Directory.CreateDirectory(logsFolder);
            }
            Process.Start(new ProcessStartInfo("explorer.exe", logsFolder) { UseShellExecute = true });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
