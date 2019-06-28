using System;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;


namespace Overwolf.Other
{
    internal class Extensions
    {
        public static DateTime AssemblyCreationDate()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            return new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.MinorRevision * 2);
        }

        public static void Information(string text, bool newLine)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void Error(string text, int sleep, bool closeProc)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(text);

            Thread.Sleep(sleep);

            if (closeProc) Environment.Exit(0);
        }

        internal class Proc
        {
            public static bool IsWindowFocues(Process procName)
            {
                IntPtr activatedHandle = Globals.Imports.GetForegroundWindow();

                if (activatedHandle == IntPtr.Zero) return false;

                Globals.Imports.GetWindowThreadProcessId(activatedHandle, out int activeProcId);

                return activeProcId == procName.Id;
            }

            public static Process WaitForProcess(string procName)
            {
                Process[] process = Process.GetProcessesByName(procName);

                Information($"> waiting for { procName } to show up", false);

                while (process.Length < 1)
                {
                    Information(".", false);

                    process = Process.GetProcessesByName(procName);

                    Thread.Sleep(250);
                }

                Console.Clear();

                return process[0];
            }
        }
    }
}
