using System.Collections;
using System.Collections.Generic;
using Neat.FileManagement;

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

        public string musicPath;

        public TimingMap timingMap;

        // obsolete
        public TimeSignature[] timeSignatures
        {
            get
            {
                return timingMap.timeSignatures.ToArray();
            }
        }

        public Chart()
        {
            name = "name";
            timingMap = new TimingMap();
            path = "";
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
