using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;

namespace Galactic
{
    public partial class RemoteViewWindow : Window
    {
        private string _deviceId;
        private DispatcherTimer _remoteTimer;

        public RemoteViewWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;

            // Start a timer to update the remote view periodically.
            _remoteTimer = new DispatcherTimer();
            _remoteTimer.Interval = TimeSpan.FromSeconds(2);
            _remoteTimer.Tick += RemoteTimer_Tick;
            _remoteTimer.Start();
        }

        private void RemoteTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // For demonstration, load a static image.
                // In a real implementation, capture a live screenshot via ADB or Samsung SDK.
                string imagePath = "device_screen.png";
                if (File.Exists(imagePath))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(Path.GetFullPath(imagePath));
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    RemoteImage.Source = bitmap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating remote view: " + ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _remoteTimer.Stop();
            base.OnClosed(e);
        }
    }
}
