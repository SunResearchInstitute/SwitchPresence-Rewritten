using DiscordRPC;
using PresenceCommon.Types;

namespace PresenceCommon
{
    public static class Utils
    {
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
                assets.LargeImageKey = !string.IsNullOrWhiteSpace(largeImageKey) ? largeImageKey : $"0{0x0100000000001000:x}";
                presence.Details = "In the home menu";
            }
            else
            {
                assets.LargeImageText = !string.IsNullOrWhiteSpace(largeImageText) ? largeImageText : title.Name;
                assets.LargeImageKey = !string.IsNullOrWhiteSpace(largeImageKey) ? largeImageKey : $"0{title.Tid:x}";
                presence.Details = $"Playing {title.Name}";
            }
            presence.Assets = assets;

            if (time != null)
            {
                presence.Timestamps = time;
            }

            return presence;
        }
    }
}
