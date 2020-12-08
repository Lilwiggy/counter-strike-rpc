
namespace CSGO_Presence.types
{
    public class Response
    {
        public Map Map;
        public Player Player;
        public Previously Previously;
        public Round Round;
        public Added Added;
    }

    public class TeamCT
    {
        public int Score;
        public int ConsecutiveRoundLosses;
        public int TimeoutsRemaining;
        public int MatchesWonThisSeries;
    }

    public class TeamT
    {
        public int Score;
        public int ConsecutiveRoundLosses;
        public int TimeoutsRemaining;
        public int MatchesWonThisSeries;
    }

    public class Map
    {
        public string Mode;
        public string Name;
        public string Phase;
        public int Round;
        public int NumMatchesToWinSeries;
        public int CurrentSpectators;
        public int SounvenirsTotal;
        public TeamCT TeamCT;
        public TeamT TeamT;
    }

    public class MatchStats
    {
        public int Kills;
        public int Assists;
        public int Deaths;
        public int MVPs;
        public int Score;
    }

    public class Player
    {
        public string SteamID;
        public string Name;
        public string Activity;
        public MatchStats Stats;
        public bool MatchStats;
        public int ObserverSlot;
        public string Team;
        public string Clan;
    }
    // The `previously` response has properties that can have different types so this is what I am settling on for now
    public class Previously 
    {
        public object Map;
        public object Round;
        public object Player;
    }

    public class Round
    {
        public string Phase;
    }

    public class Added
    {
        public Player Player;
    }
}
