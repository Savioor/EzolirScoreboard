using EzolirScoreboard.MatchRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzolirScoreboard
{
    class GlobalData
    {

        public List<Team> teams { get; }

        public List<Match> matches { get; }

        public DisplayToCrowd display { get; }

        private static GlobalData inct;

        public static GlobalData get { get {
                if (inct == null)
                    inct = new GlobalData();
                return inct;
            } }

        private GlobalData()
        {
            teams = new List<Team>();
            matches = new List<Match>();
            display = new DisplayToCrowd();
        }

        public Team getTeamByNumber(uint number)
        {
            return teams.Find((team) => team.number == number);
        }

        public Team getTeamByName(String name)
        {
            return teams.Find((team) => team.name.Equals(name));
        }

        public Match getMatchByUId(int id)
        {
            return matches.Find((m) => m.uID == id);
        }

        public List<Team> getRankedTeams()
        {
            List<Team> ret = new List<Team>();
            ret.AddRange(teams);
            ret.RemoveAll((a) => a.number == 0);
            ret.Sort();
            ret.Reverse();
            return ret;
        }

        public void addNewMatch(List<Match.Participant> part, uint track)
        {
            Match newMatch = new Match(part, track, matches.Count > 0 ? matches[matches.Count - 1].uID + 1 : 1);
            foreach(Match.Participant p in part)
            {
                p.getFirstTeam().appearIn.Add(newMatch);
                if (track == 3)
                {
                    p.getSecondTeam().appearIn.Add(newMatch);
                }
            }
            matches.Add(newMatch);
        }

    }
}
