using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Music
{
    // not implemented, but should contain this logic
    public class ChartTimer
    {
        private ChartPlayer player;
        public ChartTimer(ChartPlayer _player)
        {
            player = _player;
        }

        // temp
        public NoteHighway highway => player.ui.scroller;
        public float approachRate => highway.approachRate;

        public float maxTime
        {
            get
            {
                return player.time + approachRate;
            }
        }
        public float minTime
        {
            get
            {
                // time at the beginning of the window (not the judgementline)
                return player.time - (highway.judgementLine.anchoredPosition.x / highway.distancePerSecond);
            }
        }
        public TimeSpan judgementTimespan
        {
            get
            {
                return new TimeSpan(player.time, maxTime);
            }
        }
        public TimeSpan fullTimespan
        {
            get
            {
                return new TimeSpan(minTime, maxTime);
            }
        }
    }
}
