@echo off
REM Build MBBS Launcher without single-file publishing
REM This version is less likely to trigger antivirus false positives

echo Building MBBS Launcher (Standard Distribution)...
cd src\MBBSLauncher

dotnet publish -c Release -r win-x86 --self-contained false ^
    /p:PublishSingleFile=false ^
    /p:OutputPath="..\..\builds\MBBS Launcher v1.10 - Standard\"

echo.
echo Build complete! Output in: builds\MBBS Launcher v1.10 - Standard\
pause
