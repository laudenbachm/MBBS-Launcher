# MBBS Launcher

**Version:** v1.00

## Screenshot

![MBBS Launcher Screenshot](images/MBBS%20Launcher%20Screenshot.png)
**Created by:** Mark Laudenbach
**Created with Love in:** Iowa
**License:** MIT

## About

MBBS Launcher is a Windows application that provides easy access to tools and utilities for The Major BBS Version 10 sysops. Inspired by the classic DOS-era Major BBS launcher interface, this modern version brings the nostalgic feel of the original while adding contemporary features and usability.

**📺 Watch the Demo:** [MBBS Launcher Demo on YouTube](https://youtu.be/izJImFGJ2NA)

## 🐛 Bug Disclosure

This was coded by a guy who Googles "how to exit vim" every single time. There WILL be bugs. There WILL be spelling mistakes. There WILL be profanity shouted at my monitor. You've been warned!

## Features

- **Retro DOS-Style Interface** - Classic blue screen design reminiscent of the original Major BBS v6.25 launcher
- **Easy Program Access** - Launch BBS utilities and tools with keyboard (0-9, 99) or mouse clicks
- **Clickable Menu Options** - Click any menu button with your mouse or use traditional keyboard input
- **Configurable Menu** - Customize program paths and menu options via F12 configuration editor
- **Auto-Launch at Startup** - Optional Windows startup integration (disabled by default)
- **Smart Process Management** - Automatically detects if programs are already running and brings them to foreground
- **WGServer Protection** - Prevents conflicts by detecting if the BBS server is already running
- **Auto-Hide/Show** - Launcher hides when programs run and reappears when they close
- **16:9 Aspect Ratio** - Modern scalable window design while maintaining the classic look
- **Single-File Deployment** - All resources embedded in executable, no external files needed
- **INI Configuration** - Simple text-based configuration file for easy editing
- **Custom Icon Support** - Professional Windows application icon in taskbar and title bar

## System Requirements

- **Operating System:** Windows 7 or later, Windows Server 2012 or later
- **Architecture:** 32-bit (x86)
- **.NET Runtime:** .NET 6.0 Runtime (Windows Desktop)
- **Disk Space:** ~5 MB

## Installation

1. Download the latest release from the [Releases](https://github.com/laudenbachm/MBBS-Launcher/releases) page
2. Extract the ZIP file to a folder of your choice
3. Install .NET 6.0 Desktop Runtime if not already installed: [Download .NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
4. Run `MBBS Launcher.exe`

**Note:** All images and resources are embedded in the executable - no external files required!

## Antivirus False Positives

Some antivirus software may flag MBBS Launcher as suspicious due to legitimate behaviors that are common in system utilities:

- **Process enumeration** - Checking if BBS programs are already running
- **Window manipulation** - Bringing running programs to foreground
- **Launching executables** - Starting BBS utilities on your behalf
- **Startup integration** - Optional auto-launch at Windows startup

**This is a FALSE POSITIVE.** The application is completely safe and open source.

### What We're Doing About It

- The executable has been submitted to Microsoft Defender and other major AV vendors for whitelisting
- All source code is publicly available in this repository for review
- We are working on obtaining a code signing certificate to eliminate these warnings

### If Your Antivirus Flags It

1. **Review the source code** - All code is available in this repository
2. **Scan on VirusTotal** - Check the analysis at https://www.virustotal.com
3. **Add an exception** - Add `MBBS Launcher.exe` to your antivirus exclusions
4. **Report as false positive** - Help us by reporting it to your AV vendor

For more information, see our [Security Policy](https://github.com/laudenbachm/MBBS-Launcher/security/policy).

## Usage

### Launching Programs

- **Number Keys (0-9, 99):** Press any number key to launch the corresponding program
  - Option 0: Exit the launcher
  - Options 1-8: Launch configured programs
  - Option 99: Special configuration option (CNF 99)
- **Mouse Click:** Click any menu button to launch that option
- **ESC Key:** Exit the launcher
- **F12 Key:** Open configuration editor

### First Time Setup

On first launch, the application will:
1. Search for BBSV10 and WGSERV folders on your system
2. Create a default configuration file `MBBSLauncher.ini`
3. Prompt you to configure program paths if not found automatically

### Configuration

Press **F12** to open the configuration editor where you can:
- Enable/disable auto-launch at Windows startup
- Set BBS installation path
- Configure launcher options (1-8, 99) with custom program names and paths
- Browse for executable files

Configuration is saved to `MBBSLauncher.ini` in the same directory as the launcher.

### Default Menu Layout

Based on the classic Major BBS launcher:

1. Hardware Setup
2. Design Menu Tree
3. Security & Accounting
4. General Configuration
5. **Go!** - Launch the BBS (wgsappgo.exe)
6. Edit Text Blocks
7. Offline Utilities
8. Reports
99. CNF 99
0. Exit MBBS Launcher

## Building from Source

### Prerequisites

- .NET 6.0 SDK or later
- Windows 10/11 or Windows Server 2016+ (for building)
- Visual Studio 2022 (optional, recommended)

### Build Instructions

#### Using Command Line

```bash
# Clone the repository
git clone https://github.com/laudenbachm/MBBS-Launcher.git
cd MBBS-Launcher

# Build the project
cd src/MBBSLauncher
dotnet restore
dotnet build -c Release

# Publish as single-file executable
dotnet publish -c Release -r win-x86 --self-contained false /p:PublishSingleFile=true
```

#### Using Visual Studio

1. Open `src/MBBSLauncher/MBBSLauncher.csproj` in Visual Studio 2022
2. Select **Release** configuration
3. Build > Publish
4. Choose target: win-x86
5. Click Publish

The compiled executable will be in `bin/Release/net6.0-windows/win-x86/publish/`

## Configuration File Format

The `MBBSLauncher.ini` file uses standard INI format:

```ini
[Paths]
BBSPath=C:\BBSV10

[Window]
X=100
Y=100
Width=960
Height=540

[Settings]
AutoLaunchAtStartup=false

[Programs]
Option1=C:\BBSV10\WGSCNF.exe -L1
Option1Name=Hardware Setup
Option2=C:\BBSV10\wgsrunmt.exe
Option2Name=Design Menu Tree
...
Option5=C:\BBSV10\wgsappgo.exe
Option5Name=Go!
...
Option8=C:\BBSV10\WGSRPT.exe
Option8Name=Reports
Option99=C:\BBSV10\WGSCNF.exe -L99
Option99Name=CNF 99
```

You can edit this file manually or use the built-in configuration editor (F12).

## Version History

See [CHANGELOG.md](CHANGELOG.md) for detailed version history.

### Current Version: v1.00
- Initial release
- Classic retro DOS-style interface with authentic MBBS look
- Configurable program launcher (Options 1-8, 99, and 0 for exit)
- Clickable menu buttons plus traditional keyboard input
- Auto-launch at Windows startup (optional)
- Smart process detection and management
- Auto-hide/show functionality when launching programs
- Single-file executable with embedded resources
- INI-based configuration with automatic BBS folder detection
- Built-in configuration editor (F12)
- Custom Windows application icon
- Window position and size memory
- 16:9 aspect ratio with scalable interface

## Credits

- **Created by:** Mark Laudenbach
- **Inspired by:** The Major BBS v6.25 DOS Launcher by Galacticomm, Inc.
- **For:** The Major BBS and Worldgroup sysop community

## Support

For issues, questions, or suggestions:
- Open an issue on [GitHub Issues](https://github.com/laudenbachm/MBBS-Launcher/issues)
- Check the [Documentation](docs/)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Dedication

This application was created with love for The Major BBS community. The Major BBS holds a special place in the history of online communication, and this launcher is a tribute to those great days while bringing convenience to modern sysops.

---

**MBBS Launcher v1.00** | Created with Love in Iowa | © 2026 Mark Laudenbach
