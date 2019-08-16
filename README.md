# SwitchPresence-Rewritten
[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/X8X0LUTH)<br>
Change your Discord rich presence to your currently playing Nintendo Switch game! Concept taken from [SwitchPresence](https://github.com/Random0666/SwitchPresence) by [Random](https://github.com/Random0666)<br>

# Setup
General switch setup can be found [here](https://switch.homebrew.guide/).<br>

Simply Create an application at the [Discord Developer Portal](https://discordapp.com/developers/applications/) call your application `Nintendo Switch` or whatever you would like and then enter your client ID and switch's IP into the SwitchPresence client!<br>

You can also optionally dump game icons using a helper homebrew included in releases it will also give you the option to toggle the SwitchPresence sysmodule!<br>
After you have dumped the icons you can bulk upload them to your Discord Developer Application under `Rich Presence->Art Assets` you can upload them with the name given to them on dump or optionally upload your own icon and set the SwitchPresence client to load that icon using the name of the custom icon.<br>

# Technical Info
The protocol for the sysmodule is a very simple struct sent via TCP<br>
```
struct titlepacket
{
    u32 magic; //Padded to 8 bytes by the compiler
    u64 tid;
    char name[512];
};
```
**Please note that magic is padded to 8 bytes which can be read into a u64 if wanted**<br>
The Packet is sent about every 5 seconds to the client from the server (in this case the switch).<br>
If a client is not connect it will not send anything.<br>
