using Mono.Simd;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using FontFactory = SharpDX.DirectWrite.Factory;
using DirectX_Renderer;
using System.Windows.Forms;

namespace EasyMemoryCleaner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Overlay());
        }
    }

    public class EasyMemoryCleaner
    {
        public Memory mem = new Memory();
        public UnityEngine.Vector2 windowSize = new UnityEngine.Vector2(0, 0);

        public enum eHumanBones
        {
            HumanBase = 0,
            HumanPelvis = 14,
            HumanLThigh1 = 15,
            HumanLThigh2 = 16,
            HumanLCalf = 17,
            HumanLFoot = 18,
            HumanLToe = 19,
            HumanRThigh1 = 20,
            HumanRThigh2 = 21,
            HumanRCalf = 22,
            HumanRFoot = 23,
            HumanRToe = 24,
            HumanSpine1 = 29,
            HumanSpine2 = 36,
            HumanSpine3 = 37,
            HumanLCollarbone = 89,
            HumanLUpperarm = 90,
            HumanLForearm1 = 91,
            HumanLForearm2 = 92,
            HumanLForearm3 = 93,
            HumanLPalm = 94,
            HumanRCollarbone = 110,
            HumanRUpperarm = 111,
            HumanRForearm1 = 112,
            HumanRForearm2 = 113,
            HumanRForearm3 = 114,
            HumanRPalm = 115,
            HumanNeck = 132,
            HumanHead = 133
        };

        private struct Matrix34
        {
            public Vector4f vec0;
            public Vector4f vec1;
            public Vector4f vec2;
        };
        private IntPtr GetPtr(IntPtr addr, int offset, Memory mem)
        {
            return IntPtr.Add((IntPtr)(mem.Read<long>((long)addr)), offset);
        }
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public unsafe UnityEngine.Vector3 INTERNAL__getPosition(IntPtr addr, Memory mem)
        {
            try
            {
                IntPtr transform_internal = IntPtr.Add(addr, 0x10);
                if (transform_internal.ToInt64() == 0x0)
                    return new UnityEngine.Vector3();

                IntPtr pMatrix = GetPtr(transform_internal, 0x38, mem);
                int index = mem.Read<int>(GetPtr(transform_internal, 0x38 + sizeof(UInt64), mem).ToInt64());
                if (pMatrix.ToInt64() == 0x0)
                    return new UnityEngine.Vector3();

                IntPtr matrix_list_base = GetPtr(pMatrix, 0x8, mem);
                if (matrix_list_base.ToInt64() == 0x0)
                    return new UnityEngine.Vector3();

                IntPtr dependency_index_table_base = GetPtr(pMatrix, 0x10, mem);
                if (dependency_index_table_base.ToInt64() == 0x0)
                    return new UnityEngine.Vector3();

                byte[] pMatricesBuff = mem.ReadByteArray(GetPtr(matrix_list_base, 0, mem).ToInt64(), (sizeof(Matrix34) * index) + sizeof(Matrix34));
                GCHandle pMatricesBufff = GCHandle.Alloc(pMatricesBuff, GCHandleType.Pinned);
                void* pMatricesBuf = pMatricesBufff.AddrOfPinnedObject().ToPointer();

                byte[] pIndicesBuff = mem.ReadByteArray(GetPtr(dependency_index_table_base, 0, mem).ToInt64(), sizeof(int) * index + sizeof(int));
                GCHandle pIndicesBufff = GCHandle.Alloc(pIndicesBuff, GCHandleType.Pinned);
                void* pIndicesBuf = pIndicesBufff.AddrOfPinnedObject().ToPointer();

                Vector4f result = *(Vector4f*)((UInt64)pMatricesBuf + 0x30 * (UInt64)index);

                int index_relation = *(int*)((UInt64)pIndicesBuf + 0x4 * (UInt64)index);

                Vector4f xmmword_1410D1340 = new Vector4f(-2.0f, 2.0f, -2.0f, 0.0f);
                Vector4f xmmword_1410D1350 = new Vector4f(2.0f, -2.0f, -2.0f, 0.0f);
                Vector4f xmmword_1410D1360 = new Vector4f(-2.0f, -2.0f, 2.0f, 0.0f);

                int tries = 10000;
                int tried = 0;

                try
                {
                    while (index_relation >= 0)
                    {
                        if (tried++ > tries) break;
                        Matrix34 matrix34 = *(Matrix34*)((UInt64)pMatricesBuf + (0x30 * (UInt64)index_relation));

                        Vector4f v10 = matrix34.vec2 * result;
                        Vector4f v11 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(0)));
                        Vector4f v12 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(85)));
                        Vector4f v13 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(-114)));
                        Vector4f v14 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(-37)));
                        Vector4f v15 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(-86)));
                        Vector4f v16 = (Vector4f)(VectorOperations.Shuffle(matrix34.vec1, (ShuffleSel)(113)));
                        result = (((((((v11 * xmmword_1410D1350) * v13) - ((v12 * xmmword_1410D1360) * v14)) * VectorOperations.Shuffle(v10, (ShuffleSel)(-86))) +
                                ((((v15 * xmmword_1410D1360) * v14) - ((v11 * xmmword_1410D1340) * v16)) * VectorOperations.Shuffle(v10, (ShuffleSel)(85)))) +
                                (((((v12 * xmmword_1410D1340) * v16) - (v15 * xmmword_1410D1350 * v13)) * VectorOperations.Shuffle(v10, (ShuffleSel)(0))) + v10)) + matrix34.vec0);

                        index_relation = *(int*)((UInt64)pIndicesBuf + 0x4 * (UInt64)index_relation);
                    } // Not the cause of the memory leak
                }
                catch (Exception e)
                {
                    Console.WriteLine("Caught INTERNAL__getPosition exception - " + e.ToString());
                    pIndicesBufff.Free();
                    pMatricesBufff.Free();
                    return new UnityEngine.Vector3();
                }

                pIndicesBufff.Free();
                pMatricesBufff.Free();

                return new UnityEngine.Vector3(result.X, result.Y, result.Z);
            }
            catch (Exception e)
            {
                Console.WriteLine("GetBonePosition Crashed");
                return new UnityEngine.Vector3();
            }
        }

        public EasyMemoryCleaner(UnityEngine.Vector2 win)
        {
            windowSize = win;
        }

        private int paintCalled = 0;
        private Camera FPSCamera = null;
        private GameObject GW = null;
        private LocalGameWorld LGW = null;
        private Player[] players = null;

        private Player local;

        private Renderer rend = new Renderer();

        public void ReadWorlds()
        {
            while (true)
            {
                System.Threading.Thread.Sleep(5);

                paintCalled++;

                try
                {
                    GW = mem.FindActiveObject("GameWorld");
                    LGW = GW.GetLocalGameWorldObject();

                    players = LGW.GetPlayerArrayObject().GetPlayers();
                    if (paintCalled >= 500 || FPSCamera == null)
                    {
                        FPSCamera = new Camera(mem.FindTaggedObject("fps camera").addr, mem);
                        paintCalled = 0;
                    }
                    FPSCamera.GetViewmatrix();
                }
                catch (Exception e)
                {
                    Console.WriteLine("ReadWorlds crashed - " + e.ToString());
                    continue;
                }
            }
        }

        public void Draw(WindowRenderTarget device)
        {
            rend.device = device;

            SolidColorBrush solidColorBrush = new SolidColorBrush(device, SharpDX.Color.Red);
            TextFormat espFont = new TextFormat(new FontFactory(), "Tahoma Bold", 10f);

            System.Threading.Thread.Sleep(5);

            if (LGW == null || GW == null || players == null || FPSCamera == null)
                return;

            UnityEngine.Vector3 LocalLocation = new UnityEngine.Vector3();

            int Players = 0, Scavs = 0, PScavs = 0;

            foreach (Player p in players)
            {
                bool Local = p.IsLocal();

                if (Local)
                {
                    local = p;
                    LocalLocation = local.GetMovementContext().GetLocation();
                }

                if (p == null || LocalLocation == new UnityEngine.Vector3() || Local)
                    continue;

                IntPtr BoneMatrix = p.GetBoneMatrix();
                UnityEngine.Vector3 Location = INTERNAL__getPosition(mem.Read<IntPtr>(BoneMatrix + (0x20 + ((int)eHumanBones.HumanBase * 8))), mem);

                if (Location == new UnityEngine.Vector3(0, 0, Location.z))
                    continue;

                PlayerProfile pProfile = p.GetPlayerProfile();
                bool IsPlayer = pProfile.IsPlayer();
                PlayerInfo pInfo = pProfile.GetPlayerInfo();

                int RegisterDate = pInfo.GetRegistrationDate();

                if (IsPlayer == false)
                {
                    if (RegisterDate != 0)
                        PScavs++;
                    else
                        Scavs++;
                }
                else
                    Players++;

                UnityEngine.Vector3 baseW2S, headW2S;

                if (!FPSCamera.WorldToScreen(Location, out baseW2S, windowSize))
                    continue;

                float _Distance = UnityEngine.Vector3.Distance(LocalLocation, Location);

                if (baseW2S == new UnityEngine.Vector3(0, 0, baseW2S.z) || _Distance >= 200 || baseW2S.x > windowSize.x + 150 || baseW2S.x < -150 || baseW2S.y < -150)
                    continue;

                UnityEngine.Vector3 headPos = INTERNAL__getPosition(mem.Read<IntPtr>(BoneMatrix + (0x20 + ((int)eHumanBones.HumanHead * 8))), mem);

                if (!FPSCamera.WorldToScreen(headPos, out headW2S, windowSize))
                    continue;

                UnityEngine.Vector2 wSize;
                wSize.y = Math.Abs(headW2S.y - baseW2S.y) * 1.3f;
                wSize.x = (wSize.y / 1.7f);

                System.Drawing.Rectangle boxRect = new System.Drawing.Rectangle((int)(baseW2S.x - (wSize.x / 2)), (int)(baseW2S.y - wSize.y), (int)wSize.x, (int)wSize.y);
                
                rend.DrawBoxESP(solidColorBrush, boxRect);

                string pName = "N/A";

                if (IsPlayer == false)
                {
                    if (RegisterDate != 0)
                    {
                        pName = "Player Scav";
                    }
                    else
                    {
                        pName = "Scav";
                    }
                }
                else
                {
                    Players++;
                    pName = pInfo.GetName();
                }

                solidColorBrush.Color = Color.White;

                if (_Distance <= 100f)
                {
                    rend.RectHealthBar(baseW2S.x - ((wSize.x + 12) / 2), (baseW2S.y - wSize.y - 1), 3.5f, wSize.y, (int)p.GetBodyController().GetHealthPercentage());
                }

                float headSZ;
                if (_Distance == 0)
                    headSZ = 20;
                headSZ = 2 + (20 / _Distance);
                rend.DrawCircle(new Ellipse(new RawVector2(headW2S.x, headW2S.y - headSZ), headSZ, headSZ), solidColorBrush, false);

                rend.DrawText(pName + " (" + (int)_Distance + "m)", new RawVector2(boxRect.Left, boxRect.Bottom + 1), solidColorBrush, espFont);
                solidColorBrush.Color = Color.Red;
            }

            solidColorBrush.Color = Color.White;
            TextFormat watermarkFont = new TextFormat(new FontFactory(), "Tahoma", 15f);
            rend.DrawText("Players: " + Players, new RawVector2(5, 5), solidColorBrush, watermarkFont);
            rend.DrawText("Player Scavs: " + PScavs, new RawVector2(5, 20), solidColorBrush, watermarkFont);
            rend.DrawText("Scavs: " + Scavs, new RawVector2(5, 35), solidColorBrush, watermarkFont);

            watermarkFont.Dispose();
            solidColorBrush.Dispose();
            espFont.Dispose();
        }

        public void DrawAll(WindowRenderTarget device)
        {
            rend.device = device;

            SolidColorBrush solidColorBrush = new SolidColorBrush(device, SharpDX.Color.Red);
            TextFormat espFont = new TextFormat(new FontFactory(), "Tahoma Bold", 10f);

            System.Threading.Thread.Sleep(5);

            if (LGW == null || GW == null || players == null || FPSCamera == null)
                return;

            UnityEngine.Vector3 LocalLocation = new UnityEngine.Vector3();

            int Players = 0, Scavs = 0, PScavs = 0;

            foreach (Player p in players)
            {
                bool Local = p.IsLocal();
                local = p;
                LocalLocation = local.GetMovementContext().GetLocation();

                if (p == null || LocalLocation == new UnityEngine.Vector3())
                    continue;

                IntPtr BoneMatrix = p.GetBoneMatrix();
                UnityEngine.Vector3 Location = INTERNAL__getPosition(mem.Read<IntPtr>(BoneMatrix + (0x20 + ((int)eHumanBones.HumanBase * 8))), mem);

                if (Location == new UnityEngine.Vector3(0, 0, Location.z))
                    continue;

                PlayerProfile pProfile = p.GetPlayerProfile();
                bool IsPlayer = pProfile.IsPlayer();
                PlayerInfo pInfo = pProfile.GetPlayerInfo();

                int RegisterDate = pInfo.GetRegistrationDate();

                if (IsPlayer == false)
                {
                    if (RegisterDate != 0)
                        PScavs++;
                    else
                        Scavs++;
                }
                else
                    Players++;

                UnityEngine.Vector3 baseW2S, headW2S;

                if (!FPSCamera.WorldToScreen(Location, out baseW2S, windowSize))
                    continue;

                if (baseW2S == new UnityEngine.Vector3(0, 0, baseW2S.z) || baseW2S.x > windowSize.x + 150 || baseW2S.x < -150 || baseW2S.y < -150)
                    continue;

                UnityEngine.Vector3 headPos = INTERNAL__getPosition(mem.Read<IntPtr>(BoneMatrix + (0x20 + ((int)eHumanBones.HumanHead * 8))), mem);

                if (!FPSCamera.WorldToScreen(headPos, out headW2S, windowSize))
                    continue;

                UnityEngine.Vector2 wSize;
                wSize.y = Math.Abs(headW2S.y - baseW2S.y) * 1.3f;
                wSize.x = (wSize.y / 1.7f);

                System.Drawing.Rectangle boxRect = new System.Drawing.Rectangle((int)(baseW2S.x - (wSize.x / 2)), (int)(baseW2S.y - wSize.y), (int)wSize.x, (int)wSize.y);

                rend.DrawBoxESP(solidColorBrush, boxRect);

                string pName = "N/A";

                if (IsPlayer == false)
                {
                    if (RegisterDate != 0)
                    {
                        pName = "PScav";
                    }
                    else
                    {
                        pName = "Scav";
                    }
                }
                else
                {
                    Players++;
                    pName = pInfo.GetName();
                }

                solidColorBrush.Color = Color.White;

                rend.RectHealthBar(baseW2S.x - ((wSize.x + 12) / 2), (baseW2S.y - wSize.y - 1), 3.5f, wSize.y, (int)p.GetBodyController().GetHealthPercentage());
                float headSZ;
                headSZ = 2;
                rend.DrawCircle(new Ellipse(new RawVector2(headW2S.x, headW2S.y - headSZ), headSZ, headSZ), solidColorBrush, false);
                rend.DrawText(pName, new RawVector2(boxRect.Left, boxRect.Bottom + 1), solidColorBrush, espFont);
                solidColorBrush.Color = Color.Red;
            }

            solidColorBrush.Color = Color.White;
            TextFormat watermarkFont = new TextFormat(new FontFactory(), "Tahoma", 12f);
            rend.DrawText("Players: " + Players, new RawVector2(5, 5), solidColorBrush, watermarkFont);
            rend.DrawText("Player Scavs: " + PScavs, new RawVector2(5, 15), solidColorBrush, watermarkFont);
            rend.DrawText("Scavs: " + Scavs, new RawVector2(5, 25), solidColorBrush, watermarkFont);
            rend.DrawText("Total: " + players.Length, new RawVector2(5, 35), solidColorBrush, watermarkFont);

            watermarkFont.Dispose();
            solidColorBrush.Dispose();
            espFont.Dispose();
        }
    }
}