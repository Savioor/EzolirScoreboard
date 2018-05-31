using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzolirScoreboard.MatchRelated
{
    class ScoresObject : IComparable<ScoresObject>
    {

        public uint place { get; }
        public uint gates { get; }
        public TimeSpan time { get; }

        public ScoresObject(uint place, uint gates, TimeSpan span)
        {
            this.place = place;
            this.gates = gates;
            time = span;
        }

        public int CompareTo(ScoresObject other)
        {
            if (place > other.place)
                return 1;
            if (place < other.place)
                return -1;
            if (time.TotalMilliseconds > other.time.TotalMilliseconds)
                return 1;
            if (this.time.TotalMilliseconds < other.time.TotalMilliseconds)
                return -1;
            return 0;
        }

    }
}
