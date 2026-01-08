// MBBS Launcher - Main Form
// Created by Mark Laudenbach with Love in Iowa
// https://github.com/laudenbachm/MBBS-Launcher
//
// File: Forms/MainForm.cs
// Version: v1.00
//
// Change History:
// 26.01.07.1 - 06:00PM - Initial creation
// 26.01.07.3 - 07:15PM - Added better error handling for startup

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MBBSLauncher.Forms
{
    public partial class MainForm : Form
    {
        private ConfigManager _config;
        private Image? _backgroundImage;
        private int _selectedOption = 5; // Default to option 5 (Go!)
        private System.Windows.Forms.Timer? _digitTimer;
        private string _digitBuffer = "";

        public MainForm()
        {
            try
            {
                InitializeComponent();
                InitializeCustomComponents();

                _config = new ConfigManager();

                // Check if wgserver is already running on startup
                try
                {
                    if (ProcessHelper.IsWGServerRunning())
                    {
                        var result = MessageBox.Show(
                            "WGServer is already running!\n\nWould you like to bring it to the foreground?",
                            "MBBS Launcher",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (result == DialogResult.Yes)
                        {
                            var process = ProcessHelper.GetProcess("wgserver");
                            if (process != null)
                            {
                                ProcessHelper.BringToForeground(process);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log but don't fail - process detection is optional
                    Program.LogError("WGServer detection", ex);
                }

                // Search for BBS folders on first run
                try
                {
                    if (string.IsNullOrEmpty(_config.GetValue("Paths", "BBSPath")) ||
                        !Directory.Exists(_config.GetValue("Paths", "BBSPath")))
                    {
                        _config.SearchForBBSFolders();
                        _config.SaveConfig();
                    }
                }
                catch (Exception ex)
                {
                    // Log but don't fail - folder search is optional
                    Program.LogError("BBS folder search", ex);
                }

                LoadBackgroundImage();
                LoadWindowSettings();
            }
            catch (Exception ex)
            {
                Program.LogError("MainForm constructor", ex);
                throw; // Re-throw to be caught by global handler
            }
        }

        private void InitializeCustomComponents()
        {
            this.Text = $"{Program.APP_NAME} {Program.APP_VERSION}. Created with Love \u2764 by {Program.AUTHOR} in Iowa";
            this.Size = new Size(960, 540); // 16:9 aspect ratio
            this.MinimumSize = new Size(640, 360); // Minimum 16:9
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.BackColor = Color.FromArgb(0, 0, 170); // Classic DOS blue
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true); // Auto-invalidate on resize

            // Load application icon
            LoadApplicationIcon();

            // Initialize digit input timer
            _digitTimer = new System.Windows.Forms.Timer();
            _digitTimer.Interval = 1000; // 1 second timeout for multi-digit input
            _digitTimer.Tick += DigitTimer_Tick;

            // Handle keyboard input
            this.KeyDown += MainForm_KeyDown;
            this.Paint += MainForm_Paint;
            this.Resize += MainForm_Resize;
            this.FormClosing += MainForm_FormClosing;
            this.Move += MainForm_Move;
            this.MouseClick += MainForm_MouseClick;
            this.VisibleChanged += MainForm_VisibleChanged;
            this.Activated += MainForm_Activated;
            this.Shown += MainForm_Shown;
        }

        private void LoadBackgroundImage()
        {
            try
            {
                // Try to load from embedded resources first
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "MBBSLauncher.Resources.background.png";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        _backgroundImage = Image.FromStream(stream);
                        return;
                    }
                }

                // Fallback to file system if embedded resource not found
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "background.png");
                if (File.Exists(imagePath))
                {
                    _backgroundImage = Image.FromFile(imagePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading background image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadApplicationIcon()
        {
            try
            {
                // Try to load icon from file system first
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "icon.ico");
                if (File.Exists(iconPath))
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
                Program.LogError("LoadApplicationIcon", ex);
            }
        }

        private void LoadWindowSettings()
        {
            try
            {
                // Load window size
                string widthStr = _config.GetValue("Window", "Width");
                string heightStr = _config.GetValue("Window", "Height");

                if (int.TryParse(widthStr, out int width) && int.TryParse(heightStr, out int height))
                {
                    if (width >= this.MinimumSize.Width && height >= this.MinimumSize.Height)
                    {
                        this.Size = new Size(width, height);
                    }
                }

                // Load window position
                string xStr = _config.GetValue("Window", "X");
                string yStr = _config.GetValue("Window", "Y");

                if (int.TryParse(xStr, out int x) && int.TryParse(yStr, out int y))
                {
                    // Ensure the window is visible on screen
                    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                    if (x >= workingArea.Left && x < workingArea.Right - 100 &&
                        y >= workingArea.Top && y < workingArea.Bottom - 100)
                    {
                        this.StartPosition = FormStartPosition.Manual;
                        this.Location = new Point(x, y);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail - window settings are optional
                Program.LogError("LoadWindowSettings", ex);
            }
        }

        private void SaveWindowSettings()
        {
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    _config.SetValue("Window", "X", this.Location.X.ToString());
                    _config.SetValue("Window", "Y", this.Location.Y.ToString());
                    _config.SetValue("Window", "Width", this.Size.Width.ToString());
                    _config.SetValue("Window", "Height", this.Size.Height.ToString());
                    _config.SaveConfig();
                }
            }
            catch (Exception ex)
            {
                // Log but don't fail - window settings are optional
                Program.LogError("SaveWindowSettings", ex);
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveWindowSettings();
        }

        private void MainForm_Move(object? sender, EventArgs e)
        {
            // Save position when moved (debounced by only saving on close)
        }

        private void MainForm_VisibleChanged(object? sender, EventArgs e)
        {
            // Force repaint when visibility changes
            if (this.Visible)
            {
                this.Invalidate();
                this.Refresh();
            }
        }

        private void MainForm_Activated(object? sender, EventArgs e)
        {
            // Force repaint when window is activated
            this.Invalidate();
        }

        private void MainForm_Shown(object? sender, EventArgs e)
        {
            // Force repaint when window is first shown
            this.Invalidate();
            this.Refresh();
        }

        private void MainForm_Paint(object? sender, PaintEventArgs e)
        {
            if (_backgroundImage != null)
            {
                // Draw background image scaled to fit window while maintaining aspect ratio
                e.Graphics.DrawImage(_backgroundImage, 0, 0, this.ClientSize.Width, this.ClientSize.Height);
            }
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            // Maintain 16:9 aspect ratio
            int targetWidth = this.Width;
            int targetHeight = (int)(targetWidth / 16.0 * 9.0);

            if (this.Height != targetHeight)
            {
                this.Height = targetHeight;
            }

            this.Invalidate(); // Redraw on resize
        }

        private void MainForm_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // Get click position relative to client area
            float x = e.X / (float)this.ClientSize.Width;
            float y = e.Y / (float)this.ClientSize.Height;

            // Define clickable regions based on background image (1440x810 reference)
            // Left column: Options 1, 2, 3, 4
            if (x >= 0.038f && x <= 0.101f) // x: 54-145 pixels
            {
                if (y >= 0.389f && y <= 0.469f) LaunchOption(1); // Option 1
                else if (y >= 0.519f && y <= 0.599f) LaunchOption(2); // Option 2
                else if (y >= 0.648f && y <= 0.728f) LaunchOption(3); // Option 3
                else if (y >= 0.778f && y <= 0.858f) LaunchOption(4); // Option 4
            }
            // Center column: Options 5, 0
            else if (x >= 0.431f && x <= 0.498f) // x: 621-717 pixels
            {
                if (y >= 0.476f && y <= 0.537f) LaunchOption(5); // Option 5 (Go!)
                else if (y >= 0.667f && y <= 0.728f) LaunchOption(0); // Option 0 (Exit)
            }
            // Right column: Options 6, 7, 8, 99
            else if (x >= 0.687f && x <= 0.750f) // x: 989-1080 pixels
            {
                if (y >= 0.389f && y <= 0.469f) LaunchOption(6); // Option 6
                else if (y >= 0.519f && y <= 0.599f) LaunchOption(7); // Option 7
                else if (y >= 0.648f && y <= 0.728f) LaunchOption(8); // Option 8
                else if (y >= 0.778f && y <= 0.858f) LaunchOption(99); // Option 99
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // F12 opens configuration editor
            if (e.KeyCode == Keys.F12)
            {
                OpenConfigEditor();
                e.Handled = true;
                return;
            }

            // Number keys 0-9 (handle multi-digit for option 99)
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                int digit = e.KeyCode - Keys.D0;
                HandleDigitInput(digit);
                e.Handled = true;
                return;
            }

            // NumPad keys 0-9
            if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                int digit = e.KeyCode - Keys.NumPad0;
                HandleDigitInput(digit);
                e.Handled = true;
                return;
            }

            // Arrow keys for future mouse selection feature
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                // TODO: Implement arrow key selection in future version
                e.Handled = true;
            }

            // Enter key launches selected option
            if (e.KeyCode == Keys.Enter)
            {
                LaunchOption(_selectedOption);
                e.Handled = true;
            }

            // Escape key exits
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.Handled = true;
            }
        }

        private void HandleDigitInput(int digit)
        {
            // Add digit to buffer
            _digitBuffer += digit.ToString();

            // Restart timer
            _digitTimer?.Stop();
            _digitTimer?.Start();

            // Check if we have a complete option number
            // Option 0 is always immediate (exit)
            if (digit == 0 && _digitBuffer == "0")
            {
                _digitTimer?.Stop();
                _digitBuffer = "";
                LaunchOption(0);
                return;
            }

            // Check for option 99
            if (_digitBuffer == "99")
            {
                _digitTimer?.Stop();
                _digitBuffer = "";
                LaunchOption(99);
                return;
            }

            // Single digit options 1-9 with delay to allow for 99
            if (_digitBuffer.Length == 1 && digit >= 1 && digit <= 9)
            {
                // Wait for potential second digit
                return;
            }

            // If buffer gets too long or invalid, reset
            if (_digitBuffer.Length > 2)
            {
                _digitTimer?.Stop();
                _digitBuffer = "";
            }
        }

        private void DigitTimer_Tick(object? sender, EventArgs e)
        {
            _digitTimer?.Stop();

            // Process single digit if we have one
            if (_digitBuffer.Length == 1 && int.TryParse(_digitBuffer, out int option))
            {
                _digitBuffer = "";
                LaunchOption(option);
            }
            else
            {
                _digitBuffer = "";
            }
        }

        private void LaunchOption(int optionNumber)
        {
            // Option 0 is Exit
            if (optionNumber == 0)
            {
                this.Close();
                return;
            }

            // Get program path from config
            string programCommand = _config.GetValue("Programs", $"Option{optionNumber}");
            string programName = _config.GetValue("Programs", $"Option{optionNumber}Name", $"Option {optionNumber}");

            if (string.IsNullOrWhiteSpace(programCommand))
            {
                MessageBox.Show(
                    $"{programName} is not configured.\n\nPress F12 to configure programs.",
                    "Not Configured",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // Parse program path and arguments
            string programPath;
            string? arguments = null;

            // Check if there are arguments (look for space after .exe)
            int exeIndex = programCommand.IndexOf(".exe", StringComparison.OrdinalIgnoreCase);
            if (exeIndex > 0 && exeIndex + 4 < programCommand.Length)
            {
                programPath = programCommand.Substring(0, exeIndex + 4).Trim();
                arguments = programCommand.Substring(exeIndex + 4).Trim();
            }
            else
            {
                programPath = programCommand.Trim();
            }

            if (!File.Exists(programPath))
            {
                MessageBox.Show(
                    $"Program not found:\n{programPath}\n\nPress F12 to update configuration.",
                    "File Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Get process name from exe path
            string processName = Path.GetFileNameWithoutExtension(programPath);

            // Special handling for wgsappgo which spawns wgserver
            string monitorProcess = processName;
            if (processName.Equals("wgsappgo", StringComparison.OrdinalIgnoreCase))
            {
                monitorProcess = "wgserver";
            }

            // Check if process is already running
            if (ProcessHelper.IsProcessRunning(monitorProcess))
            {
                var result = MessageBox.Show(
                    $"{programName} is already running!\n\nWould you like to bring it to the foreground?",
                    "Already Running",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    var process = ProcessHelper.GetProcess(monitorProcess);
                    if (process != null)
                    {
                        ProcessHelper.BringToForeground(process);
                    }
                }
                return;
            }

            // Launch the program
            string? workingDir = Path.GetDirectoryName(programPath);
            var launchedProcess = ProcessHelper.LaunchProgram(programPath, workingDir, arguments);

            if (launchedProcess != null)
            {
                // Hide the launcher window
                this.Hide();

                // Wait for the process to exit, then show the launcher again
                System.Threading.Tasks.Task.Run(() =>
                {
                    // For wgsappgo (Option 5), monitor both wgsappgo and wgserver
                    if (processName.Equals("wgsappgo", StringComparison.OrdinalIgnoreCase))
                    {
                        // Wait for wgsappgo to exit
                        launchedProcess.WaitForExit();

                        // Then wait for wgserver to exit (if it's running)
                        System.Threading.Thread.Sleep(500); // Brief delay to detect wgserver

                        while (ProcessHelper.IsProcessRunning("wgserver"))
                        {
                            var serverProcess = ProcessHelper.GetProcess("wgserver");
                            if (serverProcess != null)
                            {
                                serverProcess.WaitForExit();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        launchedProcess.WaitForExit();
                    }

                    // Show launcher again
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Show();
                        this.Activate();
                        this.BringToFront();
                        this.Invalidate();
                        this.Refresh();
                    });
                });
            }
        }

        private void OpenConfigEditor()
        {
            using (var configEditor = new ConfigEditorForm(_config))
            {
                configEditor.ShowDialog(this);
            }

            // Reload config after editing
            _config.LoadConfig();
        }

        // Note: Dispose method is in MainForm.Designer.cs
    }
}
