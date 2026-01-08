# Build Instructions for MBBS Launcher

This document provides detailed instructions for building MBBS Launcher from source code.

## Prerequisites

### Required Software

1. **.NET 6.0 SDK** (or later)
   - Download: https://dotnet.microsoft.com/download/dotnet/6.0
   - Choose the SDK (not just runtime)
   - Verify installation: `dotnet --version`

2. **Windows Operating System** (for testing)
   - Windows 10/11 recommended for development
   - Windows Server 2016+ also supported

3. **Git** (for cloning repository)
   - Download: https://git-scm.com/downloads

### Optional Software

1. **Visual Studio 2022** (Community, Professional, or Enterprise)
   - Provides IDE with WinForms designer
   - Download: https://visualstudio.microsoft.com/
   - Required workload: ".NET desktop development"

2. **Visual Studio Code**
   - Lightweight alternative to Visual Studio
   - Install C# extension

## Building from Command Line

### Step 1: Clone the Repository

```bash
git clone https://github.com/laudenbachm/MBBS-Launcher.git
cd MBBS-Launcher
```

### Step 2: Restore Dependencies

```bash
cd src/MBBSLauncher
dotnet restore
```

### Step 3: Build the Project

For a standard build:
```bash
dotnet build -c Release
```

The output will be in: `bin/Release/net6.0-windows/`

### Step 4: Create Single-File Executable

To create a single-file 32-bit executable:

```bash
dotnet publish -c Release -r win-x86 --self-contained false /p:PublishSingleFile=true
```

The single-file executable will be in: `bin/Release/net6.0-windows/win-x86/publish/`

### Alternative: Self-Contained Build

If you want to include the .NET runtime (larger file, no runtime installation required):

```bash
dotnet publish -c Release -r win-x86 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

## Building with Visual Studio 2022

### Step 1: Open the Project

1. Launch Visual Studio 2022
2. File → Open → Project/Solution
3. Navigate to `MBBS-Launcher/src/MBBSLauncher/`
4. Open `MBBSLauncher.csproj`

### Step 2: Set Build Configuration

1. In the toolbar, change configuration from "Debug" to "Release"
2. Ensure platform is set to "x86" (32-bit)

### Step 3: Build

1. Build → Build Solution (or press Ctrl+Shift+B)
2. Check Output window for build results

### Step 4: Publish Single-File Executable

1. Right-click the project in Solution Explorer
2. Click "Publish"
3. Create new publish profile:
   - Target: Folder
   - Configuration: Release | win-x86
   - Deployment Mode: Framework-dependent
   - File publish options:
     - ☑ Produce single file
     - ☑ Enable ReadyToRun compilation (optional)
4. Click "Publish"

The executable will be in: `bin/Release/net6.0-windows/win-x86/publish/`

## Post-Build Steps

### 1. Copy Required Resources

Ensure these files are in the same directory as `MBBSLauncher.exe`:

```
MBBSLauncher/
├── MBBSLauncher.exe
└── Resources/
    ├── background.png
    └── icon.ico (if converted)
```

Copy the images from the project:
```bash
mkdir publish/Resources
copy ..\..\images\background.png publish\Resources\
```

### 2. Convert Icon (Optional)

If you need to convert the PNG icon to ICO format:

**Using online tool:**
- Visit https://convertio.co/png-ico/
- Upload `images/MBBS Launcher ICON 1024x1024.png`
- Download the ICO file
- Save as `src/MBBSLauncher/Resources/icon.ico`

**Using ImageMagick (if installed):**
```bash
magick convert "images/MBBS Launcher ICON 1024x1024.png" -define icon:auto-resize=256,128,64,48,32,16 "src/MBBSLauncher/Resources/icon.ico"
```

### 3. Rebuild with Icon

After adding the ICO file:
1. Rebuild the project
2. The icon will be embedded in the executable

## Testing the Build

1. Navigate to the publish directory
2. Ensure `Resources/background.png` exists
3. Run `MBBSLauncher.exe`
4. On first run, it will create `MBBSLauncher.ini`
5. Test the configuration editor (F12)
6. Test program launching (if you have BBS utilities configured)

## Build Configurations

### Debug Build
- Includes debugging symbols
- No optimizations
- Larger file size
- Use for development and testing

```bash
dotnet build -c Debug
```

### Release Build
- Optimized for performance
- No debugging symbols
- Smaller file size
- Use for distribution

```bash
dotnet build -c Release
```

## Troubleshooting

### "Project targets framework that requires .NET 6.0"
- Install .NET 6.0 SDK (not just runtime)
- Verify with: `dotnet --version`

### "Windows is required for this project"
- WinForms applications can only be built on Windows
- If using WSL/Linux, the code can be written but must be built on Windows

### "Icon file not found"
- Ensure `Resources/icon.ico` exists
- Or comment out the `<ApplicationIcon>` line in .csproj
- Or use a PNG temporarily (will be embedded differently)

### Single-file publish fails
- Ensure .NET 6.0 SDK or later is installed
- Try without single-file first: `dotnet publish -c Release -r win-x86`
- Check for any missing dependencies

### Runtime errors on target machine
- Ensure .NET 6.0 Desktop Runtime is installed on target
- For self-contained builds, the runtime is included (larger file)

## Creating a Distribution Package

1. Build the single-file executable (Release)
2. Create a ZIP file with:
   ```
   MBBS-Launcher-v1.00/
   ├── MBBSLauncher.exe
   ├── Resources/
   │   └── background.png
   ├── README.md
   └── LICENSE
   ```
3. Upload to GitHub Releases

## Continuous Integration (Future)

For automated builds, consider:
- GitHub Actions workflow
- Azure DevOps pipeline
- Automatic version tagging
- Release artifact publishing

---

**Build Support:** For issues or questions about building, please open an issue on GitHub.

**Last Updated:** 2026-01-07
