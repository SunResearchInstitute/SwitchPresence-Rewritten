using System.Runtime.InteropServices;

namespace PresenceCommon
{
    internal static class DataHandler
    {
        internal static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T data;
            try
            {
                data = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return data;
        }
    }
}
