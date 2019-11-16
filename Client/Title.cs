using System.Runtime.InteropServices;
using System.Text;

namespace SwitchPresence_Rewritten
{
    class Title
    {
        public ulong Magic { get; }
        public ulong Tid { get; }
        public string Name { get; }

        [StructLayout(LayoutKind.Sequential, Size = 524)]
        private struct TitlePacket
        {
            [MarshalAs(UnmanagedType.U8)]
            public ulong magic;
            [MarshalAs(UnmanagedType.U8)]
            public ulong tid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] name;
        }

        public Title(byte[] bytes)
        {
            TitlePacket title = Utils.ByteArrayToStructure<TitlePacket>(bytes);
            Magic = title.magic;
            Tid = title.tid;
            Name = Encoding.UTF8.GetString(title.name, 0, title.name.Length).Split('\0')[0];
        }
    }
}
