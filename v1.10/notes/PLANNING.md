# MBBS Launcher v1.10 - Development Plan

**Planning Date:** January 12, 2026
**Current Version:** v1.00
**Target Version:** v1.10
**Last Updated:** January 12, 2026

---

## Overview

Version 1.10 brings user experience improvements focused on accessibility (mouse/keyboard navigation), convenience (auto-start, system tray), and expanded utility options. These features maintain the nostalgic DOS-era feel while adding modern conveniences for sysops running The Major BBS v10.

**Scope for v1.10:**
- Mouse Navigation with visual feedback
- Auto-Load BBS on Startup (optional, OFF by default)
- System Tray Icon with status indication
- Additional Program Options (utilities at bottom of launcher)

**Deferred to Post-v1.10:**
- Installer (Inno Setup) - Will be created after v1.10 features are complete, reusable for future versions

---

## Feature 1: Mouse Navigation with Visual Feedback

### Description
Add visual selection indicator and hover effects to make the launcher more discoverable and accessible to users who prefer mouse navigation.

### Current State
- Mouse click detection already works (MainForm.cs:297-349)
- Uses normalized coordinate system (0.0-1.0) mapped to 1440x810 reference resolution
- Click regions defined for all options (1-9, 0, 99) and URL links
- No visual feedback on hover or selection

### Implementation Plan

**Step 1: Add Mouse Tracking State**
- Add `_hoveredOption` field to MainForm (similar to existing `_selectedOption`)
- Track which button the mouse is currently over

**Step 2: Implement MouseMove Handler**
- Add `MainForm_MouseMove` event handler
- Use existing normalized coordinate system to detect which option is under cursor
- Update `_hoveredOption` when cursor moves between regions
- Call `Invalidate()` to trigger repaint when hover state changes

**Step 3: Visual Hover Effect**
- Modify `MainForm_Paint` to draw highlight when `_hoveredOption` is set
- Options for visual effect:
  - Semi-transparent highlight overlay (recommended - keeps retro feel)
  - Border/outline around hovered button
  - Slight brightness increase on hovered region
- Use same coordinate mapping as click detection

**Step 4: Cursor Changes**
- Change cursor to `Hand` when over clickable regions
- Change back to `Default` when over non-clickable areas

**Step 5: Selection Indicator for Arrow Keys**
- Draw distinct selection indicator for keyboard-selected option
- Different visual from hover (e.g., solid border vs. glow)

### Files to Modify
- `src/MBBSLauncher/Forms/MainForm.cs` - Add MouseMove handler, update Paint event
- `src/MBBSLauncher/Forms/MainForm.Designer.cs` - Wire up MouseMove event

### Technical Notes
- Double-buffering already enabled (no flicker concerns)
- Normalized coordinates already calculated in MouseClick handler - can extract to shared method
- Consider debouncing Invalidate() calls if performance is an issue (unlikely)

### Estimated Complexity: Low-Medium

---

## Feature 2: Auto-Load BBS on Startup

### Description
Option to automatically launch the BBS (wgserver via Option 5 "Go!") when the launcher starts, with configurable delay.

### Requirements
- **OFF by default** - This is an opt-in feature only
- User-configurable delay (default: 5 seconds)
- Visual countdown with cancel option
- Optional quiet mode (minimize to tray after launch)

### Current State
- Windows startup registration exists (ConfigEditorForm.cs:473-520)
- Uses Registry key: `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run`
- `AutoLaunchAtStartup` setting exists but only controls Windows startup, not BBS auto-start
- BBS detection on startup exists (checks if wgserver already running)

### Implementation Plan

**Step 1: Add Configuration Options**
Add to `[Settings]` section in INI:
```ini
[Settings]
AutoLaunchAtStartup=false      ; Existing - launch LAUNCHER at Windows startup
AutoStartBBS=false             ; NEW - OFF BY DEFAULT - auto-launch BBS when launcher starts
AutoStartDelay=5               ; NEW - user configurable delay in seconds (default 5)
QuietMode=false                ; NEW - minimize to tray after auto-launch
```

**Step 2: Update ConfigManager**
- Add default values for new settings in `CreateDefaultConfig()`
- `AutoStartBBS` MUST default to `false`
- `AutoStartDelay` defaults to `5`
- No structural changes needed (existing GetValue/SetValue work fine)

**Step 3: Update ConfigEditorForm**
- Add checkbox: "Auto-start BBS when launcher opens" (unchecked by default)
- Add numeric input: "Delay before auto-start (seconds)" (default 5, range 0-60)
- Add checkbox: "Minimize to tray after auto-start" (depends on Feature 4)
- Group these in a "BBS Auto-Start" section

**Step 4: Implement Auto-Start Logic in MainForm**
- In `MainForm_Shown` event (after form is visible):
  1. Check if `AutoStartBBS=true`
  2. Check if wgserver is NOT already running
  3. If both true, start countdown timer
  4. Display countdown on screen
  5. Allow cancellation via any keypress or mouse click
  6. When timer expires, trigger Option 5 launch
  7. If `QuietMode=true`, minimize to tray (requires Feature 4)

**Step 5: Visual Countdown**
- Show "Auto-starting BBS in X seconds... Press any key to cancel"
- Overlay text on main form (bottom area or center)
- Countdown updates every second
- Clear message when cancelled or when launch occurs

### Files to Modify
- `src/MBBSLauncher/ConfigManager.cs` - Add default settings
- `src/MBBSLauncher/Forms/ConfigEditorForm.cs` - Add UI controls
- `src/MBBSLauncher/Forms/MainForm.cs` - Add auto-start logic and countdown display

### Dependencies
- Feature 4 (System Tray) for QuietMode functionality
- Recommend implementing Feature 4 first or in parallel

### Estimated Complexity: Low-Medium

---

## Feature 3: System Tray Icon

### Description
Add system tray icon allowing the launcher to minimize to tray, with right-click context menu for quick actions and status indication.

### Requirements
- Tray icon that shows launcher is running
- **Icon changes when a program is running** (visual status)
- **Hover tooltip shows which program is running**
- **Option to bring running program to foreground**
- Right-click context menu for quick actions
- Minimize to tray option

### Current State
- No system tray functionality
- Launcher hides when programs run, shows when they exit
- No minimize-to-tray option

### Implementation Plan

**Step 1: Add Configuration Options**
```ini
[Settings]
MinimizeToTray=true            ; NEW - minimize to tray instead of taskbar
ShowTrayIcon=true              ; NEW - always show tray icon when launcher running
```

**Step 2: Add NotifyIcon Component**
- Add `NotifyIcon` to MainForm
- Set default icon from embedded resource (existing icon.ico)
- Set default tooltip text: "MBBS Launcher - Idle"

**Step 3: Create Status Icons**
- Need two icon states:
  - **Idle icon:** Normal launcher icon (existing)
  - **Running icon:** Modified icon indicating program is running (could be color overlay, badge, or alternate icon)
- Consider creating icon variants or using icon overlay

**Step 4: Dynamic Tooltip and Icon**
- When program launches:
  - Change icon to "running" state
  - Update tooltip: "MBBS Launcher - Running: [Program Name]"
  - Example: "MBBS Launcher - Running: Hardware Setup"
- When program exits:
  - Change icon back to "idle" state
  - Update tooltip: "MBBS Launcher - Idle"

**Step 5: Implement Tray Icon Context Menu**
Right-click menu items:
```
Show Launcher
---
Start BBS (Go!)
Bring [Running Program] to Front    <- Only visible when program running
---
Configuration (F12)
---
Exit
```

**Step 6: "Bring to Front" Functionality**
- Menu item only visible when a launched program is running
- Shows actual program name: "Bring Hardware Setup to Front"
- Uses existing `ProcessHelper.BringToForeground()` logic
- If multiple programs could run (future), show submenu

**Step 7: Minimize to Tray Behavior**
- Override `OnResize` or handle `Resize` event
- When minimized AND `MinimizeToTray=true`:
  - Set `ShowInTaskbar = false`
  - Keep `NotifyIcon.Visible = true`
  - Show balloon tip first time: "MBBS Launcher minimized to tray"

**Step 8: Restore from Tray**
- Double-click tray icon: Restore window
- "Show Launcher" menu item: Restore window
- Restore logic:
  - Set `ShowInTaskbar = true`
  - Set `WindowState = FormWindowState.Normal`
  - Activate and bring to front

**Step 9: Integration with Program Launch**
- When launching a program:
  - Instead of hiding form completely, minimize to tray
  - Update tray icon to "running" state
  - Update tooltip with program name
- When program exits:
  - Update tray icon to "idle" state
  - Restore launcher from tray (current behavior) OR show notification

**Step 10: Update ConfigEditorForm**
- Add checkbox: "Minimize to system tray"
- Add checkbox: "Always show tray icon"

### Files to Modify
- `src/MBBSLauncher/Forms/MainForm.cs` - Add NotifyIcon, tray menu, status tracking, minimize logic
- `src/MBBSLauncher/Forms/MainForm.Designer.cs` - Wire up components
- `src/MBBSLauncher/Forms/ConfigEditorForm.cs` - Add UI controls
- `src/MBBSLauncher/ConfigManager.cs` - Add default settings

### New Resources Needed
- Running state icon (or icon overlay logic)

### Technical Notes
- NotifyIcon requires proper disposal in `Dispose()` to avoid orphaned tray icons
- Track currently running program name for tooltip display
- Context menu items need dynamic show/hide based on state
- Balloon tips may be blocked by Windows Focus Assist

### Estimated Complexity: Medium

---

## Feature 4: Additional Program Options

### Description
Expand the launcher to support additional configurable programs (offline utilities) displayed at the bottom of the launcher.

### Requirements
- User will provide updated background image with space at bottom for additional options
- User will provide list of programs and commands
- Must be included in v1.10 before release

### Current State
- Options 1-9 configurable
- Option 0 = Exit
- Option 99 = Special CNF option
- All options defined in INI [Programs] section
- Background image has fixed layout (1440x810)

### Implementation Plan

**Step 1: Await User Input**
- [ ] User to provide new background image with space for additional options
- [ ] User to provide list of programs with:
  - Program name (display label)
  - Executable path
  - Command line arguments (if any)
  - Suggested keyboard shortcut (if any)

**Step 2: Determine Input Method**
Options for triggering additional programs:
- Two-digit numbers (10-19, 20-29, etc.) - existing digit buffer supports this
- Function keys (F1-F11, excluding F12 for config)
- Letter keys (A-Z)
- Will decide based on number of programs and user preference

**Step 3: Update Click Regions**
- Add new normalized coordinate regions for bottom area buttons
- Map to new background image layout

**Step 4: Extend Configuration**
```ini
[Programs]
; Existing options 1-9, 99...
Option10=C:\path\to\utility.exe
Option10Name=Utility Name
Option11=...
; etc.
```

**Step 5: Update Keyboard Handler**
- Extend digit buffer logic for new option numbers
- Or add letter/function key handling as appropriate

**Step 6: Update Paint Handler**
- Add hover regions for new buttons (if mouse navigation implemented)

### Status: AWAITING INPUT
- Need: New background image
- Need: Program list with paths and commands

### Estimated Complexity: Medium (depends on number of programs)

---

## Installer (Post-v1.10)

### Description
Professional installer using Inno Setup for easy distribution. Created after v1.10 features are complete and will be reusable for future versions (1.20, etc.).

### Requirements (Confirmed)
1. **Small installer** - Do not bundle .NET runtime
2. **Detect .NET 6 Desktop Runtime x86**
   - If missing: Alert user with message
   - Provide option/link to download
   - **Block installation if .NET 6 is not installed**
3. **Install location:** `C:\MBBS Launcher` (not Program Files)
4. **Start Menu shortcut:** Always created
5. **Desktop shortcut:** Ask user (optional checkbox)
6. **Auto-start at login:** Ask user (optional checkbox)
   - Must sync with existing F12 config checkbox (`AutoLaunchAtStartup`)
   - If user selects during install, set the INI value and/or registry key
7. **Uninstaller:** Standard Windows uninstall support

### Implementation Plan (For Later)

**Inno Setup Script Structure:**
```iss
[Setup]
AppName=MBBS Launcher
AppVersion=1.10
AppPublisher=Mark Laudenbach
AppPublisherURL=https://github.com/laudenbachm/MBBS-Launcher
DefaultDirName=C:\MBBS Launcher
DefaultGroupName=MBBS Launcher
OutputBaseFilename=MBBSLauncher_Setup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Tasks]
Name: "desktopicon"; Description: "Create desktop shortcut"; GroupDescription: "Additional options:"
Name: "startupicon"; Description: "Start MBBS Launcher when Windows starts"; GroupDescription: "Startup options:"

[Files]
Source: "MBBS Launcher.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\MBBS Launcher"; Filename: "{app}\MBBS Launcher.exe"
Name: "{group}\Uninstall MBBS Launcher"; Filename: "{uninstallexe}"
Name: "{autodesktop}\MBBS Launcher"; Filename: "{app}\MBBS Launcher.exe"; Tasks: desktopicon

[Registry]
; If startup selected, add to Windows startup
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "MBBS Launcher"; ValueData: """{app}\MBBS Launcher.exe"""; Tasks: startupicon

[Code]
// Pascal script to check for .NET 6 Desktop Runtime x86
function IsDotNet6DesktopRuntimeInstalled(): Boolean;
var
  ResultCode: Integer;
begin
  // Check registry for .NET 6 Desktop Runtime
  Result := RegKeyExists(HKLM, 'SOFTWARE\WOW6432Node\dotnet\Setup\InstalledVersions\x86\sharedfx\Microsoft.WindowsDesktop.App');
  // Additional version checking logic here
end;

function InitializeSetup(): Boolean;
begin
  Result := True;
  if not IsDotNet6DesktopRuntimeInstalled() then
  begin
    MsgBox('MBBS Launcher requires .NET 6 Desktop Runtime (x86).' + #13#10 + #13#10 +
           'Please download and install it from:' + #13#10 +
           'https://dotnet.microsoft.com/download/dotnet/6.0' + #13#10 + #13#10 +
           'Select ".NET Desktop Runtime 6.0.x" for Windows x86.' + #13#10 + #13#10 +
           'Installation cannot continue without this runtime.',
           mbError, MB_OK);
    Result := False;  // Block installation
  end;
end;
```

### Files to Create (Later)
- `installer/MBBSLauncher.iss` - Inno Setup script
- `installer/build-installer.bat` - Build script

### Notes
- Will be created after v1.10 is feature-complete
- Same installer script can be versioned for 1.20, 1.30, etc.
- Consider code signing certificate in future (removes SmartScreen warning)

---

## Implementation Order for v1.10

1. **Feature 3: System Tray Icon** - Foundation for other features, enables quiet mode
2. **Feature 1: Mouse Navigation** - Standalone UX improvement
3. **Feature 2: Auto-Load BBS** - Uses tray for QuietMode
4. **Feature 4: Additional Options** - Once user provides program list and background image

**Post v1.10:**
5. **Installer** - Package the completed v1.10

---

## Version 1.2+ Planning Notes (Future)

### BBS Health Monitoring & Alerting

**Concept:** Monitor wgserver.exe to ensure it's not just running as a process but actually responding to connections. Alert sysop and optionally auto-restart.

#### Monitoring Options

**Option A: Telnet/TCP Connection Test**
- Attempt TCP connection to BBS port
- Problem: Creates log entry each time ("tickling" the service)
- May be acceptable with long intervals (e.g., every 5 minutes)

**Option B: Other Protocol Tests**
- Check if specific port is listening (doesn't require full connection)
- Use Windows `netstat` or socket bind test
- Less intrusive but less thorough

**Option C: Custom MBBS Module (Recommended - Exciting Idea!)**

Create a custom module for MBBS v10 that provides a health check endpoint for the launcher to query. This would enable:

1. **Status Check Without Log Entries**
   - Module provides lightweight status endpoint
   - Launcher queries without creating user session logs

2. **Rich Data from BBS**
   - Current user count
   - System uptime
   - Memory usage
   - Active channels/forums
   - Queue status

3. **Command Interface**
   - Graceful shutdown command
   - Restart command
   - Run cleanup routines
   - Clear caches
   - Custom sysop commands

4. **Implementation Approach**
   - Review MBBS v10 Module SDK: https://github.com/TheMajorBBS/MBBS-v10-module-SDK
   - Create module that listens on dedicated port (e.g., 9999)
   - Simple protocol: Launcher sends command, module responds with status
   - Could use HTTP REST, raw TCP, or named pipes

5. **Example Protocol**
   ```
   Launcher -> Module: PING
   Module -> Launcher: PONG 1705084800 OK

   Launcher -> Module: STATUS
   Module -> Launcher: USERS:5 UPTIME:3600 MEM:256MB OK

   Launcher -> Module: SHUTDOWN
   Module -> Launcher: SHUTDOWN_INITIATED OK
   ```

**This is a fantastic idea** - it would make the launcher much more powerful and avoid the log spam issue entirely. Worth exploring the SDK to assess feasibility.

#### Auto-Restart Logic
- Configurable retry count (e.g., 3 attempts)
- Configurable delay between retries (e.g., 30 seconds)
- Graceful shutdown attempt before restart
- Hard kill (`taskkill /F`) if graceful fails
- Stop retrying after max attempts

#### Alerting Options

**1. Email (SMTP)**
```ini
[EmailAlerts]
EnableEmailAlerts=false
SmtpHost=smtp.example.com
SmtpPort=587
SmtpUsername=user@example.com
SmtpPassword=<encrypted>
SmtpUseSsl=true
FromAddress=bbs-monitor@example.com
ToAddress=sysop@example.com
```
- Test button to verify configuration
- Password stored using Windows DPAPI encryption

**2. Webhooks (Discord, Teams, Slack, etc.)**
```ini
[WebhookAlerts]
EnableWebhookAlerts=false
WebhookUrl=https://discord.com/api/webhooks/xxx/yyy
WebhookType=Discord  ; or Teams, Slack, Generic
```

- Discord webhook format (JSON POST):
  ```json
  {
    "content": "BBS Alert: wgserver.exe is not responding!",
    "username": "MBBS Launcher"
  }
  ```

- Microsoft Teams webhook format (JSON POST):
  ```json
  {
    "@type": "MessageCard",
    "summary": "BBS Alert",
    "sections": [{
      "activityTitle": "MBBS Launcher Alert",
      "facts": [{
        "name": "Status",
        "value": "BBS not responding"
      }]
    }]
  }
  ```

- Test button to verify webhook works

**3. Alert Triggers**
- BBS detected as not responding
- Auto-restart initiated
- Auto-restart failed after X attempts
- BBS recovered (back online)
- Custom module status changes (if implemented)

#### Configuration UI
- Dedicated "Monitoring" tab or section in config editor
- Enable/disable monitoring checkbox
- Check interval (seconds/minutes)
- Port to check
- Max restart attempts
- Alert configuration (email and/or webhook)
- Test buttons for each alert method

---

## Questions Resolved

| Question | Answer |
|----------|--------|
| Auto-start BBS default | OFF (false) |
| Auto-start delay default | 5 seconds, user configurable |
| Installer .NET handling | Small installer, detect & block if missing, provide download link |
| Install location | `C:\MBBS Launcher` |
| Tray icon status | Yes, change icon when program running |
| Tray hover info | Yes, show running program name |
| Tray bring to front | Yes, context menu option |
| Installer timing | Post-v1.10, reusable for future versions |

## Questions Still Open

1. **Additional Programs:** Awaiting list of offline utilities and commands
2. **Background Image:** Awaiting updated image with space at bottom
3. **Telnet Port:** Default port for MBBS v10? (for v1.2 monitoring)
4. **Custom Module Interest:** Should we explore the MBBS module SDK for v1.2?

---

## Files & Folders Reference

```
v1.10/
├── notes/
│   ├── PLANNING.md          (this file)
│   ├── CHANGELOG.md         (changes made during development)
│   └── ISSUES.md            (problems encountered and solutions)
├── builds/
│   └── (timestamped builds during development)
└── installer/
    └── (installer files created post-v1.10)
```

---

## Next Steps

1. [x] Review and confirm feature requirements
2. [ ] Begin Feature 3: System Tray Icon implementation
3. [ ] User to provide program list for Feature 4
4. [ ] User to provide updated background image

---

*Plan created: January 12, 2026*
*Last updated: January 12, 2026*
