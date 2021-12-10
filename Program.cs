using System;
using System.Threading;
using System.Windows.Forms;

namespace WinKaf;

static class Program
{

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        const string appName = "WinKaf";

        _ = new Mutex(true, appName, out bool createdNew);

        if (!createdNew)
        {
            //app is already running! Exiting the application  
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new WinKaf(args));
    }
}

