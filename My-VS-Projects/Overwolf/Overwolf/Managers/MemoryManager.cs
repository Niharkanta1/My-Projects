using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Overwolf.Other;

namespace Overwolf.Managers
{
    class MemoryManager
    {
        #region Constants

        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;

        #endregion
        private static IntPtr m_pProcessHandle;

        private static int m_iNumberOfBytesRead;
        private static int m_iNumberOfBytesWritten;

        public static void Initialize(int ProcessID) => m_pProcessHandle = Globals.Imports.OpenProcess(PROCESS_VM_READ, false, ProcessID);

    }
}
