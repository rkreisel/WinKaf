using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WinKaf;

namespace Caffiene
{
    public partial class WinKaf : Form
    {
        #region Externals
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

        public WinKaf(string[] args)
        {
            _argList = ParseArgs(args);
            if (_argList.Count > 0)
            {
                if (_argList.ContainsKey("/b"))
                {
                    int.TryParse(_argList["/b"], out breakSeconds);
                }
                if (_argList.ContainsKey("/d"))
                {
                    AllocConsole();
                }
                if (_argList.ContainsKey("/h"))
                {
                    MessageBox.Show($"Help{Environment.NewLine}/b - seconds to wait before breaking the timer loop, default 300 (5 minutes){Environment.NewLine}/d - debug mode (opens command window with consoel output{Environment.NewLine}/h - This help screen", "WinKaf command line options", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            InitializeComponent();
            UpdateText(breakSeconds.ToString(), lblCountdown);
            BackgroundLoopManager(loopCycle);
        }

        private IDictionary<string,string> ParseArgs(string[] args)
        {
            var argList = new Dictionary<string, string>();
            foreach(var arg in args)
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

        #region Looping code
        private void BackgroundLoopManager(int seconds)
        {
            Thread looper = new Thread(new ThreadStart(BackgroundLoop))
            {
                IsBackground = true,
                Name = "TimerThread"
            };
            loopCycle = seconds;
            looper.Start();
        }

        private void BackgroundLoop()
        {
            var recheckTime = DateTime.Now.AddSeconds(breakSeconds);
            while (true)
            {

                var waittime = recheckTime - DateTime.Now;
                UpdateText(((int)waittime.TotalSeconds).ToString(), lblCountdown);

                if (DateTime.Now >= recheckTime)
                {
                    Console.WriteLine($"Timer hit: {recheckTime.ToString()}");
                    UpdateText(breakSeconds.ToString(), lblCountdown);
                    Thread.Sleep(loopCycle * 1000);
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
                    recheckTime = DateTime.Now.AddSeconds(breakSeconds);
                }
                Thread.Sleep(loopCycle * 1000);
            }
        }

        private void UpdateText<T>(string newText, T txtObj)
        {
            if (InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action<string, T>(UpdateText), new object[] { newText, txtObj });
                    return;
                }
                catch { } // don't do anything, because this only fails during shutdown anyway
            }
            if (txtObj.HasProperty("Text"))
            {
                (txtObj as dynamic).Text = newText;
            }
            else
            {
                Console.WriteLine($"UpdateText failed because it could not find a Text property in the object passed to it. {txtObj}");
            }
        }
        #endregion        
    }
}
