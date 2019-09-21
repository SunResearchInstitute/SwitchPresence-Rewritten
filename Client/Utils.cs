using System.Runtime.InteropServices;

namespace SwitchPresence_Rewritten
{
    public static class Utils
    {
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            T stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }
    }
}
