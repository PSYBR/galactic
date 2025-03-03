namespace Galactic
{
    public class DeviceHardwareInfo
    {
        public int TotalMemory { get; set; }      // in MB
        public int UsedMemory { get; set; }       // in MB
        public int TotalStorage { get; set; }     // in GB
        public int UsedStorage { get; set; }      // in GB
        public int BatteryLevel { get; set; }     // percentage
        public double Temperature { get; set; }   // Celsius

        // Additional hardware/software info
        public string OSVersion { get; set; }
        public int CPUUsage { get; set; }         // percentage
        public string DeviceModel { get; set; }
    }
}
