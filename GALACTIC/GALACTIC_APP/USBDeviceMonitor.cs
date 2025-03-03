using System;
using System.Management;

namespace Galactic
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

                // Check for Samsung devices.
                if (deviceId.Contains("SAMSUNG", StringComparison.OrdinalIgnoreCase) ||
                    description.Contains("Samsung", StringComparison.OrdinalIgnoreCase))
                {
                    string info = $"Detected Device: {description} (ID: {deviceId})";
                    DeviceConnected?.Invoke(this, info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing USB event: " + ex.Message);
            }
        }
    }
}
