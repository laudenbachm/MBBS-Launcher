# MBBS Launcher v1.10 - Development Changelog

This file tracks all changes made during v1.10 development.

---

## [Unreleased] - v1.10

### Planning Phase
- **2026-01-12** - Created v1.10 folder structure
- **2026-01-12** - Created PLANNING.md with feature specifications
- **2026-01-12** - Documented v1.2 monitoring feature ideas

### Added
- **2026-01-12** - Added MinimizeToTray and ShowTrayIcon settings to ConfigManager.cs
- **2026-01-12** - Added System Tray Icon feature to MainForm.cs:
  - NotifyIcon with context menu (Show Launcher, Start BBS, Bring to Front, Configuration, Exit)
  - Dynamic tooltip showing running program name
  - "Bring to Front" menu item appears when program is running
  - Minimize to tray when launching programs
  - Balloon notification on first minimize to tray
  - Double-click tray icon to restore launcher
- **2026-01-12** - Added running program state tracking (_runningProgramName, _runningProcess)
- **2026-01-12** - Added Mouse Navigation feature:
  - Hover detection for all clickable options (1-9, 0, 99) and URL links
  - Visual hover highlight with semi-transparent cyan overlay
  - Hand cursor when hovering over clickable areas
  - GetOptionAtPosition() and GetOptionBounds() helper methods
- **2026-01-12** - Added Auto-Load BBS on Startup feature:
  - AutoStartBBS, AutoStartDelay, QuietMode config settings
  - Visual countdown with "Press any key or click to cancel" message
  - Cancellation via any key press or mouse click
  - QuietMode to minimize to tray after auto-start
- **2026-01-12** - Added F1 Help dialog (HelpForm.cs):
  - Comprehensive help documentation
  - Keyboard shortcuts reference
  - Feature explanations and troubleshooting
- **2026-01-12** - Added F2 Enable/Disable Modules:
  - Launches WGSDMOD.exe from BBS path
  - Full tray integration when running

### Changed
- **2026-01-12** - Updated ConfigManager.cs version to v1.10
- **2026-01-12** - Updated MainForm.cs version to v1.10
- **2026-01-12** - Updated MainForm.Designer.cs version to v1.10
- **2026-01-12** - Updated Program.cs APP_VERSION to v1.10
- **2026-01-12** - LaunchOption now minimizes to tray instead of hiding completely
- **2026-01-12** - Program exit now clears tray status and restores launcher

### Fixed
- **2026-01-12** - Fixed auto-launch at startup checkbox not persisting (now reads from registry)
- **2026-01-12** - Fixed ConfigEditorForm Option 99 section (duplicate variables, compile errors)
- **2026-01-12** - Fixed config file not found when app starts from Windows startup (was using relative path)
- **2026-01-13** - Enhanced display repaint on restore using BeginInvoke, Invalidate(true), Update()

### Removed
- **2026-01-12** - Removed arrow key navigation (yellow selection boxes didn't look right)
- **2026-01-13** - Removed hover highlight boxes (cyan overlays)

### Changed (continued)
- **2026-01-12** - ESC key now minimizes to taskbar (instead of exiting), with optional tray minimize
- **2026-01-12** - F10 key now closes the program
- **2026-01-12** - Option names in config editor now display as fixed labels (match background image)
- **2026-01-12** - F2 option in config editor now uses same layout as options 1-8
- **2026-01-12** - Tray icon shows "Running: The Major BBS" for Option 5 (instead of "Go!")

---

## Feature Implementation Log

### Feature 1: Mouse Navigation
- [x] Added _hoveredOption tracking field
- [x] MouseMove and MouseLeave event handlers
- [x] GetOptionAtPosition() helper method
- [x] GetOptionBounds() for hit regions
- [x] DrawHoverHighlight() with cyan overlay
- [x] Hand cursor on hover
- [x] Testing pending

### Feature 2: Auto-Load BBS on Startup
- [x] Added config settings (AutoStartBBS, AutoStartDelay, QuietMode)
- [x] CheckAutoStartBBS() on form shown
- [x] AutoStartTimer for countdown
- [x] DrawAutoStartCountdown() visual display
- [x] Cancel on keypress (MainForm_KeyDown)
- [x] Cancel on mouse click (MainForm_MouseClick)
- [x] QuietMode integration
- [x] Testing pending

### Feature 3: System Tray Icon
- [x] Added config settings (MinimizeToTray, ShowTrayIcon)
- [x] MainForm.cs NotifyIcon implementation
- [x] Context menu with all planned items
- [x] Dynamic tooltip with running program name
- [x] "Bring to Front" functionality
- [x] Minimize to tray behavior
- [x] Dispose cleanup for NotifyIcon
- [ ] ConfigEditorForm UI for tray settings (deferred - settings work via INI)
- [x] Testing pending

### Feature 4: Additional Program Options
- [ ] Awaiting program list and background image

### Installer (Post v1.10)
- [ ] Deferred to after v1.10 is complete

---

*Log started: January 12, 2026*
