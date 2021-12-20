using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Audio.Music
{
    public class NoteReader
    {
        private List<Note> active;
        private ChartPlayer player;

        private Note waiting;

        public NoteReader(ChartPlayer player)
        {
            this.player = player;
        }

        // find notes
        public void SetTime(float newTime)
        {
            active = player.noteMap.GetNotes(player.timer.fullTimespan);
        }

        // update notes
        public void UpdateTime(float newTime)
        {
            // removes notes that are already off
            while (active.Count > 0 && active[0].timeSpan.off < player.time)
                active.RemoveAt(0);

            // spawn notes that are now visible
            while (HasNext(out Note next))
                active.Add(next);
        }
        private bool HasNext(out Note next)
        {
            next = player.noteMap.Next(active[active.Count - 1]);
            bool hasNext = next != null;
            bool spawnNext = hasNext && next.timeSpan.on < player.timer.maxTime;
            return spawnNext;
        }
    }
}
