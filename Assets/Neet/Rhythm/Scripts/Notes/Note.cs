using System.Collections.Generic;

namespace Neat.Music.Notes
{
    [System.Serializable]
    public class Composition
    {
        public TimingMap timing;

        public List<Track> tracks;
    }
    [System.Serializable]
    public class Track
    {
        public string name;
        //public Composition composition; // parent ref

        //public NoteMap notes;
    }
    public class KeySignature
    {
        public Note key;
    }
    public class Note
    {
        public KeySignature signature;
        public Timing timing;
    }
}
