using System.Collections.Generic;

namespace Neat.Music
{
    public class Note
    {
        public Timing timing;
        public int value;

        public Note(int value)
        {
            this.value = value;
        }
    }
}
