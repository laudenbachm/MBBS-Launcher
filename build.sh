#!/bin/bash
# MBBS Launcher Build Script
# Creates timestamped builds in separate folders

# Add .NET to PATH
export PATH="/c/Users/Mark/AppData/Local/Microsoft/dotnet:$HOME/.dotnet:$PATH"

# Get timestamp for folder name
BUILD_TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BUILD_DIR="builds/build_${BUILD_TIMESTAMP}"

# Create build directory
mkdir -p "$BUILD_DIR"

echo "================================================"
echo "MBBS Launcher Build Script"
echo "================================================"
echo "Build folder: $BUILD_DIR"
echo "Timestamp: $BUILD_TIMESTAMP"
echo ""

# Copy latest images from images folder to Resources before building
echo "Updating Resources with latest images..."
if [ -f "images/MBBS Launcher Screen.png" ]; then
    cp "images/MBBS Launcher Screen.png" "src/MBBSLauncher/Resources/background.png"
    echo "  ✓ Updated background.png"
fi

if [ -f "images/Windows Icon.ico" ]; then
    cp "images/Windows Icon.ico" "src/MBBSLauncher/Resources/icon.ico"
    echo "  ✓ Updated icon.ico"
fi

echo ""

# Navigate to project directory
cd "src/MBBSLauncher"

echo "Restoring dependencies..."
dotnet restore

echo ""
echo "Building Release configuration..."
dotnet build -c Release

echo ""
echo "Publishing single-file executable for Windows x86..."
dotnet publish -c Release -r win-x86 --self-contained false -p:PublishSingleFile=true -o "../../${BUILD_DIR}"

# Go back to root
cd ../..

echo ""
echo "Copying documentation..."
cp README.md "$BUILD_DIR/"
cp LICENSE "$BUILD_DIR/"
cp CHANGELOG.md "$BUILD_DIR/"

# Create build info file
cat > "$BUILD_DIR/BUILD_INFO.txt" <<EOF
MBBS Launcher v1.00
Build Date: $(date +"%Y-%m-%d %H:%M:%S")
Build Number: $BUILD_TIMESTAMP
Platform: Windows x86 (32-bit)
.NET Version: 6.0
Build Type: Release (Single-file executable)

Files included:
- MBBS Launcher.exe (Single-file application with embedded resources)
- README.md (Project documentation)
- LICENSE (MIT License)
- CHANGELOG.md (Version history)

Features:
- All resources (images, icons) are embedded in the EXE
- No external files needed except configuration
- Clickable menu options (keyboard input still supported)

To run:
1. Ensure .NET 6.0 Desktop Runtime is installed
2. Double-click "MBBS Launcher.exe"
3. On first run, configure paths using F12

Created by Mark Laudenbach with Love in Iowa
https://github.com/laudenbachm/MBBS-Launcher
EOF

echo ""
echo "================================================"
echo "Build Complete!"
echo "================================================"
echo "Output location: $BUILD_DIR"
echo ""
echo "Contents:"
ls -lh "$BUILD_DIR"
echo ""
echo "To test, copy the entire folder to a Windows machine"
echo "or run from WSL using: explorer.exe $BUILD_DIR"
echo "================================================"
