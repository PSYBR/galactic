using System;
using System.Windows;
using System.Management;
using System.Media; // For playing sound

namespace Galactic
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
            // Start monitoring USB device insertions.
            _usbMonitor = new USBDeviceMonitor();
            _usbMonitor.DeviceConnected += OnDeviceConnected;
            _usbMonitor.StartMonitoring();
        }

        private void OnDeviceConnected(object sender, string deviceInfo)
        {
            Dispatcher.Invoke(() =>
            {
                var devices = ADBManager.GetConnectedDevices();
                if (devices.Count > 0)
                {
                    string deviceId = devices[0];
                    bool sdkInitialized = SamsungSDKHelper.InitializeDevice(deviceId);
                    if (sdkInitialized)
                    {
                        // Play a chime sound to confirm successful connection.
                        try
                        {
                            SoundPlayer player = new SoundPlayer("chime.wav");
                            player.Load();
                            player.Play();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error playing chime sound: " + ex.Message);
                        }

                        // Open the Main Window.
                        MainWindow mainWindow = new MainWindow(deviceId);
                        mainWindow.Show();
                        _usbMonitor.StopMonitoring();
                        Close();
                    }
                    else
                    {
                        SplashText.Text = "SDK initialization failed.";
                    }
                }
                else
                {
                    SplashText.Text = "Device detected but not recognized by ADB.";
                }
            });
        }
    }
}
