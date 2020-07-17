using System.Runtime.InteropServices;
using System.Text;

namespace PresenceCommon.Types
{
    public class Title
    {
        public ulong Magic { get; }
        public ulong ProgramId { get; }
        public string Name { get; }

        [StructLayout(LayoutKind.Sequential, Size = 528)]
        private struct TitlePacket
        {
            [MarshalAs(UnmanagedType.U8)]
            public ulong magic;
            [MarshalAs(UnmanagedType.U8)]
            public ulong programId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] name;
        }

        public Title(byte[] bytes)
        {
            TitlePacket title = DataHandler.ByteArrayToStructure<TitlePacket>(bytes);
            Magic = title.magic;
            ProgramId = title.programId;
            Name = Encoding.UTF8.GetString(title.name, 0, title.name.Length).Split('\0')[0];
        }
    }
}
