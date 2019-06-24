using System;
using System.Text;
using System.Numerics;

namespace SupremeMemCleaner
{
    public class Player : BaseClass
    {
        public Player() { }
        public Player(long addr, Memory mem) : base(addr, mem) { }

        public IntPtr GetBoneMatrix()
        {
            IntPtr temp = mem.Read<IntPtr>(addr + 0x88);
            IntPtr temp1 = mem.Read<IntPtr>(temp + 0x28);
            IntPtr temp2 = mem.Read<IntPtr>(temp1 + 0x28);
            return mem.Read<IntPtr>(temp2 + 0x10);
        }

        public PlayerProfile GetPlayerProfile()
        {
            return new PlayerProfile(mem.Read<long>(addr + 0x490), mem); ;
        }

        public BodyController GetBodyController()
        {
            return new BodyController(mem.Read<long>(mem.Read<long>(mem.Read<long>(addr + 0x4c0) + 0x18) + 0x28), mem);
        }

        public bool IsLocal()
        {
            return mem.Read<bool>(addr + 0x18);
        }

        public MovementContext GetMovementContext()
        {
            return new MovementContext(mem.Read<long>(addr + 0x38), mem);
        }
    }

    public class MovementContext : BaseClass
    {
        public MovementContext(long addr, Memory mem) : base(addr, mem) { }

        public UnityEngine.Vector3 GetLocation()
        {
            return mem.Read<UnityEngine.Vector3>(addr + 0x68);
        } // WILL ONLY UPDATE WHEN TARGET IS WITHIN FOV, USE BASE BONE POSITION | SHOULD ONLY BE USED FOR LOCAL
    }

    public class BodyController : BaseClass
    {
        public BodyController(long addr, Memory mem) : base(addr, mem) { }

        public enum BodyParts
        {
            Head = 0x20,
            Thorax = 0x28,
            Stomach = 0x30,
            LeftArm = 0x38,
            RightArm = 0x40,
            LeftLeg = 0x48,
            RightLeg = 0x50
        }

        public float GetBodyPartHP(BodyParts part)
        {
            return mem.Read<float>(mem.Read<long>(mem.Read<long>(addr + (Int32)part) + 0x10) + 0x10);
        }

        public float GetMaxBodyPartHP(BodyParts part)
        {
            return mem.Read<float>(mem.Read<long>(mem.Read<long>(addr + (Int32)part) + 0x10) + 0x14);
        }

        public float GetMaxSumOfAllBodyParts()
        {
            if (GetMaxBodyPartHP(BodyParts.Thorax) == 80)
                return 435;
            else
                return 725;
        }

        public float GetSumOfAllBodyParts()
        {
            float ret = 0f;
            foreach (BodyParts part in Enum.GetValues(typeof(BodyParts)))
            {
                ret += GetBodyPartHP(part);
            }
            return ret;
        }

        public float GetHealthPercentage()
        {
            return (GetSumOfAllBodyParts() / GetMaxSumOfAllBodyParts()) * 100;
        }
    }

    public class PlayerProfile : BaseClass
    {
        public PlayerProfile(long addr, Memory mem) : base(addr, mem) { }

        public bool IsPlayer()
        {
            return mem.Read<bool>(addr + 0x20);
        }

        public PlayerInfo GetPlayerInfo()
        {
            return new PlayerInfo(mem.Read<long>(addr + 0x28), mem);
        }
    }

    public class PlayerInfo : BaseClass
    {
        public PlayerInfo(long addr, Memory mem) : base(addr, mem) { }

        public string GetName()
        {
            return mem.UReadString(addr + 0x10);
        }

        public int GetRegistrationDate()
        {
            return mem.Read<int>(addr + 0x54);
        }

        static string[] CyrilicToLatinL =
        "a,b,v,g,d,e,zh,z,i,j,k,l,m,n,o,p,r,s,t,u,f,kh,c,ch,sh,sch,j,y,j,e,yu,ya".Split(',');
        static string[] CyrilicToLatinU =
          "A,B,V,G,D,E,Zh,Z,I,J,K,L,M,N,O,P,R,S,T,U,F,Kh,C,Ch,Sh,Sch,J,Y,J,E,Yu,Ya".Split(',');

        public static string CyrilicToLatin(string s)
        {
            var sb = new StringBuilder((int)(s.Length * 1.5));
            foreach (char c in s)
            {
                if (c >= '\x430' && c <= '\x44f') sb.Append(CyrilicToLatinL[c - '\x430']);
                else if (c >= '\x410' && c <= '\x42f') sb.Append(CyrilicToLatinU[c - '\x410']);
                else if (c == '\x401') sb.Append("Yo");
                else if (c == '\x451') sb.Append("yo");
                else sb.Append(c);
            }
            return sb.ToString();
        }

        public string getScavName(string name)
        {
            string converted = CyrilicToLatin(name);
            return converted;
        }

        public string IfIsScavBoss(string name)
        {
            string converted = CyrilicToLatin(name);
            if (converted == "Reshala" || converted == "Killa")
                return converted;
            else return "Scav";
        }
    }

    public class Camera : GameObject
    {
        private long matrixLeadup;
        private Matrix4x4 _viewMatrix;

        public Camera(long addr, Memory mem) : base(addr, mem)
        {
            matrixLeadup = mem.Read<long>(mem.Read<long>(addr + 0x30) + 0x18);
        }

        public void GetViewmatrix()
        {
            _viewMatrix = mem.Read<Matrix4x4>(matrixLeadup + 0xC0);
        }

        public bool WorldToScreen(UnityEngine.Vector3 _Enemy, out UnityEngine.Vector3 _Screen, UnityEngine.Vector2 windowSize)
        {
            _Screen = new UnityEngine.Vector3(0, 0, 0);

            //Getting viewmatrix from fpsCamera
            //fpsCamera, new int[] { 0x30, 0x18, 0xC0 }

            Matrix4x4 temp = Matrix4x4.Transpose(_viewMatrix);

            UnityEngine.Vector3 translationVector = new UnityEngine.Vector3(temp.M41, temp.M42, temp.M43);
            UnityEngine.Vector3 up = new UnityEngine.Vector3(temp.M21, temp.M22, temp.M23);
            UnityEngine.Vector3 right = new UnityEngine.Vector3(temp.M11, temp.M12, temp.M13);

            float w = D3DXVec3Dot(translationVector, _Enemy) + temp.M44;

            if (w < 0.098f)
                return false;

            float y = D3DXVec3Dot(up, _Enemy) + temp.M24;
            float x = D3DXVec3Dot(right, _Enemy) + temp.M14;

            _Screen.x = (windowSize.x / 2) * (1f + x / w);
            _Screen.y = (windowSize.y / 2) * (1f - y / w);
            _Screen.z = w;

            return true;

        }

        private float D3DXVec3Dot(UnityEngine.Vector3 a, UnityEngine.Vector3 b)
        {
            return (a.x * b.x +
                    a.y * b.y +
                    a.z * b.z);
        }
    };
}
