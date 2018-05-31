using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzolirScoreboard.MatchRelated
{
    class Match // TODO
    {
        
        public int uID { get; }
        public uint trackID { get; }
        public bool isDouble { get { return trackID == 3; } }
        private bool scoresSet = false;
        public List<Participant> participants { get; }

        public bool areScoresSet() { return scoresSet; }

        public Match(List<Participant> part, uint id, int uID)
        {
            participants = part;
            trackID = id;
            this.uID = uID;
        }

        public void setScores(List<ScoresObject> scores)
        {
            if (scores.Count != participants.Count)
                throw new Exception("Scha rega");
            for (int i = 0; i < scores.Count; i++)
            {
                participants[i].score = scores[i];
            }
            scoresSet = true;
        }

        public List<Participant> getParticipantsInOrder()
        {
            if (!scoresSet)
                return participants;
            List<Participant> ret =  new List<Participant>();
            ret.AddRange(participants);
            ret.Sort();
            return ret;
        }

        public Participant getWinner()
        {
            return getParticipantsInOrder()[0];
        }

        public Participant getParticipantsByTeam(Team t)
        {
            foreach(Participant par in participants)
            {
                if (par.getFirstTeam().number == t.number)
                {
                    return par;
                }
                if (isDouble)
                {
                    if (par.getSecondTeam().number == t.number)
                        return par;
                }
            }
            return null;
        }

        public class Participant : IComparable<Participant>
        {
            private Team teammate1;
            private Team teammate2;

            public Team getFirstTeam()
            {
                return teammate1;
            }
            public Team getSecondTeam()
            {
                if (teammate2 == null)
                    throw new NullReferenceException("Only the third track has two teammates");
                return teammate2;
            }

            public ScoresObject score;

            public Participant(Team t1, Team t2 = null)
            {
                teammate1 = t1;
                teammate2 = t2;
                score = new ScoresObject(4, 0, new TimeSpan(0));
            }

            public override string ToString()
            {
                return teammate1.number.ToString() + (teammate2 == null ? "" : "&" + teammate2.number.ToString());
            }

            public string labelToString()
            {
                return teammate1.number.ToString() + (teammate2 == null ? "" : "&&" + teammate2.number.ToString());
            }

            public int CompareTo(Participant other)
            {
                return this.score.CompareTo(other.score);
            }
        }



    }
}
