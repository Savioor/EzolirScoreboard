using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzolirScoreboard.MatchRelated;

namespace EzolirScoreboard
{
    class Team : IComparable<Team>
    {

        public String name { get; }
        public uint number { get; }
        public List<Match> appearIn { get; }
        public uint score { get
            {
                uint ret = 0;
                foreach(Match m in appearIn)
                {
                    ret += (4 - m.getParticipantsByTeam(this).score.place) * 5;
                }
                return ret + gatePoints;
            } }
        public uint gatePoints { get
            {
                uint ret = 0;
                foreach (Match m in appearIn)
                {
                    ret += m.getParticipantsByTeam(this).score.gates;
                }
                return ret;
            } }
        public uint bonusPoints { get; set; }


        public Team(String teamName, uint teamNumber)
        {
            name = teamName;
            number = teamNumber;
            appearIn = new List<Match>();
            bonusPoints = 0;
        }

        public int CompareTo(Team other)
        {
            uint thisTotal = this.score;
            uint otherTotal = other.score;
            if (thisTotal > otherTotal)
                return 1;
            if (thisTotal < otherTotal)
                return -1;
            if (this.gatePoints > other.gatePoints)
                return 1;
            if (this.gatePoints < other.gatePoints)
                return -1;
            if (this.bonusPoints > other.bonusPoints)
                return 1;
            if (this.bonusPoints < other.bonusPoints)
                return -1;
            return 0;
        }

    }
}
