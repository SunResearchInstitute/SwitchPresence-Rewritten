using System;
using System.Runtime.InteropServices;

namespace PresenceCommon
{
    public class DataHandler
    {
        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
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
