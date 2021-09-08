using System.Collections.Generic;
using System.Collections;

namespace Neat.Music
{
    public class NoteMap
    {
        public List<Note> notes = new List<Note>();

        public List<Note> GetNotes(float startTime, float endTime)
        {
            var list = new List<Note>();
            foreach (Note n in notes)
                if (n.on >= startTime || n.off <= endTime)
                    list.Add(n);
            return list;
        }
    }
}
