// MBBS Launcher - Program Entry Point
// Created by Mark Laudenbach with Love in Iowa
// https://github.com/laudenbachm/MBBS-Launcher
//
// File: Program.cs
// Version: v1.00
//
// Change History:
// 26.01.07.1 - 06:00PM - Initial creation
// 26.01.07.3 - 07:15PM - Added global exception handling and error logging

using System;
using System.IO;
using System.Windows.Forms;
using MBBSLauncher.Forms;

namespace MBBSLauncher
{
    internal static class Program
    {
        public const string APP_VERSION = "v1.00";
        public const string APP_NAME = "MBBS Launcher";
        public const string AUTHOR = "Mark Laudenbach";
        public const string TAGLINE = "Created with Love in Iowa";
        public const string GITHUB_URL = "https://github.com/laudenbachm/MBBS-Launcher";

        // URLs displayed on the launcher screen
        public const string WEBSITE_URL = "https://themajorbbs.com";
        public const string DEMO_BBS_URL = "telnet://bbs.themajorbbs.com";
        public const string DISCORD_URL = "https://discord.gg/VhRk9xpq30";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Add global exception handlers
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                LogError("Main", ex);
                MessageBox.Show(
                    $"A fatal error occurred:\n\n{ex.Message}\n\n{ex.StackTrace}\n\nCheck error.log for details.",
                    "MBBS Launcher - Fatal Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogError("ThreadException", e.Exception);
            MessageBox.Show(
                $"An error occurred:\n\n{e.Exception.Message}\n\nCheck error.log for details.",
                "MBBS Launcher - Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogError("UnhandledException", ex);
                MessageBox.Show(
                    $"An unhandled error occurred:\n\n{ex.Message}\n\nCheck error.log for details.",
                    "MBBS Launcher - Unhandled Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public static void LogError(string context, Exception ex)
        {
            try
            {
                string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {context}\n" +
                                  $"Exception: {ex.GetType().Name}\n" +
                                  $"Message: {ex.Message}\n" +
                                  $"Stack Trace:\n{ex.StackTrace}\n" +
                                  $"----------------------------------------\n\n";
                File.AppendAllText(logFile, logMessage);
            }
            catch
            {
                // If we can't log, at least we tried
            }
        }
    }
}
