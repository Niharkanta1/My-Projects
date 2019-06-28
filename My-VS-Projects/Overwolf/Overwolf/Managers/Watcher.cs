using System;
using System.Threading;

using Overwolf.Other;

namespace Overwolf.Managers
{
    class Watcher
    {
        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(75);

                if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.OtherControls.ToggleGlow) & 0x8000)) ThreadManager.ToggleThread("ESP");
                if (Convert.ToBoolean((long)Globals.Imports.GetAsyncKeyState(Settings.OtherControls.ToggleAimbot) & 0x8000)) ThreadManager.ToggleThread("Aimbot");               
            }
        }
    }
}
