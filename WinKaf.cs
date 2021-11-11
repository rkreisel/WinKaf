using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinKaf;

namespace WinKaf
{
    public partial class WinKaf : Form
    {
        #region Externals
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

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
        private int loopCycle = 1;
        private int breakSeconds = 300;
        private IDictionary<string, string> _argList;
        private bool mouseMode = false;
        private bool quietMode = false;
        private bool loggingEnabled = false;
        private string logFileName = string.Empty;
        private const string dateFormat = "yyyyMMddHHmmss";
        private bool debugEnabled = false;

        public WinKaf(string[] args)
        {
            var startupMessages = new List<string>();

            _argList = ParseArgs(args);
            if (_argList.Count > 0)
            {
                if (_argList.ContainsKey("/b"))
                {
                    int.TryParse(_argList["/b"], out breakSeconds);
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
                    logFileName = $"{_argList["/l"]}";
                    if (string.IsNullOrWhiteSpace(logFileName))
                        logFileName = $"WinKafLog-{DateTime.Now.ToString(dateFormat)}.txt";

                    loggingEnabled = true;
                    startupMessages.Add($"Logging enabled. Logging to {logFileName}.");
                }
                if (_argList.ContainsKey("/h"))
                {
                    MessageBox.Show($"Help{Environment.NewLine}/b - seconds to wait before breaking the timer loop, default 300 (5 minutes){Environment.NewLine}/d - debug mode (opens command window with consoel output{Environment.NewLine}/m - Use 'Mouse move' to active instead of relying on the call to the internal 'stay awake' method{Environment.NewLine}/l[=filename] - enable logging{Environment.NewLine}/q - quiet mode, no messages/h - This help screen", "WinKaf command line options", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            InitializeComponent();
            if (loggingEnabled)
                btnViewLog.Visible = true;
            //UpdateText(breakSeconds.ToString(), lblCountdown);
            lblCountdown.Text = breakSeconds.ToString();
            var msg = string.Join($"{Environment.NewLine}", startupMessages.ToArray());
            LogIt(msg);
            if (startupMessages.Any() && !quietMode)
            {
                MessageBox.Show(msg, "WinKaf Startup Overrides", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            chkUseMouseMode.Checked = mouseMode;
            bw.RunWorkerAsync();
        }

        private void LogIt(string message)
        {
            if (debugEnabled)
                Console.WriteLine($"{DateTime.Now.ToString(dateFormat)} - {message}");
            if (loggingEnabled)
            {
                using StreamWriter file = new(logFileName, append: true);
                file.WriteLine($"{DateTime.Now.ToString(dateFormat)} - {message}");
            }
        }
        private IDictionary<string, string> ParseArgs(string[] args)
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
        }

        private void btnViewLog_Click(object sender, EventArgs e)
        {
            var info = new ProcessStartInfo("explorer.exe");
            info.Arguments = $"\"{logFileName}\"";
            Process.Start(info);
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
                    LogIt($"Timer hit: {recheckTime.ToString()}");
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
    }
}
