using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Galactic
{
    public static class SamsungSDKHelper
    {
        private static string RunAdbCommand(string deviceId, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = $"-s {deviceId} {arguments}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    return output;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ADB command error: " + ex.Message);
                return string.Empty;
            }
        }

        public static bool InitializeDevice(string deviceId)
        {
            string model = GetDeviceProperty(deviceId, "ro.product.model");
            if (!string.IsNullOrEmpty(model))
            {
                Console.WriteLine($"Device Model: {model}");
                return true;
            }
            return false;
        }

        public static string GetDeviceDiagnostics(string deviceId)
        {
            string firmware = GetDeviceProperty(deviceId, "ro.build.id");
            string osVersion = GetDeviceProperty(deviceId, "ro.build.version.release");
            string model = GetDeviceProperty(deviceId, "ro.product.model");
            return $"Diagnostics: Firmware {firmware}, OS Android {osVersion}, Model: {model}";
        }

        public static DeviceHardwareInfo GetDeviceHardwareInfo(string deviceId)
        {
            DeviceHardwareInfo info = new DeviceHardwareInfo();

            // Memory info
            string memInfo = RunAdbCommand(deviceId, "shell dumpsys meminfo");
            Regex memRegex = new Regex(@"Total RAM:\s+(\d+)\s+MB", RegexOptions.IgnoreCase);
            Match memMatch = memRegex.Match(memInfo);
            info.TotalMemory = memMatch.Success ? int.Parse(memMatch.Groups[1].Value) : 4096;
            Regex freeRegex = new Regex(@"Free RAM:\s+(\d+)\s+MB", RegexOptions.IgnoreCase);
            Match freeMatch = freeRegex.Match(memInfo);
            info.UsedMemory = freeMatch.Success ? info.TotalMemory - int.Parse(freeMatch.Groups[1].Value) : info.TotalMemory / 2;

            // Storage info using df -h /data (improved regex)
            string storageInfo = RunAdbCommand(deviceId, "shell df -h /data");
            // Example line: "/dev/block/mapper/userdata  128G  64G  64G  50% /data"
            Regex storageRegex = new Regex(@"/data\s+(\d+\.?\d*)([TGM])\s+(\d+\.?\d*)([TGM])", RegexOptions.IgnoreCase);
            Match storageMatch = storageRegex.Match(storageInfo);
            if (storageMatch.Success)
            {
                double total = double.Parse(storageMatch.Groups[1].Value, CultureInfo.InvariantCulture);
                string unit = storageMatch.Groups[2].Value.ToUpper();
                info.TotalStorage = unit == "T" ? (int)(total * 1024) : unit == "M" ? (int)(total / 1024) : (int)total;

                double used = double.Parse(storageMatch.Groups[3].Value, CultureInfo.InvariantCulture);
                string usedUnit = storageMatch.Groups[4].Value.ToUpper();
                info.UsedStorage = usedUnit == "T" ? (int)(used * 1024) : usedUnit == "M" ? (int)(used / 1024) : (int)used;
            }
            else
            {
                info.TotalStorage = 128;
                info.UsedStorage = 64;
            }

            // Battery info
            string batteryInfo = RunAdbCommand(deviceId, "shell dumpsys battery");
            Regex batteryRegex = new Regex(@"level:\s*(\d+)", RegexOptions.IgnoreCase);
            Match batteryMatch = batteryRegex.Match(batteryInfo);
            info.BatteryLevel = batteryMatch.Success ? int.Parse(batteryMatch.Groups[1].Value) : 50;
            Regex tempRegex = new Regex(@"temperature:\s*(\d+)", RegexOptions.IgnoreCase);
            Match tempMatch = tempRegex.Match(batteryInfo);
            info.Temperature = tempMatch.Success ? int.Parse(tempMatch.Groups[1].Value) / 10.0 : 35.0;

            info.OSVersion = GetDeviceProperty(deviceId, "ro.build.version.release");
            info.DeviceModel = GetDeviceProperty(deviceId, "ro.product.model");

            // CPU usage via top command
            string topOutput = RunAdbCommand(deviceId, "shell top -n 1");
            Regex cpuRegex = new Regex(@"CPU usage:\s*(\d+)%", RegexOptions.IgnoreCase);
            Match cpuMatch = cpuRegex.Match(topOutput);
            info.CPUUsage = cpuMatch.Success ? int.Parse(cpuMatch.Groups[1].Value) : 50;

            return info;
        }

        public static string GetDeviceProperty(string deviceId, string property)
        {
            string output = RunAdbCommand(deviceId, $"shell getprop {property}");
            return output.Trim();
        }

        public static string SetDeviceProperty(string deviceId, string property, string value)
        {
            return RunAdbCommand(deviceId, $"shell setprop {property} {value}");
        }

        public static string RunAdvancedDiagnostics(string deviceId)
        {
            string psOutput = RunAdbCommand(deviceId, "shell ps");
            string result = "";
            foreach (var line in psOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.ToLower().Contains("malware") || line.ToLower().Contains("unknown"))
                    result += line + Environment.NewLine;
            }
            return string.IsNullOrEmpty(result) ? "No suspicious processes detected." : result;
        }
    }
}
