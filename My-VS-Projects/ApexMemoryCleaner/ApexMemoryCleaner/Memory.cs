using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ApexMemoryCleaner
{
    [Flags]
    public enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
    }

    public class Memory
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("Kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("Kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern int CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        private string processName = "EscapeFromTarkov";
        IntPtr processHandle = IntPtr.Zero;
        IntPtr baseAddr = IntPtr.Zero;
        Process targetProcess = null;
        GameObjectManager GOM = null;

        public Memory()
        {
            Process[] procID = Process.GetProcessesByName(processName);
            if (procID.Length > 0)
            {
                targetProcess = procID[0];

                targetProcess.PriorityClass = ProcessPriorityClass.High;
                logger.Info("Setting " + processName + " priority to high");

                processHandle = OpenProcess(ProcessAccessFlags.VMRead, false, targetProcess.Id);

                baseAddr = targetProcess.MainModule.BaseAddress;

                GOM = new GameObjectManager(Read<long>(baseAddr.ToInt64() + 0x1432840), this);
                logger.Info("Game Object Manager Found !!!");
            }
            else
            {
                logger.Info("ERROR: Could not find " + processName + " process");
                Application.Exit();
            }

            Process[] dwmID = Process.GetProcessesByName("dwm");
            if (dwmID.Length > 0)
            {
                dwmID[0].PriorityClass = ProcessPriorityClass.Idle; // Overlay makes DWM CPU usage skyrocket
                logger.Info("Setting dwm.exe priority to low");
            }
        }

        int Exit()
        {
            return CloseHandle(processHandle);
        }

        public GameObject FindActiveObject(string objName)
        {
            BaseObject activeObject = GOM.GetFirstActiveObject();
            do
            {
                if (activeObject.GetGameObject().GetObjectName().ToLower().Equals(objName.ToLower()))
                {
                    return activeObject.GetGameObject();
                }
                activeObject = activeObject.GetNextBaseObject();
            } while (activeObject.GetAddr() != GOM.GetFirstActiveObject().GetAddr());

            return null;
        }

        public GameObject FindTaggedObject(string objName)
        {
            BaseObject taggedObject = GOM.GetFirstTaggedObject();
            int MAX_CT = 2000;
            int i = 0;
            do
            {
                i++;
                if (taggedObject.GetGameObject().GetObjectName().ToLower().Equals(objName.ToLower()))
                {
                    return taggedObject.GetGameObject();
                }
                if (i >= MAX_CT)
                {
                    logger.Info("Overflow when finding tagged object: " + objName);
                    return new GameObject();
                }
                taggedObject = taggedObject.GetNextBaseObject();
            } while (taggedObject.GetAddr() != GOM.GetFirstActiveObject().GetAddr());

            return new GameObject();
        }

        public GameObject FindObject(BaseObject b, string objName)
        {
            BaseObject activeObject = b;
            do
            {
                if (activeObject.GetGameObject().GetObjectName().ToLower().Equals(objName.ToLower()))
                {
                    return activeObject.GetGameObject();
                }
                activeObject = activeObject.GetNextBaseObject();
            } while (activeObject.GetAddr() != b.GetAddr());
            return null;
        }

        public string UReadString(long ptr)
        {
            long unicodeStringObjAddr = Read<long>(ptr);
            int length = Read<int>(unicodeStringObjAddr + 0x10);
            byte[] b = ReadByteArray(unicodeStringObjAddr + 0x14, length * 2);
            return System.Text.Encoding.Unicode.GetString(b);
        }

        public T Read<T>(IntPtr ptr)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buf = new byte[size];
            int read = 0;
            ReadProcessMemory((int)processHandle, ptr.ToInt64(), buf, size, ref read);

            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);
            T data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return data;
        }
        public T Read<T>(long ptr)
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buf = new byte[size];
            int read = 0;
            ReadProcessMemory((int)processHandle, (long)ptr, buf, size, ref read);

            GCHandle handle = GCHandle.Alloc(buf, GCHandleType.Pinned);
            T data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return data;
        }

        public byte[] ReadByteArray(long ptr, int sz)
        {
            byte[] buf = new byte[sz];
            int read = 0;
            ReadProcessMemory((int)processHandle, (long)ptr, buf, sz, ref read);
            return buf;
        }
    }
}