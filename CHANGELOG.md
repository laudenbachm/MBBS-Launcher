# Changelog

All notable changes to MBBS Launcher will be documented in this file.

The format for change tracking: `YY.MM.DD.X - HH:MMAM/PM`
- YY = Year (26 = 2026)
- MM = Month
- DD = Day
- X = Change number for that day (starts at 1, increments with each change)
- HH:MM = Time in 12-hour format with AM/PM

## [v1.00] - 2026-01-07

### 26.01.07.1 - 06:00PM
**Initial Release**

#### Added
- Classic DOS-style retro interface inspired by Major BBS v6.25 launcher
- Background image support with 16:9 scalable window
- 9 configurable menu options (1-9) plus Exit (0)
- Keyboard input support (number keys 0-9)
- F12 configuration editor
- INI-based configuration system (`MBBSLauncher.ini`)
- Process detection for running programs
- WGServer detection to prevent conflicts
- Auto-hide/show functionality when launching programs
- Bring-to-foreground capability for already running applications
- Automatic search for BBSV10 and WGSERV folders on first run
- Configuration editor with:
  - Path configuration for BBSV10 and WGSERV
  - Program path configuration for all 9 menu options
  - Custom naming for menu options
  - Browse dialogs for folder and file selection
- Special handling for wgsappgo.exe → wgserver.exe process chain
- Version information embedded in executable
- GitHub repository integration
- MIT License
- Comprehensive README and documentation

#### Technical Details
- Built with .NET 6.0 WinForms
- 32-bit (x86) executable
- Single-file publish support
- Compatible with Windows 7+, Windows Server 2012+
- File change tracking implemented in all source files

#### File Structure
```
MBBSLauncher/
├── src/
│   └── MBBSLauncher/
│       ├── Forms/
│       │   ├── MainForm.cs
│       │   ├── MainForm.Designer.cs
│       │   ├── ConfigEditorForm.cs
│       │   └── ConfigEditorForm.Designer.cs
│       ├── Resources/
│       │   ├── background.png
│       │   └── icon.png
│       ├── Program.cs
│       ├── ConfigManager.cs
│       ├── ProcessHelper.cs
│       └── MBBSLauncher.csproj
├── images/
│   ├── MBBS Launcher Screen.png
│   └── MBBS Launcher ICON 1024x1024.png
├── docs/
├── README.md
├── CHANGELOG.md
├── LICENSE
└── .gitignore
```

#### Known Limitations
- Icon file needs conversion from PNG to ICO format (requires Windows tools)
- Mouse selection with arrow keys planned for future version
- Single executable requires .NET 6.0 Desktop Runtime on target system

---

## Version Numbering Scheme

MBBS Launcher uses the following versioning scheme:
- **Major.Minor** format (e.g., v1.00)
- Major version increments for significant feature additions or breaking changes
- Minor version increments for bug fixes and small features
- Individual file changes tracked in source code headers using YY.MM.DD.X format

---

**Note:** This changelog follows the [Keep a Changelog](https://keepachangelog.com/) format and uses [Semantic Versioning](https://semver.org/) principles adapted for the project's versioning scheme.
