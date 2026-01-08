# Quick Build Guide

## Automated Build Script

The project includes an automated build script that creates timestamped builds in separate folders.

### Usage

```bash
./build.sh
```

Each build is placed in a unique folder: `builds/build_YYYYMMDD_HHMMSS/`

### Build Output

Each build folder contains:
- **MBBSLauncher.exe** - The main application (1.3 MB)
- **MBBSLauncher.pdb** - Debug symbols (optional, can be deleted for distribution)
- **Resources/** - Background image and icon
- **README.md** - Project documentation
- **LICENSE** - MIT License
- **CHANGELOG.md** - Version history
- **BUILD_INFO.txt** - Build timestamp and details

### Testing a Build

#### From WSL:
```bash
# Open the build folder in Windows Explorer
explorer.exe builds/build_YYYYMMDD_HHMMSS/

# Or navigate to the folder
cd builds/build_YYYYMMDD_HHMMSS/
```

#### On Windows:
1. Copy the entire build folder to your Windows machine
2. Double-click `MBBSLauncher.exe` to run
3. If .NET 6.0 Desktop Runtime is not installed, download from:
   https://dotnet.microsoft.com/download/dotnet/6.0

### Build System Details

- **Platform:** Windows x86 (32-bit)
- **.NET Version:** 6.0
- **Build Type:** Release
- **Single File:** Yes (all dependencies bundled)
- **Self-Contained:** No (requires .NET 6.0 Desktop Runtime)

### Multiple Builds

The build script automatically creates timestamped folders, so you can:
- Keep multiple build versions
- Test different builds side-by-side
- Compare changes between builds
- No need to manually rename or backup files

### Example Build Folders

```
builds/
├── build_20260107_230736/  <- Current build
├── build_20260107_140230/  <- Earlier build
└── build_20260107_095512/  <- First build
```

### Clean Up Old Builds

To remove old builds:
```bash
# Remove all builds
rm -rf builds/

# Remove specific build
rm -rf builds/build_20260107_230736/
```

### Build Warnings

The build may show these warnings (safe to ignore):
- **CS8618** - Non-nullable field warnings (fields are initialized in InitializeCustomControls)
- **NETSDK1074** - Application host not customized on Linux (icon/manifest requires Windows build)

These warnings don't affect functionality and can be ignored for testing.

### Known Limitations

- Icon file (.ico) not included - needs conversion from PNG on Windows
- Application host customization requires Windows build
- Some Win32 features may not be fully testable from WSL

---

**Next Steps:** Copy a build folder to Windows and test the application!
