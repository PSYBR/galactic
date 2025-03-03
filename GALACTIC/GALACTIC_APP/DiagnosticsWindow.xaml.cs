using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Galactic
{
    public partial class DiagnosticsWindow : Window
    {
        private string _deviceId;
        private DispatcherTimer _timer;
        private string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "ScanLog.txt");

        public DiagnosticsWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var diag = SamsungSDKHelper.GetDeviceDiagnostics(_deviceId);
            var advancedDiag = SamsungSDKHelper.RunAdvancedDiagnostics(_deviceId);
            DiagnosticsText.Text = diag + "\n\nAdvanced Diagnostics:\n" + advancedDiag;

            // Update progress bar based on CPU usage (for demo purposes)
            var info = SamsungSDKHelper.GetDeviceHardwareInfo(_deviceId);
            DiagnosticsProgressBar.Value = info.CPUUsage;

            LogDiagnostics(diag, advancedDiag, info);
        }

        private void LogDiagnostics(string diag, string advancedDiag, DeviceHardwareInfo info)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"Timestamp: {timestamp}\n{diag}\nAdvanced Diagnostics:\n{advancedDiag}\n";
                logEntry += $"Hardware Info:\nMemory: {info.UsedMemory} MB of {info.TotalMemory} MB, " +
                            $"Storage: {info.UsedStorage} GB of {info.TotalStorage} GB, OS: {info.OSVersion}, " +
                            $"CPU: {info.CPUUsage}%, Model: {info.DeviceModel}\n";
                logEntry += "------------------------------------------------------\n";

                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging diagnostics: " + ex.Message);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            this.Close();
        }
    }
}
