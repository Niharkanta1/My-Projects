using System;
using System.Windows.Forms;

namespace SupremeMemCleaner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogControl.Info("Starting Supreme Memory Cleaner: Main");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SupremeCleaner());
        }
    }
}
