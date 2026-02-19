// MBBS Launcher - App Manager Form Designer
// Created by Mark Laudenbach with Love in Iowa
// https://github.com/laudenbachm/MBBS-Launcher
//
// File: Forms/AppManagerForm.Designer.cs
// Version: v1.6 Beta
//
// Change History:
// 26.02.11.1 - Initial creation for v1.6 Beta

#nullable enable

using System.Drawing;
using System.Windows.Forms;

namespace MBBSLauncher.Forms
{
    partial class AppManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel titleBarPanel;
        private Label titleLabel;
        private Button closeButton;
        private CheckBox alwaysOnTopCheckbox;
        private CheckBox autoHideCheckbox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.titleBarPanel = new Panel();
            this.titleLabel = new Label();
            this.closeButton = new Button();
            this.alwaysOnTopCheckbox = new CheckBox();
            this.autoHideCheckbox = new CheckBox();

            this.SuspendLayout();

            //
            // titleBarPanel
            //
            this.titleBarPanel.BackColor = Color.FromArgb(0, 0, 128);
            this.titleBarPanel.Location = new Point(0, 0);
            this.titleBarPanel.Name = "titleBarPanel";
            this.titleBarPanel.Size = new Size(280, 30);
            this.titleBarPanel.TabIndex = 0;
            this.titleBarPanel.MouseDown += (s, e) => this.OnMouseDown(e);
            this.titleBarPanel.Cursor = Cursors.SizeAll;

            //
            // titleLabel
            //
            this.titleLabel.AutoSize = false;
            this.titleLabel.BackColor = Color.Transparent;
            this.titleLabel.Font = new Font("Consolas", 10F, FontStyle.Bold);
            this.titleLabel.ForeColor = Color.Cyan;
            this.titleLabel.Location = new Point(10, 6);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new Size(200, 18);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "MBBS App Manager (Beta)";
            this.titleLabel.MouseDown += (s, e) => this.OnMouseDown(e);
            this.titleLabel.Cursor = Cursors.SizeAll;

            //
            // closeButton
            //
            this.closeButton.BackColor = Color.FromArgb(0, 0, 128);
            this.closeButton.FlatStyle = FlatStyle.Flat;
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.Font = new Font("Consolas", 10F, FontStyle.Bold);
            this.closeButton.ForeColor = Color.White;
            this.closeButton.Location = new Point(250, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new Size(25, 24);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "×";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += CloseButton_Click;
            this.closeButton.FlatAppearance.MouseOverBackColor = Color.Red;
            this.closeButton.Cursor = Cursors.Hand;

            //
            // alwaysOnTopCheckbox
            //
            this.alwaysOnTopCheckbox.AutoSize = true;
            this.alwaysOnTopCheckbox.BackColor = Color.Transparent;
            this.alwaysOnTopCheckbox.Font = new Font("Consolas", 8F);
            this.alwaysOnTopCheckbox.ForeColor = Color.White;
            this.alwaysOnTopCheckbox.Location = new Point(10, 210);
            this.alwaysOnTopCheckbox.Name = "alwaysOnTopCheckbox";
            this.alwaysOnTopCheckbox.Size = new Size(110, 17);
            this.alwaysOnTopCheckbox.TabIndex = 2;
            this.alwaysOnTopCheckbox.Text = "Always on top";
            this.alwaysOnTopCheckbox.UseVisualStyleBackColor = false;
            this.alwaysOnTopCheckbox.CheckedChanged += AlwaysOnTopCheckbox_CheckedChanged;
            this.alwaysOnTopCheckbox.Checked = true;

            //
            // autoHideCheckbox
            //
            this.autoHideCheckbox.AutoSize = true;
            this.autoHideCheckbox.BackColor = Color.Transparent;
            this.autoHideCheckbox.Font = new Font("Consolas", 8F);
            this.autoHideCheckbox.ForeColor = Color.White;
            this.autoHideCheckbox.Location = new Point(130, 210);
            this.autoHideCheckbox.Name = "autoHideCheckbox";
            this.autoHideCheckbox.Size = new Size(140, 17);
            this.autoHideCheckbox.TabIndex = 3;
            this.autoHideCheckbox.Text = "Auto-hide when done";
            this.autoHideCheckbox.UseVisualStyleBackColor = false;
            this.autoHideCheckbox.CheckedChanged += AutoHideCheckbox_CheckedChanged;
            this.autoHideCheckbox.Checked = false;

            //
            // AppManagerForm
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(0, 0, 128);
            this.ClientSize = new Size(280, 235);
            this.Controls.Add(this.autoHideCheckbox);
            this.Controls.Add(this.alwaysOnTopCheckbox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleBarPanel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "AppManagerForm";
            this.Opacity = 0.95;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "MBBS App Manager";
            this.TopMost = true;
            this.ShowInTaskbar = false;

            // Add controls to title bar panel
            this.titleBarPanel.Controls.Add(this.titleLabel);
            this.titleBarPanel.Controls.Add(this.closeButton);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CloseButton_Click(object? sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void AlwaysOnTopCheckbox_CheckedChanged(object? sender, System.EventArgs e)
        {
            _alwaysOnTop = alwaysOnTopCheckbox.Checked;
            this.TopMost = _alwaysOnTop;
            SaveSettings();
        }

        private void AutoHideCheckbox_CheckedChanged(object? sender, System.EventArgs e)
        {
            _autoHide = autoHideCheckbox.Checked;
            SaveSettings();
        }
    }
}
