using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMemoryCleaner
{
    public class BaseClass
    {
        public long addr = 0;
        public Memory mem = null;

        public BaseClass() { }
        public BaseClass(long addr, Memory mem)
        {
            this.addr = addr;
            this.mem = mem;
        }

        public long GetAddr()
        {
            return addr;
        }
    }

    public class BaseObject : BaseClass
    {
        public BaseObject(long addr, Memory mem) : base(addr, mem) { }

        public long GetGameObjectAddr()
        {
            return mem.Read<long>(addr + 0x10);
        }

        public GameObject GetGameObject()
        {
            return new GameObject(GetGameObjectAddr(), mem);
        }

        public long GetNextBaseObjectAddr()
        {
            return mem.Read<long>(addr + 0x8);
        }

        public BaseObject GetNextBaseObject()
        {
            return new BaseObject(GetNextBaseObjectAddr(), mem);
        }
    }

    public class GameObject : BaseClass
    {
        public GameObject() : base() { }
        public GameObject(long addr, Memory mem) : base(addr, mem) { }

        public long GetGameObjectNameAddr()
        {
            return mem.Read<long>(addr + 0x60);
        }

        public string GetObjectName()
        {
            long objNameAddr = GetGameObjectNameAddr();
            int i = 0;
            for (; i < 256; i++)
            {
                if (mem.Read<byte>(objNameAddr + i) == 0)
                {
                    break;
                }
            }
            byte[] b = mem.ReadByteArray(objNameAddr, i);

            return System.Text.Encoding.UTF8.GetString(b);
        }

        public long GetN65Addr()
        {
            return mem.Read<long>(addr + 0x30);
        }

        public long GetLocalGameWorldObjectAddr()
        {
            long n65a = mem.Read<long>(addr + 0x30);
            long n70a = mem.Read<long>(n65a + 0x18);
            long lgwa = mem.Read<long>(n70a + 0x28);
            return lgwa;
        }

        public LocalGameWorld GetLocalGameWorldObject()
        {
            return new LocalGameWorld(GetLocalGameWorldObjectAddr(), mem);
        }

    }

    public class GameObjectManager : BaseClass
    {
        public GameObjectManager(long addr, Memory mem) : base(addr, mem) { }

        public long GetFirstActiveObjectAddr()
        {
            return mem.Read<long>(addr + 0x18);
        }

        public BaseObject GetFirstActiveObject()
        {
            return new BaseObject(GetFirstActiveObjectAddr(), mem);
        }
        //not tested
        public long GetLastActiveObjectAddr()
        {
            return mem.Read<long>(addr + 0x10);
        }
        //not tested
        public BaseObject GetLastActiveObject()
        {
            return new BaseObject(GetLastActiveObjectAddr(), mem);
        }


        public long GetFirstTaggedObjectAddr()
        {
            return mem.Read<long>(addr + 0x8);
        }

        public BaseObject GetFirstTaggedObject()
        {
            return new BaseObject(GetFirstTaggedObjectAddr(), mem);
        }
        //not tested
        public long GetLastTaggedObjectAddr()
        {
            return mem.Read<long>(addr + 0x0);
        }
        //not tested
        public BaseObject GetLastTaggedObject()
        {
            return new BaseObject(GetLastTaggedObjectAddr(), mem);
        }
    }

    public class LocalGameWorld : BaseClass
    {
        public LocalGameWorld(long addr, Memory mem) : base(addr, mem) { }

        public long GetPlayerArrayObjectAddr()
        {
            return mem.Read<long>(addr + 0x60);
        }

        public PlayerList GetPlayerArrayObject()
        {
            return new PlayerList(GetPlayerArrayObjectAddr(), mem);
        }
    }

    public class PlayerList : BaseClass
    {
        public PlayerList(long addr, Memory mem) : base(addr, mem) { }

        public int GetCount()
        {
            return mem.Read<int>(addr + 0x18);
        }

        public Player[] GetPlayers()
        {
            int playerCount = GetCount();

            long fpAddr = mem.Read<long>(addr + 0x10);
            Player[] p = new Player[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                p[i] = new Player(mem.Read<long>(fpAddr + 0x20 + (i * 0x8)), mem);
            }
            return p;
        }
    }
}
