using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;

namespace Galactic
{
    public partial class GVisionWindow : Window
    {
        private string _deviceId;
        public GVisionWindow(string deviceId)
        {
            InitializeComponent();
            _deviceId = deviceId;
            UpdateGVisionImage();
        }
        private void UpdateGVisionImage()
        {
            // For demonstration, load a static image representing the device screen.
            // Replace with live capture (e.g., via "adb exec-out screencap -p") for real-time view.
            string imagePath = "device_screen.png";
            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(Path.GetFullPath(imagePath));
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                GVisionImage.Source = bitmap;
            }
        }
        private void InteractionCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(InteractionCanvas);
            MessageBox.Show($"Touched at: {pos.X}, {pos.Y}", "G-Vision Interaction", MessageBoxButton.OK, MessageBoxImage.Information);
            // In a real implementation, send the touch coordinates via adb (e.g., "adb shell input tap X Y").
        }
    }
}
