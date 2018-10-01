using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using DiscordRPC;
using DiscordRPC.Logging;

namespace Counter_Strike_Presence
{
    class Program
    {
        static bool WorkShop;
        static string Steam_ID;
        static string Map;
        static string TeamName;
        static string Mode;
        static dynamic Now;
        public static DiscordRpcClient client;
        static HttpListener listener = new HttpListener();
        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            client = new DiscordRpcClient("494943194165805082", true, 0);
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning, Colored = true };
            client.Initialize();

            Console.WriteLine("Ready! You can minimize me now.");
            while (true)
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://127.0.0.1:2348/");
                listener.Start();
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                dynamic JSON = JObject.Parse(GetRequestData(request));
                UpdatePresence(JSON);
                string responseString = "";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
                listener.Stop();
            }
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

        public static void UpdatePresence(dynamic json)
        {
            RichPresence presence = new RichPresence();
           // Console.WriteLine(json);

            if (json.player.activity == "menu")
                Mode = "In menus";

            if (Steam_ID == null)
                Steam_ID = json.player.steamid;


            if (json.map != null)
            {
                if (Mode == "In menus" && json.map.phase.ToString() != "live")
                {
                    Now = null;
                }
                else
                {
                    if (Now == null)
                        Now = DateTime.UtcNow;
                }
                switch (json.map.mode.ToString())
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
                    default:
                         Mode = char.ToUpper(json.map.mode.ToString().ToCharArray()[0]) + json.map.mode.ToString().Substring(1);
                        break;
                }
                switch (json.map.name.ToString())
                {
                    case "de_cble":
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
                        if (json.map.name.ToString().StartsWith("workshop"))
                        {
                            WorkShop = true;
                            Map = json.map.name.ToString().Substring(json.map.name.ToString().Split('/')[1].Length + json.map.name.ToString().Split('/')[2].Length + 1);
                        }
                        else
                        {
                            WorkShop = false;
                            Map = char.ToUpper(json.map.name.ToString().Substring(3).ToCharArray()[0]) + json.map.name.ToString().Substring(4);
                        }
                        break;
                }
            }

            if (json.player.team != null)
            {
                if (json.player.team.ToString() == "CT")
                    TeamName = "Counter-Terrorists";
                else
                    TeamName = "Terrorists";
                if (json.player.match_stats != null)
                {
                    if (json.player.steamid == Steam_ID)
                    {
                        string s = json.player.team.ToString() == "T"
                            ? $"Score: {json.map.team_t.score}:{json.map.team_ct.score}"
                            : $"Score: {json.map.team_ct.score}:{json.map.team_t.score}";
                        presence.State = $"K: {json.player.match_stats.kills} / A: {json.player.match_stats.assists} / D: {json.player.match_stats.deaths}. {s}";
                    }
                    else {
                        presence.State = $"Spectating. Score: T: {json.map.team_t.score} / CT: {json.map.team_ct.score}";
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
                        SmallImageKey = json.player.team.ToString().ToLower(),
                        SmallImageText = TeamName
                    };
                }
                else
                {
                    presence.Assets = new Assets()
                    {
                        LargeImageKey = "workshop",
                        LargeImageText = Map,
                        SmallImageKey = json.player.team.ToString().ToLower(),
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
                client.SetPresence(presence);
            }
        }
    }
}
