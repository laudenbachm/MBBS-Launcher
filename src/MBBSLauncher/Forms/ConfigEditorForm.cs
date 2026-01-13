// MBBS Launcher - Configuration Editor Form
// Created by Mark Laudenbach with Love in Iowa
// https://github.com/laudenbachm/MBBS-Launcher
//
// File: Forms/ConfigEditorForm.cs
// Version: v1.10
//
// Change History:
// 26.01.07.1 - 06:00PM - Initial creation
// 26.01.12.1 - Added Auto-Start BBS settings and F2 Module Editor option

using System;
using System.Drawing;
using System.Windows.Forms;

namespace MBBSLauncher.Forms
{
    public partial class ConfigEditorForm : Form
    {
        private ConfigManager _config;
        private TextBox? _bbsPathTextBox;
        private TextBox[] _programTextBoxes = Array.Empty<TextBox>();
        private TextBox? _program99TextBox;
        private CheckBox? _autoLaunchCheckBox;

        // Auto-Start BBS controls
        private CheckBox? _autoStartBBSCheckBox;
        private NumericUpDown? _autoStartDelayNumeric;
        private CheckBox? _quietModeCheckBox;

        // Behavior settings
        private CheckBox? _escToTrayCheckBox;

        // F2 Module Editor
        private TextBox? _moduleEditorTextBox;

        public ConfigEditorForm(ConfigManager config)
        {
            _config = config;
            InitializeComponent();
            InitializeCustomControls();
            LoadConfiguration();
        }

        private void InitializeCustomControls()
        {
            this.Text = $"{Program.APP_NAME} {Program.APP_VERSION} - Configuration Editor";
            this.Size = new Size(700, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Load application icon
            LoadApplicationIcon();

            int yPos = 20;

            // Header section - Line 1: MBBS Launcher version
            Label versionLabel = new Label
            {
                Text = $"{Program.APP_NAME} {Program.APP_VERSION}",
                Location = new Point(20, yPos),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(versionLabel);
            yPos += 28;

            // Header section - Line 2: GitHub link
            LinkLabel githubLink = new LinkLabel
            {
                Text = Program.GITHUB_URL,
                Location = new Point(20, yPos),
                Size = new Size(650, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = Color.FromArgb(0, 102, 204)
            };
            githubLink.LinkClicked += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = Program.GITHUB_URL,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not open browser: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            this.Controls.Add(githubLink);
            yPos += 25;

            // Header section - Line 3: Created with Love by...
            Label authorLabel = new Label
            {
                Text = $"Created with Love \u2764 by {Program.AUTHOR} in Iowa",
                Location = new Point(20, yPos),
                Size = new Size(650, 22),
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(220, 20, 60), // Crimson red
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(authorLabel);
            yPos += 35;

            // Auto-launch at startup checkbox
            _autoLaunchCheckBox = new CheckBox
            {
                Text = "Launch MBBS Launcher automatically at Windows startup",
                Location = new Point(20, yPos),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 9),
                Checked = false
            };
            this.Controls.Add(_autoLaunchCheckBox);
            yPos += 30;

            // Auto-Start BBS section
            Label autoStartLabel = new Label
            {
                Text = "Auto-Start BBS Settings:",
                Location = new Point(20, yPos),
                Size = new Size(650, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(autoStartLabel);
            yPos += 25;

            _autoStartBBSCheckBox = new CheckBox
            {
                Text = "Automatically start BBS (Option 5) when launcher opens",
                Location = new Point(40, yPos),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 9),
                Checked = false
            };
            this.Controls.Add(_autoStartBBSCheckBox);

            Label delayLabel = new Label
            {
                Text = "Delay:",
                Location = new Point(450, yPos + 3),
                Size = new Size(45, 20)
            };
            this.Controls.Add(delayLabel);

            _autoStartDelayNumeric = new NumericUpDown
            {
                Location = new Point(495, yPos),
                Size = new Size(50, 25),
                Minimum = 0,
                Maximum = 60,
                Value = 5
            };
            this.Controls.Add(_autoStartDelayNumeric);

            Label secondsLabel = new Label
            {
                Text = "seconds",
                Location = new Point(550, yPos + 3),
                Size = new Size(60, 20)
            };
            this.Controls.Add(secondsLabel);
            yPos += 30;

            _quietModeCheckBox = new CheckBox
            {
                Text = "Quiet mode (minimize to tray after auto-start)",
                Location = new Point(40, yPos),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 9),
                Checked = false
            };
            this.Controls.Add(_quietModeCheckBox);
            yPos += 30;

            _escToTrayCheckBox = new CheckBox
            {
                Text = "ESC key minimizes to system tray (instead of taskbar)",
                Location = new Point(40, yPos),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 9),
                Checked = false
            };
            this.Controls.Add(_escToTrayCheckBox);
            yPos += 35;

            // Paths section
            Label pathsLabel = new Label
            {
                Text = "BBS Installation Path:",
                Location = new Point(20, yPos),
                Size = new Size(650, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(pathsLabel);
            yPos += 30;

            Label bbsLabel = new Label
            {
                Text = "BBS Path:",
                Location = new Point(40, yPos),
                Size = new Size(120, 20)
            };
            this.Controls.Add(bbsLabel);

            _bbsPathTextBox = new TextBox
            {
                Location = new Point(160, yPos),
                Size = new Size(400, 20),
                Tag = "BBSPath"
            };
            this.Controls.Add(_bbsPathTextBox);

            Button browseBtn = new Button
            {
                Text = "Browse...",
                Location = new Point(570, yPos - 2),
                Size = new Size(80, 24),
                Tag = _bbsPathTextBox
            };
            browseBtn.Click += BrowseButton_Click;
            this.Controls.Add(browseBtn);

            yPos += 40;

            // Programs section
            Label programsLabel = new Label
            {
                Text = "Launcher Options:",
                Location = new Point(20, yPos),
                Size = new Size(650, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(programsLabel);
            yPos += 30;

            // Scrollable panel for programs
            Panel scrollPanel = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(650, 300),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(scrollPanel);

            // Static option names (matching background image)
            string[] optionNames = new string[]
            {
                "Hardware Setup",        // 1
                "Design Menu Tree",      // 2
                "Security & Accounting", // 3
                "Configuration Options", // 4
                "Go!",                   // 5
                "Edit Text Blocks",      // 6
                "Basic Utilities",       // 7
                "Reports"                // 8
            };

            _programTextBoxes = new TextBox[8];

            int innerYPos = 10;
            for (int i = 1; i <= 8; i++)
            {
                // Option number and name as single bold label
                Label optionLabel = new Label
                {
                    Text = $"{i} - {optionNames[i - 1]}",
                    Location = new Point(10, innerYPos),
                    Size = new Size(200, 20),
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };
                scrollPanel.Controls.Add(optionLabel);

                Label pathLabel = new Label
                {
                    Text = "Program:",
                    Location = new Point(220, innerYPos),
                    Size = new Size(60, 20)
                };
                scrollPanel.Controls.Add(pathLabel);

                _programTextBoxes[i - 1] = new TextBox
                {
                    Location = new Point(280, innerYPos),
                    Size = new Size(250, 20),
                    Tag = $"Option{i}"
                };
                scrollPanel.Controls.Add(_programTextBoxes[i - 1]);

                Button browseProgramBtn = new Button
                {
                    Text = "Browse...",
                    Location = new Point(540, innerYPos - 2),
                    Size = new Size(80, 24),
                    Tag = _programTextBoxes[i - 1]
                };
                browseProgramBtn.Click += BrowseProgramButton_Click;
                scrollPanel.Controls.Add(browseProgramBtn);

                innerYPos += 30;
            }

            // Add Option 99
            Label option99Label = new Label
            {
                Text = "99 - CNF 99",
                Location = new Point(10, innerYPos),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            scrollPanel.Controls.Add(option99Label);

            Label path99Label = new Label
            {
                Text = "Program:",
                Location = new Point(220, innerYPos),
                Size = new Size(60, 20)
            };
            scrollPanel.Controls.Add(path99Label);

            _program99TextBox = new TextBox
            {
                Location = new Point(280, innerYPos),
                Size = new Size(250, 20),
                Tag = "Option99"
            };
            scrollPanel.Controls.Add(_program99TextBox);

            Button browse99Btn = new Button
            {
                Text = "Browse...",
                Location = new Point(540, innerYPos - 2),
                Size = new Size(80, 24),
                Tag = _program99TextBox
            };
            browse99Btn.Click += BrowseProgramButton_Click;
            scrollPanel.Controls.Add(browse99Btn);

            innerYPos += 30;

            // F2 - Enable / Disable Modules (hidden option, same layout as options 1-8)
            Label f2Label = new Label
            {
                Text = "F2 - Enable / Disable Modules",
                Location = new Point(10, innerYPos),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            scrollPanel.Controls.Add(f2Label);

            Label modulePathLabel = new Label
            {
                Text = "Program:",
                Location = new Point(220, innerYPos),
                Size = new Size(60, 20)
            };
            scrollPanel.Controls.Add(modulePathLabel);

            _moduleEditorTextBox = new TextBox
            {
                Location = new Point(280, innerYPos),
                Size = new Size(250, 20),
                Tag = "ModuleEditor"
            };
            scrollPanel.Controls.Add(_moduleEditorTextBox);

            Button browseF2Btn = new Button
            {
                Text = "Browse...",
                Location = new Point(540, innerYPos - 2),
                Size = new Size(80, 24),
                Tag = _moduleEditorTextBox
            };
            browseF2Btn.Click += BrowseProgramButton_Click;
            scrollPanel.Controls.Add(browseF2Btn);

            yPos += 310;

            // Buttons
            Button saveBtn = new Button
            {
                Text = "Save",
                Location = new Point(480, yPos),
                Size = new Size(90, 30),
                DialogResult = DialogResult.OK
            };
            saveBtn.Click += SaveButton_Click;
            this.Controls.Add(saveBtn);

            Button cancelBtn = new Button
            {
                Text = "Cancel",
                Location = new Point(580, yPos),
                Size = new Size(90, 30),
                DialogResult = DialogResult.Cancel
            };
            this.Controls.Add(cancelBtn);

            this.AcceptButton = saveBtn;
            this.CancelButton = cancelBtn;
        }

        private void LoadConfiguration()
        {
            // Load BBS path
            if (_bbsPathTextBox != null)
            {
                _bbsPathTextBox.Text = _config.GetValue("Paths", "BBSPath");
            }

            // Load programs (Options 1-8 only, Option 9 is skipped)
            for (int i = 1; i <= 8; i++)
            {
                _programTextBoxes[i - 1].Text = _config.GetValue("Programs", $"Option{i}");
            }

            // Load option 99
            if (_program99TextBox != null)
                _program99TextBox.Text = _config.GetValue("Programs", "Option99");

            // Load auto-launch setting from actual registry state (not INI)
            if (_autoLaunchCheckBox != null)
            {
                _autoLaunchCheckBox.Checked = IsInWindowsStartup();
            }

            // Load auto-start BBS settings
            if (_autoStartBBSCheckBox != null)
            {
                string autoStartValue = _config.GetValue("Settings", "AutoStartBBS", "false");
                _autoStartBBSCheckBox.Checked = autoStartValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            if (_autoStartDelayNumeric != null)
            {
                if (int.TryParse(_config.GetValue("Settings", "AutoStartDelay", "5"), out int delay))
                {
                    _autoStartDelayNumeric.Value = Math.Max(0, Math.Min(60, delay));
                }
            }

            if (_quietModeCheckBox != null)
            {
                string quietModeValue = _config.GetValue("Settings", "QuietMode", "false");
                _quietModeCheckBox.Checked = quietModeValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            if (_escToTrayCheckBox != null)
            {
                string escToTrayValue = _config.GetValue("Settings", "EscMinimizesToTray", "false");
                _escToTrayCheckBox.Checked = escToTrayValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            // Load F2 Module Editor path
            if (_moduleEditorTextBox != null)
            {
                string bbsPath = _config.GetValue("Paths", "BBSPath", @"C:\BBSV10");
                string moduleEditorPath = _config.GetValue("Programs", "ModuleEditor", "");

                // Default to WGSDMOD.exe in BBS path if not configured
                if (string.IsNullOrEmpty(moduleEditorPath))
                {
                    moduleEditorPath = System.IO.Path.Combine(bbsPath, "WGSDMOD.exe");
                }
                _moduleEditorTextBox.Text = moduleEditorPath;
            }
        }

        private void BrowseButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is TextBox textBox)
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Select BBS installation folder";
                    if (!string.IsNullOrEmpty(textBox.Text))
                        dialog.SelectedPath = textBox.Text;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = dialog.SelectedPath;
                    }
                }
            }
        }

        private void BrowseProgramButton_Click(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is TextBox textBox)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Executable Files (*.exe)|*.exe|All Files (*.*)|*.*";
                    dialog.Title = "Select Program";

                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        try
                        {
                            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(textBox.Text);
                            dialog.FileName = System.IO.Path.GetFileName(textBox.Text);
                        }
                        catch { }
                    }

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = dialog.FileName;
                    }
                }
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            // Save BBS path
            if (_bbsPathTextBox != null)
            {
                _config.SetValue("Paths", "BBSPath", _bbsPathTextBox.Text);
            }

            // Save programs (Options 1-8 only, Option 9 is skipped)
            // Names are fixed to match the background image, only paths are configurable
            for (int i = 1; i <= 8; i++)
            {
                _config.SetValue("Programs", $"Option{i}", _programTextBoxes[i - 1].Text);
            }

            // Save option 99
            if (_program99TextBox != null)
                _config.SetValue("Programs", "Option99", _program99TextBox.Text);

            // Save F2 Module Editor path
            if (_moduleEditorTextBox != null)
                _config.SetValue("Programs", "ModuleEditor", _moduleEditorTextBox.Text);

            // Save auto-start BBS settings
            if (_autoStartBBSCheckBox != null)
                _config.SetValue("Settings", "AutoStartBBS", _autoStartBBSCheckBox.Checked.ToString().ToLower());

            if (_autoStartDelayNumeric != null)
                _config.SetValue("Settings", "AutoStartDelay", ((int)_autoStartDelayNumeric.Value).ToString());

            if (_quietModeCheckBox != null)
                _config.SetValue("Settings", "QuietMode", _quietModeCheckBox.Checked.ToString().ToLower());

            if (_escToTrayCheckBox != null)
                _config.SetValue("Settings", "EscMinimizesToTray", _escToTrayCheckBox.Checked.ToString().ToLower());

            // Save auto-launch setting and update Windows startup
            if (_autoLaunchCheckBox != null)
            {
                bool autoLaunch = _autoLaunchCheckBox.Checked;
                _config.SetValue("Settings", "AutoLaunchAtStartup", autoLaunch.ToString().ToLower());

                try
                {
                    if (autoLaunch)
                    {
                        AddToWindowsStartup();
                    }
                    else
                    {
                        RemoveFromWindowsStartup();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not update Windows startup settings: {ex.Message}", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            _config.SaveConfig();

            MessageBox.Show("Configuration saved successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadApplicationIcon()
        {
            try
            {
                // Try to load icon from file system first
                string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icon.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                    return;
                }

                // Fallback: try to extract from embedded resources
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "MBBSLauncher.Resources.icon.ico";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        this.Icon = new Icon(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail - icon is optional
                Program.LogError("LoadApplicationIcon (ConfigEditor)", ex);
            }
        }

        private void AddToWindowsStartup()
        {
            try
            {
                using (Microsoft.Win32.RegistryKey? key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        // Get the actual executable path (works with single-file publish)
                        string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                        if (!string.IsNullOrEmpty(exePath))
                        {
                            key.SetValue("MBBS Launcher", $"\"{exePath}\"");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError("AddToWindowsStartup", ex);
                throw;
            }
        }

        private void RemoveFromWindowsStartup()
        {
            try
            {
                using (Microsoft.Win32.RegistryKey? key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        // Check if the value exists before trying to delete it
                        if (key.GetValue("MBBS Launcher") != null)
                        {
                            key.DeleteValue("MBBS Launcher", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError("RemoveFromWindowsStartup", ex);
                throw;
            }
        }

        private bool IsInWindowsStartup()
        {
            try
            {
                using (Microsoft.Win32.RegistryKey? key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    if (key != null)
                    {
                        return key.GetValue("MBBS Launcher") != null;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.LogError("IsInWindowsStartup", ex);
            }
            return false;
        }
    }
}
