using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using DiscordRPC;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Win32;
using CSGO_Presence.types;

namespace CSGO_Presence
{
    public partial class Form1 : Form
    {
        static double v = 3;
        static DateTime? Start = null;
        static bool WorkShop;
        static string Steam_ID;
        static string Map;
        static string TeamName;
        static string Mode;
        static dynamic Now;
        public static DiscordRpcClient client;
        static HttpListener listener = new HttpListener();
        public Form1()
        {
            InitializeComponent();
        }
        string cfgText = @"""Discord Presence v.1""
{
    ""uri"" ""http://127.0.0.1:2348""
    ""timeout"" ""5.0""
    ""buffer"" ""0.1""
    ""heartbeat"" ""15.0""
    ""data""
   {
        ""map"" ""1""
        ""round"" ""1""
        ""player_match_stats"" ""1""
        ""player_id"" ""1""
   }
}";

        // Oh yeah btw APR gay and NBK worst player
        private void InstallButton_Click(object sender, EventArgs e)
        {
            object steamReg = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", new object());
            if (File.Exists($@"{steamReg}\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
            {
                if (File.Exists($@"{steamReg}\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg"))
                {
                    MessageBox.Show("Nice! You have TWO versions installed now! But that'd be stupid so I did nothing.");
                }
                else
                {
                    File.WriteAllText($@"{steamReg}\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg", cfgText);
                    MessageBox.Show("Installed! I hope you and enjoy! glhf <3");
                }
            }
            else
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                int i = 0;
                foreach (DriveInfo drive in drives)
                {
                    if (File.Exists($@"{drive.Name}\Steam\steamapps\common\Counter-Strike Global Offensive\csgo.exe"))
                    {
                        if (File.Exists($@"{drive.Name}\Steam\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg"))
                        {
                            MessageBox.Show("Nice! You have TWO versions installed now! But that'd be stupid so I did nothing.");
                        }
                        else
                        {
                            File.WriteAllText($@"{drive.Name}\Steam\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg", cfgText);
                            MessageBox.Show("Installed! I hope you and enjoy! glhf <3");
                        }
                    }
                    else if (File.Exists($@"{drive.Name}\SteamLibrary\steamapps\common\Counter-Strike Global Offensive"))
                    {

                        if (File.Exists($@"{drive.Name}\SteamLibrary\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg"))
                        {
                            MessageBox.Show("Nice! You have TWO versions installed now! But that'd be stupid so I did nothing.");
                        }
                        else
                        {
                            File.WriteAllText($@"{drive.Name}\SteamLibrary\steamapps\common\Counter-Strike Global Offensive\csgo\cfg\gamestate_integration_discordpresence.cfg", cfgText);
                            MessageBox.Show("Installed! I hope you and enjoy! glhf <3");
                        }
                    }
                    i++;
                    if (i >= drives.Length)
                    {
                        MessageBox.Show("I tried to find a standard install path for CS:GO but couldn't find one :( Sorry about that. I will open your favorite browser with install instructions on how to install it manually :) glhf <3");
                        Process.Start($"https://github.com/Lilwiggy/counter-strike-rpc/blob/master/README.md#manual-install");
                    }
                      
                }
            }
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            Process csgo = new Process();
            csgo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            csgo.StartInfo.FileName = "CMD.exe";
            csgo.StartInfo.Arguments = "/C start steam://rungameid/730";
            csgo.Start();
            Hide();

            RunListener();

        }

        private void RunListener()
        {
            if (Start == null)
                Start = DateTime.UtcNow;
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:2348/");
            Task.Run(() => {
                listener.Start();
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                JObject JSON = JObject.Parse(GetRequestData(request));
                Response res = JSON.ToObject<Response>();
                UpdatePresence(res);
                string responseString = "";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                listener.Stop();
                RunListener();
            });
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            HttpClient http = new HttpClient();
            if (!HttpListener.IsSupported)
            {
                MessageBox.Show("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            client = new DiscordRpcClient("494943194165805082", true, 0);
            client.Initialize();
            
            var hi = await http.GetAsync("https://raw.githubusercontent.com/Lilwiggy/counter-strike-rpc/master/version.json");
            string s = await hi.Content.ReadAsStringAsync();
            dynamic JSON = JObject.Parse(s);

            if (JSON.v != v)
            {
                MessageBox.Show("You have an outdated version! I'll open up your favorite browser with a link to the latest version for you to download :)");
                Process.Start($"https://github.com/Lilwiggy/counter-strike-rpc/releases/tag/V{JSON.v}");
            }
            RunListener();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
                Hide();

            else if (FormWindowState.Normal == this.WindowState)
                NotifyIcon.Visible = false;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                NotifyIcon.ContextMenuStrip = new ContextMenuStrip();
                ToolStripButton quitButton = new ToolStripButton();
                quitButton.Text = "Quit";
                quitButton.Click += HandleQuit;
                NotifyIcon.ContextMenuStrip.Items.Add(quitButton);
            }
            else if (e.Button == MouseButtons.Left)
            {
                Show();
            }

        }

        private void HandleQuit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static string GetRequestData(HttpListenerRequest request)
        {
            Stream body = request.InputStream;
            Encoding encoding = request.ContentEncoding;
            StreamReader reader = new StreamReader(body, encoding);

            string s = reader.ReadToEnd();
            body.Close();
            reader.Close();
            return s;
        }

        public static void UpdatePresence(Response json)
        {  
            RichPresence presence = new RichPresence();

            if (json.Player.Activity == "menu")
                Mode = "In menus";

            if (Steam_ID == null)
                Steam_ID = json.Player.SteamID;


            if (json.Map != null)
            {
                if (Mode == "In menus" && json.Map.Phase != "live")
                {
                    Now = null;
                }
                else
                {
                    if (Now == null)
                        Now = DateTime.UtcNow;
                }
                switch (json.Map.Mode)
                {
                    case "gungameprogressive":
                        Mode = "Arms Race";
                        break;
                    case "gungametrbomb":
                        Mode = "Demolition";
                        break;
                    case "scrimcomp2v2":
                        Mode = "Wingman";
                        break;
                    case "survival":
                        Mode = "Danger Zone";
                        break;
                    default:
                        Mode = char.ToUpper(json.Map.Mode.ToCharArray()[0]) + json.Map.Mode.Substring(1);
                        break;
                }
                switch (json.Map.Name)
                {
                    case "de_cbble":
                        Map = "Cobblestone";
                        break;
                    case "de_stmarc":
                        Map = "St. Marc";
                        break;
                    case "de_dust2":
                        Map = "Dust II";
                        break;
                    case "de_shortnuke":
                        Map = "Nuke";
                        break;
                    default:
                        if (json.Map.Name.StartsWith("workshop"))
                        {
                            WorkShop = true;
                            Map = json.Map.Name.Substring(json.Map.Name.Split('/')[1].Length + json.Map.Name.Split('/')[2].Length + 1);
                        }
                        else
                        {
                            WorkShop = false;
                            Map = char.ToUpper(json.Map.Name.Substring(3).ToCharArray()[0]) + json.Map.Name.Substring(4);
                        }
                        break;
                }
            }

            if (json.Player.Team != null)
            {
                if (json.Player.Team == "CT")
                    TeamName = "Counter-Terrorists";
                else
                    TeamName = "Terrorists";
                if (json.Player.Stats != null)
                {
                    if (json.Player.SteamID == Steam_ID)
                    {
                        string s = "";
                        if (Mode == "Deathmatch" || Mode == "Arms Race")
                        {
                            s = $"Score: {json.Player.Stats.Score}";
                        }
                        else
                        {
                            s = json.Player.Team == "T"
                               ? $"Score: {json.Map.TeamT.Score}:{json.Map.TeamCT.Score}"
                               : $"Score: {json.Map.TeamCT.Score}:{json.Map.TeamT.Score}";
                        }
                        presence.State = $"K: {json.Player.Stats.Kills} / A: {json.Player.Stats.Assists} / D: {json.Player.Stats.Deaths}. {s}";
                    }
                    else
                    {
                        presence.State = $"Spectating. Score: T: {json.Map.TeamT.Score} / CT: {json.Map.TeamCT.Score}";
                    }
                }
                presence.Details = $"Playing {Mode}";
                if (Now != null)
                {
                    presence.Timestamps = new Timestamps()
                    {
                        Start = Now
                    };

                }
                if (!WorkShop)
                {
                    presence.Assets = new Assets()
                    {
                        LargeImageKey = Map.ToLower().Replace(' ', '_'),
                        LargeImageText = Map,
                        SmallImageKey = json.Player.Team.ToLower(),
                        SmallImageText = TeamName
                    };
                }
                else
                {
                    presence.Assets = new Assets()
                    {
                        LargeImageKey = "workshop",
                        LargeImageText = Map,
                        SmallImageKey = json.Player.Team.ToLower(),
                        SmallImageText = TeamName
                    };
                }
                client.SetPresence(presence);
            }
            else if (Mode == "In menus")
            {
                presence.Details = Mode;
                presence.Assets = new Assets()
                {
                    LargeImageKey = "idle",
                    LargeImageText = "In menus"
                };
                presence.Timestamps = new Timestamps()
                {
                    Start = Start
                };
                client.SetPresence(presence);
            }
        }
    }
}
