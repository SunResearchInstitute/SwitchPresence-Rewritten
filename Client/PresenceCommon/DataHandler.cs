using DiscordRPC;
using PresenceCommon.Types;
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

        public static RichPresence CreateDiscordPresence(Title title, Timestamps time, string largeImageKey = "", string largeImageText = "", string smallImageKey = "", string state = "")
        {
            RichPresence presence = new RichPresence()
            {
                State = state
            };

            Assets assets = new Assets
            {
                SmallImageKey = smallImageKey,
                SmallImageText = "Switch-Presence Rewritten"
            };

            if (title.Name == "NULL")
            {
                assets.LargeImageText = !string.IsNullOrWhiteSpace(largeImageText) ? largeImageText : "Home Menu";
                assets.LargeImageKey = !string.IsNullOrWhiteSpace(largeImageKey) ? largeImageKey : string.Format("0{0:x}", 0x0100000000001000);
                presence.Details = "In the home menu";
            }
            else
            {
                assets.LargeImageText = !string.IsNullOrWhiteSpace(largeImageText) ? largeImageText : title.Name;
                assets.LargeImageKey = !string.IsNullOrWhiteSpace(largeImageKey) ? largeImageKey : string.Format("0{0:x}", title.Tid);
                presence.Details = $"Playing {title.Name}";
            }
            presence.Assets = assets;
            presence.Timestamps = time;

            return presence;
        }
    }
}
