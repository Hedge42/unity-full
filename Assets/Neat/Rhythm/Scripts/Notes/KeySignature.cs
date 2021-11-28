using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Audio.Music
{
    [Serializable]
    public class KeySignature
    {
        public float time;
        public MusicScale scale = new MusicScale();

        public KeySignature next;
        public KeySignature prev;
    }
}
