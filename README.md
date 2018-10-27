# CS:GO Rich Presence
Look at <a href="#notes">notes</a> before running the presence application.

# Running

In order for the presence to work, hit the button that says "Launch CS:GO".

This will minimize the application to your system tray (the ^ icon on your taskbar on the far right side).
To fully close it right click the counter strike icon and hit "Quit".

# Installer
Head on over to the <a href="https://github.com/Lilwiggy/counter-strike-rpc/releases">releases</a> and download the latest release.
The installtion process is as follows:

Step 1:
Hit install. (Mind blowing I know) It will try and automatically find your Steam directory. Assuming it does, you're done.

If it doesn't find your Steam directory, it will ask you for your steam directory.
Locate your steam directory from the browser window and hit "Ok".

# Manual install

Open the folder named "Files" and copy the file named "gamestate_integration_discordpresence.cfg".
Find the directory where CSGO is installed (normally it's C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive)
When you find it find the folder named "csgo" and the in that the folder named "cfg".
In the "cfg" folder paste the cfg file you coped earlier.

# Notes
If you open the presence application and you run into this error:
"Unhandled Exception: System.Net.HttpListenerException: Access is denied"
just right click the application and run it as administrator.
