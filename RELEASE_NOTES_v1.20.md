# MBBS Launcher v1.20 Release Notes

## Overview
Version 1.20 adds Ghost3 support for BBS door connectivity, along with UI improvements and updated visuals.

## 🎯 New Features

### Ghost3 Auto-Launch Support
- **Ghost3 Integration**: Automatically launch Ghost3 after The Major BBS starts
- **Configurable Delay**: Set custom delay (0-300 seconds, default 60) before Ghost3 launches
- **Visual Countdown**: Green-themed countdown timer with cancellation support
- **Flexible Path Configuration**: Customize Ghost3 installation path (default: C:\Ghost3\Ghost3.exe)
- **Independent Operation**: Ghost3 runs separately - no monitoring or management needed

### Updated Interface
- **New Background Image**: Refreshed launcher screen with updated text
- **Improved Help Dialog (F1)**:
  - Larger font (11pt) for better readability
  - No word wrapping - displays as formatted
  - Wider window (820px) to accommodate content
  - Resizable for user preference
- **Resizable Configuration Window (F12)**:
  - Now fully resizable for low-resolution displays
  - Minimum size: 650x600px
  - Works great on 1024x768 monitors
  - Maximize button enabled
  - Clean header with Save/Cancel buttons always visible

### UI/UX Improvements
- Consistent checkbox alignment throughout configuration
- Improved Ghost3 settings layout matching BBS auto-start style
- Better button positioning and visibility
- Professional header layout in configuration editor

## 📋 What's Ghost3?
Ghost3 is a program that allows The Major BBS to connect to old school BBS doors. This optional feature is perfect for sysops running classic door games alongside their BBS.

**How it works:**
1. Enable Ghost3 in Configuration (F12)
2. Set the path to Ghost3.exe
3. Configure launch delay (how long after BBS starts)
4. Launch your BBS (Option 5 - Go!)
5. Ghost3 automatically starts after the countdown

## 🔧 Configuration
Access all new settings via F12 Configuration Editor:

**Ghost3 Auto-Launch Settings:**
- ☑ Auto-launch Ghost3 after BBS starts (for BBS door support)
- Path: `C:\Ghost3\Ghost3.exe` (customizable with Browse button)
- Delay: 60 seconds (adjustable 0-300)

## 📸 Screenshots
See the updated launcher interface with new background and improved visuals in the screenshots folder.

## 🐛 Bug Fixes & Improvements
- Fixed configuration editor button visibility issues
- Improved form resizing behavior
- Better layout consistency across all configuration sections
- Enhanced help documentation with Ghost3 information

## 💾 Download
**File:** MBBS Launcher.exe
**Size:** 1.3 MB
**Requirements:** .NET 6.0 Desktop Runtime (Windows x86)

## 🔄 Upgrading from v1.10
Simply replace your existing `MBBS Launcher.exe` with the new version. Your configuration file (`MBBSLauncher.ini`) will be automatically updated with new Ghost3 settings on first run.

## 📖 Documentation
- Press **F1** in the launcher for updated help documentation
- Press **F12** to access the configuration editor
- See README.md for full installation and usage instructions

## 🙏 Credits
Created with Love ❤ by Mark Laudenbach in Iowa

## 📝 Notes
- Ghost3 is optional - the launcher works perfectly without it
- Ghost3 must be installed separately if you want to use this feature
- Administrator rights may be required depending on installation location (see README for details)

---

**Full Changelog:** See CHANGELOG.md for complete version history
