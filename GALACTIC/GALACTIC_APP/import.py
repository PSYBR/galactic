import zipfile

files = {
    "GalaxyTool/App.xaml": '''<Application x:Class="GalaxyTool.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="SplashScreen.xaml">
    <Application.Resources>
    </Application.Resources>
</Application>
''',

    "GalaxyTool/App.xaml.cs": '''using System.Windows;

namespace GalaxyTool
{
    public partial class App : Application
    {
    }
}
''',

    "GalaxyTool/SplashScreen.xaml": '''<Window x:Class="GalaxyTool.SplashScreen"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Galaxy Tool" Height="300" Width="500" WindowStartupLocation="CenterScreen" ResizeMode="NoResize">
    <Grid Background="White">
        <TextBlock x:Name="SplashText"
                   Text="No device found. Please reconnect your Galaxy S25 Ultra via USB and ensure Samsung USB drivers are installed."
                   TextWrapping="Wrap" FontSize="16" Foreground="DarkSlateGray"
                   HorizontalAlignment="Center" VerticalAlignment="Center" Margin="20"/>
    </Grid>
</Window>
''',

    "GalaxyTool/SplashScreen.xaml.cs": '''using System;
using System.Windows;
using System.Management; // Requires adding the System.Management NuGet package

namespace GalaxyTool
{
    public partial class SplashScreen : Window
    {
        private USBDeviceMonitor _usbMonitor;

        public SplashScreen()
        {
            InitializeComponent();
            Loaded += SplashScreen_Loaded;
        }

        private void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            // Start monitoring for USB device insertions.
            _usbMonitor = new USBDeviceMonitor();
            _usbMonitor.DeviceConnected += OnDeviceConnected;
            _usbMonitor.StartMonitoring();
        }

        private void OnDeviceConnected(object sender, string deviceInfo)
        {
            // Once a USB event is detected, use ADB to verify the device connection.
            Dispatcher.Invoke(() =>
            {
                var devices = ADBManager.GetConnectedDevices();
                if (devices.Count > 0)
                {
                    string deviceId = devices[0]; // Assume the first device is our target
                    // Initialize the Samsung SDK for this device.
                    bool sdkInitialized = SamsungSDKHelper.InitializeDevice(deviceId);
                    if (sdkInitialized)
                    {
                        // Open the main window and pass the device id.
                        MainWindow mainWindow = new MainWindow(deviceId);
                        mainWindow.Show();
                        _usbMonitor.StopMonitoring();
                        Close();
                    }
                    else
                    {
                        SplashText.Text = "Failed to initialize Samsung SDK. Please verify your installation.";
                    }
                }
                else
                {
                    SplashText.Text = "Device detected via USB, but ADB did not recognize it. Ensure ADB is installed and your device is set to developer mode.";
                }
            });
        }
    }
}
''',

    "GalaxyTool/MainWindow.xaml": '''<Window x:Class="GalaxyTool.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Galaxy Tool" Height="600" Width="800" WindowStartupLocation="CenterScreen">
    <Grid Margin="10">
        <StackPanel>
            <TextBlock Text="Device Connected:" FontSize="18" FontWeight="Bold" Margin="0,0,0,10"/>
            <TextBlock x:Name="DeviceInfoText" FontSize="14" Margin="0,0,0,10"/>
            <TextBlock x:Name="DiagnosticsText" FontSize="14" Margin="0,0,0,20"/>
            <!-- Buttons for further diagnostics and configuration -->
            <Button Content="Run Diagnostics" Width="150" Height="35" Margin="0,0,0,10"/>
            <Button Content="Configure Device Settings" Width="200" Height="35"/>
        </StackPanel>
    </Grid>
</Window>
''',

    "GalaxyTool/MainWindow.xaml.cs": '''using System.Windows;

namespace GalaxyTool
{
    public partial class MainWindow : Window
    {
        private string _deviceId;

        public MainWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;
            DeviceInfoText.Text = $"Connected Device: {deviceId}";
            // Retrieve diagnostic data using the Samsung SDK helper.
            string diagnostics = SamsungSDKHelper.GetDeviceDiagnostics(deviceId);
            DiagnosticsText.Text = diagnostics;
        }
    }
}
''',

    "GalaxyTool/USBDeviceMonitor.cs": '''using System;
using System.Management;

namespace GalaxyTool
{
    public class USBDeviceMonitor
    {
        private ManagementEventWatcher _watcher;

        // Event triggered when a USB device matching the criteria is connected.
        public event EventHandler<string> DeviceConnected;

        public void StartMonitoring()
        {
            try
            {
                // WMI query to detect insertion of USB devices.
                string query = "SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'";
                _watcher = new ManagementEventWatcher(query);
                _watcher.EventArrived += new EventArrivedEventHandler(DeviceInsertedEvent);
                _watcher.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting USB monitor: " + ex.Message);
            }
        }

        public void StopMonitoring()
        {
            if (_watcher != null)
            {
                _watcher.Stop();
                _watcher.Dispose();
            }
        }

        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            try
            {
                ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string deviceId = instance["DeviceID"]?.ToString() ?? "";
                string description = instance["Description"]?.ToString() ?? "";

                // Simple check for Samsung devices.
                if (deviceId.Contains("SAMSUNG", StringComparison.OrdinalIgnoreCase) ||
                    description.Contains("Samsung", StringComparison.OrdinalIgnoreCase))
                
