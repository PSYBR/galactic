using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Galactic
{
    public static class ADBManager
    {
        public static List<string> GetConnectedDevices()
        {
            var devices = new List<string>();

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "adb",
                    Arguments = "devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Parse the output.
                    var lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("List of devices"))
                            continue;
                        // Expect lines in the format: <device-id>    device
                        var parts = Regex.Split(line, @"\s+");
                        if (parts.Length >= 2 && parts[1].Trim() == "device")
                        {
                            devices.Add(parts[0].Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running adb: " + ex.Message);
            }

            return devices;
        }
    }
}
