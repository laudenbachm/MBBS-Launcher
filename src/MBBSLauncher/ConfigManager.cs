// MBBS Launcher - Configuration Manager
// Created by Mark Laudenbach with Love in Iowa
// https://github.com/laudenbachm/MBBS-Launcher
//
// File: ConfigManager.cs
// Version: v1.20
//
// Change History:
// 26.01.07.1 - 06:00PM - Initial creation
// 26.01.12.1 - Added system tray configuration options
// 26.01.12.2 - Added AutoStartBBS, AutoStartDelay, QuietMode settings
// 26.01.23.1 - Added Ghost3 support settings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MBBSLauncher
{
    /// <summary>
    /// Manages INI configuration file for MBBS Launcher
    /// </summary>
    public class ConfigManager
    {
        private const string CONFIG_FILENAME = "MBBSLauncher.ini";
        private readonly string _configFilePath;
        private Dictionary<string, Dictionary<string, string>> _configData;

        public ConfigManager()
        {
            // Use executable's directory for config file (not working directory)
            // This ensures config is found when app starts from Windows startup
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            _configFilePath = Path.Combine(exeDir, CONFIG_FILENAME);

            _configData = new Dictionary<string, Dictionary<string, string>>();
            LoadConfig();
        }

        /// <summary>
        /// Loads configuration from INI file
        /// </summary>
        public void LoadConfig()
        {
            _configData.Clear();

            if (!File.Exists(_configFilePath))
            {
                CreateDefaultConfig();
                return;
            }

            try
            {
                string currentSection = "";
                foreach (string line in File.ReadAllLines(_configFilePath))
                {
                    string trimmedLine = line.Trim();

                    // Skip empty lines and comments
                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#"))
                        continue;

                    // Section header
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
                        if (!_configData.ContainsKey(currentSection))
                            _configData[currentSection] = new Dictionary<string, string>();
                        continue;
                    }

                    // Key-value pair
                    int equalIndex = trimmedLine.IndexOf('=');
                    if (equalIndex > 0 && !string.IsNullOrEmpty(currentSection))
                    {
                        string key = trimmedLine.Substring(0, equalIndex).Trim();
                        string value = trimmedLine.Substring(equalIndex + 1).Trim();
                        _configData[currentSection][key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Error loading configuration: {ex.Message}\nUsing default configuration.",
                    "Configuration Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                CreateDefaultConfig();
            }
        }

        /// <summary>
        /// Saves configuration to INI file
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("; MBBS Launcher Configuration File");
                sb.AppendLine($"; Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine();

                foreach (var section in _configData)
                {
                    sb.AppendLine($"[{section.Key}]");
                    foreach (var kvp in section.Value)
                    {
                        sb.AppendLine($"{kvp.Key}={kvp.Value}");
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(_configFilePath, sb.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Error saving configuration: {ex.Message}",
                    "Configuration Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates default configuration
        /// </summary>
        private void CreateDefaultConfig()
        {
            _configData.Clear();

            // Paths section
            _configData["Paths"] = new Dictionary<string, string>
            {
                { "BBSPath", @"C:\BBSV10" }
            };

            // Window section
            _configData["Window"] = new Dictionary<string, string>
            {
                { "X", "" },
                { "Y", "" },
                { "Width", "960" },
                { "Height", "540" }
            };

            // Settings section
            _configData["Settings"] = new Dictionary<string, string>
            {
                { "AutoLaunchAtStartup", "false" },
                { "MinimizeToTray", "true" },
                { "ShowTrayIcon", "true" },
                { "EscMinimizesToTray", "false" }, // ESC minimizes to taskbar by default, true = minimize to tray
                { "AutoStartBBS", "false" },       // OFF by default - auto-launch BBS when launcher starts
                { "AutoStartDelay", "5" },         // Seconds to wait before auto-starting (allows cancel)
                { "QuietMode", "false" },          // Minimize to tray after auto-start
                { "Ghost3Enabled", "false" },      // OFF by default - auto-launch Ghost3 after BBS starts
                { "Ghost3Path", @"C:\Ghost3\Ghost3.exe" }, // Default Ghost3 installation path
                { "Ghost3Delay", "60" }            // Seconds to wait after BBS starts before launching Ghost3
            };

            // Programs section
            _configData["Programs"] = new Dictionary<string, string>
            {
                { "Option1", @"C:\BBSV10\WGSCNF.exe -L1" },
                { "Option1Name", "Hardware Setup" },
                { "Option2", @"C:\BBSV10\wgsrunmt.exe" },
                { "Option2Name", "Design Menu Tree" },
                { "Option3", @"C:\BBSV10\WGSCNF.exe -L3" },
                { "Option3Name", "Security & Accounting" },
                { "Option4", @"C:\BBSV10\WGSCNF.exe -L4" },
                { "Option4Name", "Configuration Options" },
                { "Option5", @"C:\BBSV10\wgsappgo.exe" },
                { "Option5Name", "Go!" },
                { "Option6", @"C:\BBSV10\WGSCNF.exe -L6" },
                { "Option6Name", "Edit Text Blocks" },
                { "Option7", @"C:\BBSV10\WGSUMENU.exe" },
                { "Option7Name", "Basic Utilities" },
                { "Option8", @"C:\BBSV10\WGSRPT.exe" },
                { "Option8Name", "Reports" },
                { "Option9", "" },
                { "Option9Name", "" },
                { "Option99", @"C:\BBSV10\WGSCNF.exe -L99" },
                { "Option99Name", "CNF 99" }
            };

            SaveConfig();
        }

        /// <summary>
        /// Gets a configuration value
        /// </summary>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            if (_configData.ContainsKey(section) && _configData[section].ContainsKey(key))
                return _configData[section][key];
            return defaultValue;
        }

        /// <summary>
        /// Sets a configuration value
        /// </summary>
        public void SetValue(string section, string key, string value)
        {
            if (!_configData.ContainsKey(section))
                _configData[section] = new Dictionary<string, string>();

            _configData[section][key] = value;
        }

        /// <summary>
        /// Gets all keys in a section
        /// </summary>
        public Dictionary<string, string> GetSection(string section)
        {
            if (_configData.ContainsKey(section))
                return new Dictionary<string, string>(_configData[section]);
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Searches for BBSV10 folder on the system
        /// </summary>
        public void SearchForBBSFolders()
        {
            try
            {
                // Check common locations
                string[] drivesToCheck = { "C:\\", "D:\\", "E:\\" };
                string[] foldersToFind = { "BBSV10", "WGSERV" };

                foreach (string drive in drivesToCheck)
                {
                    try
                    {
                        if (!Directory.Exists(drive))
                            continue;

                        foreach (string folder in foldersToFind)
                        {
                            try
                            {
                                string path = Path.Combine(drive, folder);
                                if (Directory.Exists(path))
                                {
                                    SetValue("Paths", "BBSPath", path);
                                    return; // Found it, exit
                                }
                            }
                            catch
                            {
                                // Skip this folder if there's an error
                                continue;
                            }
                        }
                    }
                    catch
                    {
                        // Skip this drive if there's an error
                        continue;
                    }
                }
            }
            catch
            {
                // If folder search completely fails, just skip it
            }
        }
    }
}
