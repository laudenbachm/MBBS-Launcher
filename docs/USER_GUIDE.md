# MBBS Launcher User Guide

Welcome to MBBS Launcher! This guide will help you get started and make the most of your launcher.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Using the Launcher](#using-the-launcher)
3. [Configuration](#configuration)
4. [Troubleshooting](#troubleshooting)
5. [Tips and Tricks](#tips-and-tricks)

## Getting Started

### First Launch

When you run MBBS Launcher for the first time:

1. The application will search for BBS installation folders:
   - `C:\BBSV10`
   - `C:\WGSERV`
   - Other common drives (D:\, E:\)

2. A configuration file `MBBSLauncher.ini` will be created in the same directory as the launcher

3. If your BBS is installed in a non-standard location, you'll need to configure paths manually (see Configuration section)

### System Requirements

- Windows 7 or later (or Windows Server 2012+)
- .NET 6.0 Desktop Runtime ([Download here](https://dotnet.microsoft.com/download/dotnet/6.0))
- ~5 MB disk space
- Screen resolution: 640x360 minimum (16:9 aspect ratio recommended)

## Using the Launcher

### Main Interface

The launcher features a retro DOS-style interface inspired by the classic Major BBS v6.25 launcher:

- **Blue background** with yellow "THE MAJOR BBS" title
- **9 numbered menu options** (1-9) displayed on screen
- **Version information** displayed at bottom
- **Instruction bar** showing "Type a number, or ←→ and hit Enter"

### Keyboard Controls

| Key | Action |
|-----|--------|
| **0** | Exit the launcher |
| **1-9** | Launch the corresponding menu program |
| **F12** | Open configuration editor |
| **ESC** | Exit the launcher (same as 0) |
| **Enter** | Launch selected option (future feature) |
| **←→** | Select option with arrows (future feature) |

### Launching Programs

To launch a program:

1. **Press the number key** (1-9) corresponding to the program you want to run
2. The launcher will:
   - Check if the program is already running
   - If running, ask if you want to bring it to foreground
   - If not running, launch the program and hide the launcher
3. When the launched program closes, the launcher automatically reappears

### Default Menu Options

Based on the classic Major BBS menu:

| Number | Menu Item | Default Purpose |
|--------|-----------|-----------------|
| **1** | Hardware Setup | Configure hardware settings |
| **2** | Design Menu Tree | Edit BBS menu structure |
| **3** | Security & Accounting | User security and billing |
| **4** | Configuration Options | System configuration |
| **5** | **Go!** | Start the BBS (wgsappgo.exe) |
| **6** | Edit Text Blocks | Edit system text/messages |
| **7** | Basic Utilities | Essential BBS utilities |
| **8** | Add-on Utilities | Third-party add-ons |
| **9** | Reports | Generate system reports |
| **0** | Exit | Close the launcher |

## Configuration

### Opening the Configuration Editor

Press **F12** at any time to open the configuration editor.

### BBS Installation Paths

The top section of the config editor lets you set:

- **BBSV10 Path**: Location of Major BBS Version 10 installation (e.g., `C:\BBSV10`)
- **WGSERV Path**: Location of Worldgroup installation (e.g., `C:\WGSERV`)

Click **Browse** to select folders.

### Configuring Menu Programs

For each menu option (1-9), you can configure:

1. **Name**: The display name for the menu option
   - This appears in the configuration editor
   - Future versions may show it on the main screen

2. **Program**: Full path to the executable
   - Click **Browse** to select an .exe file
   - Leave blank if the option is not used

### Example Configuration

```
Option 5:
- Name: Go!
- Program: C:\BBSV10\wgsappgo.exe

Option 7:
- Name: Basic Utilities
- Program: C:\BBSV10\mbbsutil.exe

Option 0:
- Name: Exit
- Program: (exits launcher)
```

### Saving Configuration

1. Make your changes in the configuration editor
2. Click **Save** button
3. Configuration is written to `MBBSLauncher.ini`
4. Changes take effect immediately

### Manual Configuration Editing

You can also edit `MBBSLauncher.ini` directly with a text editor:

```ini
[Paths]
BBSV10=C:\BBSV10
WGSERV=C:\WGSERV

[Programs]
Option1=
Option1Name=Hardware Setup
Option5=C:\BBSV10\wgsappgo.exe
Option5Name=Go!
...
```

**Restart the launcher** after manual editing to load changes.

## Troubleshooting

### Program Won't Launch

**Problem**: Clicking a number does nothing or shows an error.

**Solutions**:
1. Press F12 to check configuration
2. Verify the program path is correct
3. Ensure the .exe file exists at that location
4. Check that you have permission to run the program
5. Try running the .exe directly to see if there are other issues

### "Program Not Found" Error

**Problem**: Error message says the program file doesn't exist.

**Solutions**:
1. Press F12 to open configuration editor
2. Browse to the correct location of the program
3. Verify the installation path is correct
4. Re-install the BBS software if files are missing

### Launcher Doesn't Reappear

**Problem**: After launching a program, the launcher stays hidden.

**Solutions**:
1. Check if the launched program is still running (Task Manager)
2. Close the launched program completely
3. Look for the launcher in the taskbar - it may be minimized
4. If stuck, restart the launcher from the Start menu or desktop

### WGServer Already Running

**Problem**: Message says "WGServer is already running"

**What it means**: The BBS server is already active, which is normal if your BBS is running.

**Options**:
- Click **Yes** to bring WGServer to the foreground
- Click **No** to leave it running in background
- You can still launch other utilities from the launcher

### Configuration Changes Not Saving

**Problem**: Changes in F12 editor don't stick.

**Solutions**:
1. Make sure you clicked **Save** button (not just closed the window)
2. Check that `MBBSLauncher.ini` isn't marked read-only
3. Verify you have write permissions in the launcher directory
4. Run launcher as administrator if needed

### Background Image Not Showing

**Problem**: Launcher shows plain blue background instead of the retro image.

**Solutions**:
1. Verify `Resources/background.png` exists in the launcher directory
2. Check file permissions (file should be readable)
3. Re-extract the launcher from the original ZIP file
4. Ensure the Resources folder is in the same directory as MBBSLauncher.exe

### .NET Runtime Error

**Problem**: Error about .NET framework or runtime not found.

**Solution**: Install .NET 6.0 Desktop Runtime:
1. Visit: https://dotnet.microsoft.com/download/dotnet/6.0
2. Download "Desktop Runtime" (not SDK)
3. Install and restart your computer
4. Launch MBBS Launcher again

## Tips and Tricks

### Quick Start Your BBS

1. Configure Option 5 to point to `wgsappgo.exe`
2. Press **5** to instantly start your BBS
3. The launcher hides automatically while BBS runs
4. When you shut down the BBS, launcher reappears

### Create Custom Menu Layouts

The 9 options are fully customizable:
- Point them to any .exe files you use frequently
- Use descriptive names in the config editor
- Organize by task type or frequency of use

### Auto-Launch on Startup

To have MBBS Launcher start automatically:
1. Press `Win+R`
2. Type: `shell:startup`
3. Create a shortcut to `MBBSLauncher.exe` in that folder

### Multiple BBS Installations

If you manage multiple BBS installations:
1. Create separate launcher folders for each
2. Configure each with different paths
3. Use different shortcuts to launch each one

### Keyboard-Only Operation

The launcher is designed for fast keyboard use:
- No mouse required
- Just press a number and Enter
- Perfect for remote desktop sessions

### Backup Your Configuration

Your settings are in `MBBSLauncher.ini`:
1. Make a backup copy periodically
2. Store with your BBS backups
3. Restore easily if needed

## Frequently Asked Questions

### Can I change the background image?

Currently, the background image is `Resources/background.png`. You could replace it with your own PNG file of the same dimensions, though it's designed to match the classic BBS look.

### Will future versions support mouse selection?

Yes! Mouse/arrow key selection is planned for future releases, allowing you to highlight and click menu options.

### Can I add more than 9 programs?

Currently, the launcher supports 9 numbered options (1-9) plus Exit (0), matching the classic design. Future versions may expand this.

### Does it work with BBS versions other than v10?

The launcher works with any program you configure, including older BBS versions, Worldgroup, or even non-BBS utilities.

### How do I update to a new version?

1. Download the new version
2. Copy your existing `MBBSLauncher.ini` to backup
3. Replace the old .exe with the new one
4. Your configuration will be preserved

## Support

If you need help:

- Check this user guide
- Review the [Troubleshooting](#troubleshooting) section
- Open an issue on GitHub: https://github.com/laudenbachm/MBBS-Launcher/issues
- Include your Windows version and any error messages

---

**MBBS Launcher User Guide** | Version 1.00 | Created by Mark Laudenbach with Love in Iowa
