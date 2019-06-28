using System;
using Overwolf.Other;
using Overwolf.Managers;

namespace Overwolf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = $"Overwolf - Build ({ Extensions.AssemblyCreationDate() })";
            Globals.Proc.Process = Extensions.Proc.WaitForProcess(Globals.Proc.Name);
            MemoryManager.Initialize(Globals.Proc.Process.Id);

            /* Temp */
            Extensions.Information("---------------------------------------------]", true);
            Extensions.Information("[TempMessage] Toggle ESP:     F8", true);
            Extensions.Information("[TempMessage] Toggle Aimbot:   F10", true);
            Extensions.Information("---------------------------------------------]", true);

            ThreadManager.Add("Watcher", Watcher.Run);
            ThreadManager.Add("Reader", Reader.Run);

            ThreadManager.Add("ESP", ESP.Run);
            ThreadManager.Add("Aimbot", Aimbot.Run);

            ThreadManager.ToggleThread("Watcher");
            ThreadManager.ToggleThread("Reader");

            if (Settings.Glow.Enabled) ThreadManager.ToggleThread("ESP");
            if (Settings.Aimbot.Enabled) ThreadManager.ToggleThread("Aimbot");
        }
    }
}
