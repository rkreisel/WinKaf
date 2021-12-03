
namespace WinKaf
{
    partial class WinKaf
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinKaf));
            this.lblCountdown = new System.Windows.Forms.Label();
            this.chkUseMouseMode = new System.Windows.Forms.CheckBox();
            this.tt = new System.Windows.Forms.ToolTip(this.components);
            this.btnViewLog = new System.Windows.Forms.Button();
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitWinKafToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmTrayIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCountdown
            // 
            this.lblCountdown.AutoSize = true;
            this.lblCountdown.Location = new System.Drawing.Point(88, 106);
            this.lblCountdown.Name = "lblCountdown";
            this.lblCountdown.Size = new System.Drawing.Size(38, 15);
            this.lblCountdown.TabIndex = 0;
            this.lblCountdown.Text = "label1";
            this.lblCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkUseMouseMode
            // 
            this.chkUseMouseMode.AutoSize = true;
            this.chkUseMouseMode.Location = new System.Drawing.Point(6, 5);
            this.chkUseMouseMode.Name = "chkUseMouseMode";
            this.chkUseMouseMode.Size = new System.Drawing.Size(118, 19);
            this.chkUseMouseMode.TabIndex = 1;
            this.chkUseMouseMode.Text = "Use Mouse Mode";
            this.tt.SetToolTip(this.chkUseMouseMode, "Temporarily toggles Mouose Mode (moves the mouse 1 pixel isntead of call ing the " +
        "Windows Internals to stop sleep. (To make permanent, add /m to the command line)" +
        "");
            this.chkUseMouseMode.UseVisualStyleBackColor = true;
            this.chkUseMouseMode.Click += new System.EventHandler(this.chkUseMouseMode_Click);
            // 
            // btnViewLog
            // 
            this.btnViewLog.Location = new System.Drawing.Point(6, 21);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(75, 23);
            this.btnViewLog.TabIndex = 2;
            this.btnViewLog.Text = "View Log";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Visible = false;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // bw
            // 
            this.bw.WorkerReportsProgress = true;
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DoWork);
            this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ProgressChanged);
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.BalloonTipText = "Manage WinKaf";
            this.trayIcon.BalloonTipTitle = "WinKaf";
            this.trayIcon.ContextMenuStrip = this.cmTrayIcon;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "WinKaf";
            this.trayIcon.Visible = true;
            this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseDoubleClick);
            // 
            // cmTrayIcon
            // 
            this.cmTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.viewLogToolStripMenuItem,
            this.exitWinKafToolStripMenuItem});
            this.cmTrayIcon.Name = "cmTrayIcon";
            this.cmTrayIcon.Size = new System.Drawing.Size(181, 92);
            this.cmTrayIcon.Opening += new System.ComponentModel.CancelEventHandler(this.cmTrayIcon_Opening);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showToolStripMenuItem.Text = "Show WinKaf";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // viewLogToolStripMenuItem
            // 
            this.viewLogToolStripMenuItem.Name = "viewLogToolStripMenuItem";
            this.viewLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewLogToolStripMenuItem.Text = "View Log";
            this.viewLogToolStripMenuItem.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // exitWinKafToolStripMenuItem
            // 
            this.exitWinKafToolStripMenuItem.Name = "exitWinKafToolStripMenuItem";
            this.exitWinKafToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitWinKafToolStripMenuItem.Text = "Exit WinKaf";
            this.exitWinKafToolStripMenuItem.Click += new System.EventHandler(this.exitWinKafToolStripMenuItem_Click);
            // 
            // WinKaf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(210, 177);
            this.Controls.Add(this.btnViewLog);
            this.Controls.Add(this.chkUseMouseMode);
            this.Controls.Add(this.lblCountdown);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WinKaf";
            this.Text = "WK";
            this.Resize += new System.EventHandler(this.WinKaf_Resize);
            this.cmTrayIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.CheckBox chkUseMouseMode;
        private System.Windows.Forms.ToolTip tt;
        private System.Windows.Forms.Button btnViewLog;
        private System.ComponentModel.BackgroundWorker bw;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip cmTrayIcon;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitWinKafToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogToolStripMenuItem;
    }
}

