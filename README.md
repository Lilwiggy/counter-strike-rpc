# CS:GO Rich Presence
Look at <a href="#notes">notes</a> before running the presence application.

# Installation

# Installer
Head on over to the <a href="/releases">releases</a> and download the latest release.
The installtion process is as follows:

Step 1:
Locate your Steam directory. For me it's in C:\Program Files (x86)\Steam
Really what I'm looking for is the directory with the steamapps folder in it (where your games are installed from steam)

Step 2:
Unzip the release.

Step 3:
Open "Installer.exe".

Step 4:
Hit the button that says "Locate directory"
This will pull up a folder browser window. Locate the directory you found in step 1.
When you have found the Steam directory, hit the button that says "ok" in the botom right.

Step 5:
After it says it has installed you can close the Installer.
Find the folder named "Files" and open that and then go to the folder named "Presence"

Step 6:
Open the "Counter-Strike Presence.exe" and be on your way and glhf <3

Step 7 (optional):

Right click "Counter-Strike Presence.exe" and hover over "Send to" and select desktop to make a shortcut for the presence.
You can also move this folder to your desktop to make it easier to open.

# Manual install

Open the folder named "Files" and copy the file named "gamestate_integration_discordpresence.cfg".
Find the directory where CSGO is installed (normally it's C:\Program Files (x86)\Steam\steamapps\common\Counter-Strike Global Offensive)
When you find it find the folder named "csgo" and the in that the folder named "cfg".
In the "cfg" folder paste the cfg file you coped earlier.

# Notes
Open CSGO first and then the presence app as opening the presence app first will cause some issues with discord and displaying your status.

If you open the presence application and you run into this error:
"Unhandled Exception: System.Net.HttpListenerException: Access is denied"
just right click the presence application and run it as administrator.
