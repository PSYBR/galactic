using System;
using System.Windows;
using System.Windows.Threading;

namespace Galactic
{
    public partial class MainWindow : Window
    {
        private string _deviceId;
        private DispatcherTimer _timer;

        public MainWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;
            
            // Retrieve device model and storage information.
            string model = SamsungSDKHelper.GetDeviceProperty(_deviceId, "ro.product.model");
            var hwInfo = SamsungSDKHelper.GetDeviceHardwareInfo(_deviceId);
            // Display device model with total storage (e.g., "SAMSUNG Galaxy S25 (128GB)")
            DeviceModelText.Text = $"{model} ({hwInfo.TotalStorage}GB)";
            DeviceInfoText.Text = $"Serial: {_deviceId}";
            DiagnosticsText.Text = SamsungSDKHelper.GetDeviceDiagnostics(_deviceId);
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateHardwareInfo();
        }

        private void UpdateHardwareInfo()
        {
            var info = SamsungSDKHelper.GetDeviceHardwareInfo(_deviceId);
            double memPercent = ((double)info.UsedMemory / info.TotalMemory) * 100;
            MemoryProgressBar.Value = memPercent;
            MemoryText.Text = $"{info.UsedMemory} MB / {info.TotalMemory} MB ({memPercent:0}%)";

            double storPercent = ((double)info.UsedStorage / info.TotalStorage) * 100;
            StorageProgressBar.Value = storPercent;
            StorageText.Text = $"{info.UsedStorage} GB / {info.TotalStorage} GB ({storPercent:0}%)";

            HardwareInfoText.Text = $"Memory: {info.UsedMemory} MB of {info.TotalMemory} MB\n" +
                                      $"Storage: {info.UsedStorage} GB of {info.TotalStorage} GB\n" +
                                      $"OS: {info.OSVersion}\n" +
                                      $"CPU Usage: {info.CPUUsage}%\n" +
                                      $"Model: {info.DeviceModel}";
            BatteryInfoText.Text = $"Battery Level: {info.BatteryLevel}%";
            TemperatureText.Text = $"Temperature: {info.Temperature} °C";

            // Show S-Pen Calibration button only if device model indicates an Ultra device.
            SPenCalibrateButton.Visibility = info.DeviceModel.ToLower().Contains("ultra") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void RemoteViewButton_Click(object sender, RoutedEventArgs e)
        {
            RemoteViewWindow remoteView = new RemoteViewWindow(_deviceId);
            remoteView.Owner = this;
            remoteView.Show();
        }

        private void SPenCalibrateButton_Click(object sender, RoutedEventArgs e)
        {
            SPenCalibrationWindow calibrationWindow = new SPenCalibrationWindow(_deviceId);
            calibrationWindow.Owner = this;
            calibrationWindow.ShowDialog();
        }

        private void RunDiagnosticsButton_Click(object sender, RoutedEventArgs e)
        {
            DiagnosticsWindow diagWindow = new DiagnosticsWindow(_deviceId);
            diagWindow.Owner = this;
            diagWindow.ShowDialog();
        }

        private void ConfigureSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsConfigurationWindow settingsWindow = new SettingsConfigurationWindow(_deviceId);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private void GVisionButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the interactive G-Vision window.
            GVisionWindow gvision = new GVisionWindow(_deviceId);
            gvision.Owner = this;
            gvision.Show();
        }
    }
}
