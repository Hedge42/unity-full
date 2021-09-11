using System;
using System.Collections.Generic;

namespace Neat.Music
{
    [Serializable]
    public class Track // is a noteMap
    {
        public string instrument = "keyboard";

        public GuitarTuning tuning = new GuitarTuning(); // standard

        public List<TrackNote> notes = new List<TrackNote>();
        public List<TrackNote> GetNotes(float startTime, float endTime)
        {
            var list = new List<TrackNote>();
            foreach (TrackNote n in notes)
                if (n.timeSpan.on >= startTime || n.timeSpan.off <= endTime)
                    list.Add(n);
            return list;
        }
    }
}
