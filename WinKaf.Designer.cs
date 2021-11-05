
namespace Caffiene
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
            this.chkUseMouseMode.Location = new System.Drawing.Point(13, 13);
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
            // WinKaf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(210, 177);
            this.Controls.Add(this.chkUseMouseMode);
            this.Controls.Add(this.lblCountdown);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "WinKaf";
            this.Text = "WinKaf";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.CheckBox chkUseMouseMode;
        private System.Windows.Forms.ToolTip tt;
    }
}

