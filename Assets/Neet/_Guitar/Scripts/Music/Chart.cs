using System.Collections;
using System.Collections.Generic;
using Neat.File;

namespace Neat.Guitar
{
    [System.Serializable]
    public class Chart
    {
        // data
        public string name;
        public float duration;
        public string path;

        public TimeSignature[] timeSignatures;

        public Chart()
        {
            name = "name";
            duration = 15; // seconds

            timeSignatures = new TimeSignature[] { new TimeSignature() };
        }

        void GetNotesBetween(float a, float b) { }
        void GetNotesStartingAt(float time) { }
        void GetNotesActiveAt(float time) { }
        void GetBarsBetween(float a, float b, int division) { }

        public void Save()
        {
            FileManager.SerializeBinary(this, path);
        }
    }
}
