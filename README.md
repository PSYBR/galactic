# Galactic

Galactic is a comprehensive Windows desktop diagnostic and troubleshooting tool designed for Samsung Galaxy devices. It leverages ADB (and optionally Samsung’s official SDK) to collect real-time hardware and software data directly from a connected device. Built using WPF and modern .NET technologies, Galactic provides a premium dark-themed interface with features such as live diagnostics, interactive remote viewing, settings synchronization, and specialized options like S-Pen calibration for Ultra models.

## Features

- **Device Connection & Initialization:**  
  - Dynamic splash screen with an official logo, animated spinner, and chime notification.
  - Displays a welcome message with version information and prompts the user to re-plug the device for synchronization.

- **Real-Time Diagnostics:**  
  - Displays device model (e.g., "SAMSUNG Galaxy S25 (128GB)") and serial number.
  - Shows detailed hardware data: memory usage, storage usage, battery level, temperature, CPU usage, and more.
  - Performs advanced diagnostics by scanning running background processes for suspicious or unknown activities.
  - Logs every diagnostic scan to a local exportable text file for troubleshooting.

- **Interactive Remote View (G-Vision):**  
  - Provides an interactive emulated view of the device’s screen.
  - Users can simulate touch events via mouse input for remote control.

- **Settings Synchronization & Developer Options:**  
  - Synchronizes essential Android settings such as USB debugging and "Show Taps."
  - Includes a dedicated S-Pen calibration window for Ultra models.
  - Offers a quick-access button to open the folder containing detailed scan logs.

## Requirements

- **Operating System:** Windows  
- **.NET SDK:** .NET 6.0 or later (Download from [Microsoft .NET](https://dotnet.microsoft.com/download))  
- **ADB:** Android Debug Bridge (ADB) must be installed and added to your system’s PATH. You can download the Android SDK Platform Tools from [here](https://developer.android.com/studio/releases/platform-tools).  
- **Samsung Device:** A Samsung Galaxy device with USB debugging enabled.  
- **Visual Studio Code:** Recommended for development, debugging, and editing the code.

## Getting Started with VS Code

### 1. Clone the Repository

bash---

git clone <repository-url>
cd Galactic

---
1. Open the Project in VS Code
   
2. Launch Visual Studio Code.
   
3. Go to File > Open Folder… and select the Galactic project folder.

4. Restore Dependencies
Open the integrated terminal (View > Terminal) and run:

bash
Copy
dotnet restore

5. Build the Project
Build the project using:

bash
Copy
dotnet build

6. Running and Debugging the Application

+ You can run the application directly from the terminal:

bash
Copy
dotnet run

Alternatively, set up a launch configuration in VS Code:

Create a .vscode folder in your project root.

Add a launch.json file with the following content:

json
Copy
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": ".NET Core Launch (console)",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/bin/Debug/net6.0/Galactic.exe",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "console": "integratedTerminal"
    }
  ]
}
Create a tasks.json file in the same folder:

json
Copy
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/Galactic.csproj"
      ],
      "problemMatcher": "$msCompile"
    }
  ]
}

Press F5 to start debugging.

Assets
Make sure the following assets are included in the project and set to “Content” with “Copy if newer”:

official_logo.png
remote_view_icon.png
spen_calibrate_icon.png
device_screen.png
chime.wav

Logging:
All diagnostic scans are logged in a local Logs folder. The Settings window includes a button to open this folder for easy access to your scan logs.

Troubleshooting +++

ADB Issues: Run adb devices in the terminal to ensure your device is recognized.

Device Connection: Verify that USB debugging is enabled on your Samsung device.

Build Errors: Ensure all NuGet packages are restored using dotnet restore.

----
Future Enhancements +++

Integration with Samsung’s official SDK for even more detailed diagnostics.

Live remote view streaming and interactive control.

Enhanced process scanning for malicious or unusual activity.

Additional device configuration options and UI improvements.

License MIT
© PSYBR Media Technologies. All rights reserved.
--------------------------
NEXT UPDATEE++++++++++++++++++++

## Additional Features for v.1.0.1 (Upcoming)

Galactic is expanding its functionality with the following upcoming features:

- **Media Import/Export & Gallery Explorer:**  
  Easily import and export media files, and explore device photos and videos in an interactive gallery.

- **Local Device Backup & App Data Backup:**  
  Create local backups of device data (media, contacts, app data) and restore them when needed.

- **Music Management & Folder Sync:**  
  Manage music files on your device and synchronize your music folders with your desktop.

- **Calendar Sync:**  
  Synchronize your device calendar with a local calendar file for easy management.

- **Wireless ADB Options:**  
  Enable and disable wireless ADB connections for greater flexibility.

- **Contacts Manager & Backup:**  
  Backup and manage your device contacts, with options to export as CSV or vCard.

- **Galaxy App Store Integration:**  
  Browse and install apps directly from an integrated Galaxy App Store interface.

*Note: Many of these features are currently in the planning or development phase and will be available in upcoming releases.*


Visit us

https://Psybr.xyz





