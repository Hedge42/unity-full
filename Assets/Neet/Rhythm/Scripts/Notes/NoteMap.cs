using System.Collections.Generic;
using System.Collections;

namespace Neat.Music.Notes
{
    public class NoteMap
    {
        List<Note> notes;

        List<Music.Timing> displayed;

        public void Assign(List<Music.Timing> beats)
        {
            foreach (var n in notes)
            {
                foreach (var b in beats)
                {
                    //if (n.beat.Equals(b))
                    //{
                    //    // ???
                    //}
                }
            }
        }
    }
}
