# MBBS Launcher v1.55 - Testing Guide

**Build Date:** February 18, 2026
**Version:** 1.55.0
**Status:** Released

---

## What's New in v1.55

### 1. Administrator Privileges (Completed)
- Application now requires administrator privileges on launch
- UAC prompt will appear when starting the launcher
- Fixes compatibility issues on Windows 11

### 2. App Manager (NEW - Beta Feature) ⭐
- **Real-time monitoring** of BBS and auto-launch programs
- **Countdown display** for pending auto-launches
- **Crash detection** with automatic alerts
- **Manual control** via right-click menus
- **Semi-transparent overlay** window (top-right by default)

---

## App Manager Features

### Display
- Shows BBS status (Running / Stopped / Crashed)
- Shows all auto-launch programs with status:
  - ✓ Running (green)
  - ⏱ Launch in X:XX (yellow countdown)
  - ⨯ Stopped (gray)
  - ⚠ CRASHED (red alert)

### Behavior
- **Auto-shows** when BBS launches (configurable)
- **Always-on-top** option (default: ON)
- **Auto-hide when complete** option (default: OFF)
- **Draggable** - position anywhere on screen
- **Remembers position** between sessions

### Right-Click Menu (on programs)
Right-click any auto-launch program for options:
- **Launch Now** - Start program immediately
- **Cancel Launch** - Cancel pending countdown
- **Stop Application** - Force-close running program

**Note:** BBS cannot be stopped via right-click (requires proper shutdown)

### Crash Detection
If the BBS crashes unexpectedly:
- App Manager shows "⚠ CRASHED" in red
- Main MBBS Launcher window automatically restores
- System tray notification alerts you

### Access App Manager
- **Automatically** - Opens when BBS starts (if configured)
- **Manually** - Right-click system tray icon → "App Manager (Beta)"

---

## Testing Instructions

### 1. Backup Your Current Launcher
```
1. Navigate to your current MBBS Launcher folder
2. Rename "MBBS Launcher.exe" to "MBBS Launcher.exe.backup"
3. Copy your MBBSLauncher.ini file as backup
```

### 2. Install Beta Build
```
1. Copy the new "MBBS Launcher.exe" from:
   C:\dev\MBBS Launcher\src\MBBSLauncher\bin\Release\net8.0-windows\win-x86\publish\

2. Place in your MBBS Launcher folder (where MBBSLauncher.ini is)

3. Right-click "MBBS Launcher.exe" → Properties
   - Verify Version: 1.55.0
```

### 3. First Launch
```
1. Double-click "MBBS Launcher.exe"

2. UAC Prompt:
   ✅ Click "Yes" to grant administrator privileges
   (This is required for v1.55)

3. Launcher should start normally with your existing config
```

### 4. Test Auto-Launch with App Manager

**Prerequisites:**
- Configure at least one auto-launch program (e.g., Ghost3)
- F12 → Auto-Launch Programs tab → Add programs

**Test Steps:**
1. Launch the BBS (Option 5)
2. **App Manager window should appear automatically** (top-right corner)
3. Observe:
   - BBS shows "✓ Running"
   - Auto-launch programs show countdown: "⏱ Launch: 0:25"
   - Countdowns update every second
4. **Wait for countdown to complete**
   - Programs launch automatically
   - Status changes to "✓ Running"
5. Close the App Manager window (X button)
6. **Reopen via system tray:**
   - Right-click tray icon → "App Manager (Beta)"

### 5. Test Right-Click Actions

**On a pending program (countdown active):**
1. Right-click on the program name
2. Select "Launch Now" - should skip countdown and launch immediately
3. OR select "Cancel Launch" - should stop countdown

**On a running program:**
1. Right-click on the program name
2. Select "Stop Application" - should force-close the program
3. Confirm the stop dialog

**On a stopped program:**
1. Right-click on the program name
2. Select "Launch Now" - should start the program

### 6. Test Crash Detection

**Simulate BBS Crash:**
1. Launch the BBS (Option 5)
2. App Manager appears showing BBS running
3. Manually force-close wgserver.exe:
   - Task Manager → Details → wgserver.exe → End Task
4. **Expected behavior:**
   - App Manager updates to show "⚠ CRASHED" in red
   - Main MBBS Launcher window automatically appears
   - System tray notification: "BBS Crashed"

### 7. Test Configurability

**App Manager Settings (in the window):**
- ☑ Always on top - Check/uncheck to test
- ☐ Auto-hide when complete - Enable to auto-hide after launches

**Configuration File (MBBSLauncher.ini):**
Check for new `[AppManager]` section:
```ini
[AppManager]
AutoShow=true          ; Show when BBS launches
AutoHide=false         ; Hide when launches complete
AlwaysOnTop=true       ; Window stays on top
LastX=1620            ; Window position X
LastY=20              ; Window position Y
```

**Change AutoShow to false:**
1. Edit MBBSLauncher.ini
2. Set `AutoShow=false` under `[AppManager]`
3. Restart launcher and launch BBS
4. **App Manager should NOT auto-show**
5. Open manually via tray icon

---

## Known Limitations (Beta)

1. **BBS stop button not available** - By design (requires proper shutdown)
2. **Crash detection is polling-based** - Checks every 2 seconds
3. **No restore from crash** - Only alerts, doesn't auto-restart (planned for later)
4. **Beta UI** - Visual design may change based on feedback

---

## What to Test and Report

### ✅ Test Checklist

- [ ] UAC prompt appears and launcher starts with admin privileges
- [ ] App Manager auto-shows when BBS launches
- [ ] Countdowns display correctly and update in real-time
- [ ] Programs launch when countdown reaches zero
- [ ] "Launch Now" right-click option works
- [ ] "Cancel Launch" right-click option works
- [ ] "Stop Application" right-click option works
- [ ] Crash detection works (BBS shows ⚠ CRASHED)
- [ ] Main window restores when BBS crashes
- [ ] "Always on top" checkbox works
- [ ] "Auto-hide when complete" checkbox works
- [ ] Window position is remembered between sessions
- [ ] Dragging window to new position works
- [ ] Cancel Auto-Launches button works
- [ ] Reopening via tray menu works
- [ ] AutoShow=false config setting works

### 🐛 Report Issues

Please report:
1. **What you were doing** (step-by-step)
2. **What you expected** to happen
3. **What actually happened**
4. **Screenshots** if applicable
5. **Error messages** from audit.log file

### 💡 Feedback

- How does the App Manager window look?
- Is the position (top-right) good, or would you prefer elsewhere?
- Is the window too large/small?
- Any features you'd like added?
- Any confusing behavior?

---

## Reverting to v1.5

If you encounter critical issues:
```
1. Download v1.5 from the GitHub releases page
2. Replace "MBBS Launcher.exe" with the v1.5 version
3. Your configuration will still work with v1.5
```

---

## Build Information

- **Executable Location:** `C:\dev\MBBS Launcher\src\MBBSLauncher\bin\Release\net8.0-windows\win-x86\publish\MBBS Launcher.exe`
- **File Size:** ~65 MB (self-contained, includes .NET 8.0 runtime)
- **Build Warnings:** 11 (all non-critical, mostly nullable reference type warnings)
- **Build Errors:** 0 ✅

### New Files Added:
- `Models/ManagedProgram.cs` - Program status tracking model
- `Forms/AppManagerForm.cs` - App Manager main code
- `Forms/AppManagerForm.Designer.cs` - App Manager UI design

### Modified Files:
- `Forms/MainForm.cs` - App Manager integration
- `ConfigManager.cs` - Added GetBool/GetInt helpers
- `MBBSLauncher.csproj` - Version bump to 1.55.0

---

## Support

If you encounter any issues during testing:
1. Check `audit.log` in the launcher folder for error details
2. Take screenshots of any unexpected behavior
3. Note the exact steps to reproduce the issue

---

**Happy Testing!** 🚀

This is beta software - expect rough edges and please provide feedback!
