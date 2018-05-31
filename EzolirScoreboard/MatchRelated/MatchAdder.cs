using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzolirScoreboard.MatchRelated
{
    class MatchAdder
    {

        public List<Match.Participant> regularGame;

        public List<Match.Participant> doubleGame;

        private static MatchAdder inct;

        public static MatchAdder get
        {
            get
            {
                if (inct == null)
                    inct = new MatchAdder();
                return inct;
            }
        }
        
        public void restartRegular()
        {
            regularGame = new List<Match.Participant>();
        }

        public void restartDouble()
        {
            doubleGame = new List<Match.Participant>();
        }

        private MatchAdder()
        {
            regularGame = new List<Match.Participant>();
            doubleGame = new List<Match.Participant>();
        }

    }
}
