﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Overwolf.Other
{
    class Globals
    {
        internal class Imports
        {
            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

            [DllImport("User32.dll")]
            public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

            [DllImport("User32.dll")]
            public static extern short GetAsyncKeyState(System.Int32 vKey);

            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern long GetAsyncKeyState(long vKey);
        }

        internal class Proc
        {
            public static string Name = "EscapeFromTarkov";

            public static Process Process { get; set; }
        }
    }
}
