using System.Threading;

using Overwolf.Other;

namespace Overwolf.Managers
{
    internal class Reader
    {
        public static void Run()
        {
            while (true)
            {
                Thread.Sleep(1);

                Structs.LocalPlayer.Base = MemoryManager.ReadMemory<int>((int)Structs.Base.Client + Offsets.dwLocalPlayer);
                Structs.LocalPlayer_t localPlayerStruct = MemoryManager.ReadMemory<Structs.LocalPlayer_t>(Structs.LocalPlayer.Base);
                Structs.LocalPlayer.LifeState = localPlayerStruct.LifeState;
                Structs.LocalPlayer.Health = localPlayerStruct.Health;
                Structs.LocalPlayer.Dormant = localPlayerStruct.Dormant;
                Structs.LocalPlayer.Flags = localPlayerStruct.Flags;
                Structs.LocalPlayer.MoveType = localPlayerStruct.MoveType;
                Structs.LocalPlayer.Team = localPlayerStruct.Team;
                Structs.LocalPlayer.ShotsFired = localPlayerStruct.ShotsFired;
                Structs.LocalPlayer.CrosshairID = localPlayerStruct.CrosshairID;
                Structs.LocalPlayer.Position = localPlayerStruct.Position;
                Structs.LocalPlayer.AimPunch = localPlayerStruct.AimPunch;
                Structs.LocalPlayer.VecView = localPlayerStruct.VecView;

                Structs.ClientState.Base = MemoryManager.ReadMemory<int>((int)Structs.Base.Engine + Offsets.dwClientState);
                Structs.ClientState_t clientStateStruct = MemoryManager.ReadMemory<Structs.ClientState_t>(Structs.ClientState.Base);
                Structs.ClientState.GameState = clientStateStruct.GameState;
                Structs.ClientState.ViewAngles = clientStateStruct.ViewAngles;
                Structs.ClientState.MaxPlayers = clientStateStruct.MaxPlayers;
            }
        }
    }
}
