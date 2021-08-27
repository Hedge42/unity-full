using System.Collections;
using System.Collections.Generic;
using Neat.File;

namespace Neat.Guitar
{
    [System.Serializable]
    public class Chart
    {
        public static string directory
        {
            get
            {
                return "C:/Users/tyler/Desktop/Charts/";
            }
        }
        public static string ext
        {
            get
            {
                return ".chart";
            }
        }

        // data
        public string name;
        public float duration;
        public string path;

        public TimingMap timingMap;

        public TimingMap.TimeSignature[] timeSignatures
        {
            get
            {
                return timingMap.timeSignatures.ToArray();
            }
        }

        public Chart()
        {
            name = "name";
            duration = 15; // seconds

            timingMap = new TimingMap();
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
