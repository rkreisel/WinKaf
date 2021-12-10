using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WinKaf
{
    public partial class WinKaf : Form
    {
        #region Externals
        [DllImport("User32.Dll")]
        private static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        #endregion

        static extern bool AllocConsole();
        private readonly int loopCycle = 1;
        private readonly int breakSeconds = 300;
        private readonly IDictionary<string, string> _argList;
        private bool mouseMode = false;
        private readonly bool quietMode = false;
        private readonly bool loggingEnabled = false;
        private readonly string logFileName = string.Empty;
        private const string dateFormat = "yyyyMMdd";
        private readonly bool debugEnabled = false;
        private int _maxLogFiles = 30;
        #nullable enable
        private Viewer? viewer;
        #nullable disable

        public WinKaf(string[] args)
        {
            var startupMessages = new List<string>();

            _argList = ParseArgs(args);
            if (_argList.Count > 0)
            {
                if (_argList.ContainsKey("/b"))
                {
_ =                    int.TryParse(_argList["/b"], out breakSeconds);
                    startupMessages.Add($"Default timeout (300 seconds) overridden to {breakSeconds}");
                }
                if (_argList.ContainsKey("/d"))
                {
                    debugEnabled = true;
                    AllocConsole();
                    startupMessages.Add("Debug mode enabled. See console for messages.");
                }
                if (_argList.ContainsKey("/m"))
                {
                    mouseMode = true;
                    startupMessages.Add("Mouse mode enabled");
                }
                if (_argList.ContainsKey("/q"))
                {
                    quietMode = true;
                    startupMessages.Add("Quiet mode enabled. (no message box messages)");
                }
                if (_argList.ContainsKey("/l"))
                {
                    logFileName = $"WinKafLog-{DateTime.Now.ToString(dateFormat)}.txt";
                    loggingEnabled = true;
                    startupMessages.Add($"Logging enabled. Logging to {logFileName}.");
                    startupMessages.AddRange(CleanupLogs());
                }
                if (_argList.ContainsKey("/h"))
                {
                    MessageBox.Show($"/b - seconds to wait before breaking the timer loop, default 300 (5 minutes){Environment.NewLine}{Environment.NewLine}/d - debug mode (opens command window with console output{Environment.NewLine}{Environment.NewLine}/m - Use 'Mouse move' to active instead of relying on the call to the internal 'stay awake' method{Environment.NewLine}{Environment.NewLine}/l [# of days of log files to keep  (default = 30)] - enable logging{Environment.NewLine}{Environment.NewLine}/q - quiet mode, no messages{Environment.NewLine}{Environment.NewLine}/h - This help screen{Environment.NewLine}{Environment.NewLine}Example:{Environment.NewLine}{Environment.NewLine}WinKaf /b=5 /d /l=5 /q{Environment.NewLine}    break (check) every 5 seconds{Environment.NewLine}    show debug window{Environment.NewLine}    enable logging and keep a max of 5 files{Environment.NewLine}    quiet mode", "WinKaf command line options", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            InitializeComponent();
            if (loggingEnabled)
                btnViewLog.Visible = true;
            lblCountdown.Text = breakSeconds.ToString();
            var msg = string.Join($"{Environment.NewLine}", startupMessages.ToArray());
            ReportIt(msg);
            if (startupMessages.Any() && !quietMode)
            {
                MessageBox.Show(msg, "WinKaf Startup Overrides", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            chkUseMouseMode.Checked = mouseMode;
            bw.RunWorkerAsync();
        }

        private void ReportIt(string message)
        {
            var pointInTime = DateTime.Now;
            var timestamp = $"{pointInTime.ToString(dateFormat)}-{pointInTime:HHmmss.fff}";
            if (debugEnabled)
                Console.WriteLine($"{timestamp} - {message}");
            if (loggingEnabled)
            {
                using StreamWriter file = new(logFileName, append: true);
                file.WriteLine($"{timestamp} - {message}");
            }
        }

        private List<string> CleanupLogs()
        {
            var srch = $"WinKafLog-*.txt";
            var result = new List<string>();

            if (_argList.ContainsKey("/l"))
            {
                if (int.TryParse(_argList["/l"], out int nbrOfLogFiles))
                {
                    _maxLogFiles = nbrOfLogFiles;
                }
            }
            var logFileList = Directory.GetFiles(Directory.GetCurrentDirectory(), srch).ToList().OrderByDescending(fn => fn);
            if (logFileList.Count() > _maxLogFiles)
            {
                foreach (var fn in logFileList.Skip(_maxLogFiles))
                {
                    File.Delete(fn);
                    result.Add($"Deleted {fn}");
                }
            }
            return result;
        }

        private static IDictionary<string, string> ParseArgs(string[] args)
        {
            var argList = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                var parts = arg.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 1)
                {
                    argList.Add(parts[0], string.Empty);
                }
                else
                {
                    argList.Add(parts[0], parts[1]);
                }
            }
            return argList;
        }

        private void MoveMouse()
        {
            //move the mouse 1 pixel then back
            var pt = new POINT();
            pt.x++;
            ClientToScreen(this.Handle, ref pt);
            pt.x--;
            ClientToScreen(this.Handle, ref pt);
        }

        private void chkUseMouseMode_Click(object sender, EventArgs e)
        {
            mouseMode = ((CheckBox)sender).Checked;
            ReportIt($"Switching mouse mode: {mouseMode}");
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            if (loggingEnabled)
            {
                ReportIt("Displaying log file contents.");
                if (this.viewer == null || this.viewer.IsDisposed)
                {
                    this.viewer = new Viewer();
                    viewer.Text = $"Ipsum Log Viewer - {logFileName}";
                }
                viewer.LoadText(File.ReadAllText(logFileName));
                viewer.Show();
            }
            else
            {
                ReportIt("Cannot show log file. Logging is not enabled.");
                MessageBox.Show("Logging not enabled. You can enable it with the '/l' command line argument.", "WinKaf - Argument Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                if (mouseMode)
                {
                    MoveMouse();
                }
                else
                    SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            }
            else
                lblCountdown.Text = e.ProgressPercentage.ToString();
        }

        private void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var recheckTime = DateTime.Now.AddSeconds(breakSeconds);
            while (true)
            {

                var waittime = recheckTime - DateTime.Now;
                bw.ReportProgress((int)waittime.TotalSeconds);

                if (DateTime.Now >= recheckTime)
                {
                    ReportIt($"Timer hit: {recheckTime}");
                    bw.ReportProgress(breakSeconds);
                    Thread.Sleep(loopCycle * 1000);
                    bw.ReportProgress(-1);
                    recheckTime = DateTime.Now.AddSeconds(breakSeconds);
                }
                else
                {
                    Thread.Sleep(loopCycle * 1000);
                }
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void WinKaf_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e) =>
            trayIcon_MouseDoubleClick(sender, null);

        private void exitWinKafToolStripMenuItem_Click(object sender, EventArgs e) =>
            Application.ExitThread();

        private void cmTrayIcon_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!loggingEnabled)
                viewLogToolStripMenuItem.Visible = false;
        }
    }
}
