using System.Collections;
using System.Collections.Generic;
using Neat.GameManager;
using UnityEngine;

namespace Neat.Audio.Music
{
    [System.Serializable]
    public class Chart : _IFile
    {
        // naming
        public string filePath;
        public string musicPath;
        public string name;

        public Chart()
        {
            Initialize();
        }

        [SerializeField] private TimingMap _timingMap;
        public TimingMap timingMap
        {
            get
            {
                if (_timingMap == null)
                    _timingMap = new TimingMap();
                return _timingMap;
            }
        }

        private KeyMap _keyMap;
        public KeyMap keyMap
        {
            get
            {
                if (_keyMap == null)
                    _keyMap = new KeyMap();
                return _keyMap;
            }
        }

        [SerializeField] private List<NoteMap> _noteMaps;
        public List<NoteMap> noteMaps
        {
            get
            {
                if (_noteMaps == null)
                    _noteMaps = new List<NoteMap>();
                return _noteMaps;
            }
        }

        public void Initialize()
        {
            if (_noteMaps == null)
                _noteMaps = new List<NoteMap>();
            if (_noteMaps.Count == 0)
                _noteMaps.Add(new NoteMap(new GuitarTuning()));

            if (_keyMap == null)
                _keyMap = new KeyMap();
            if (_keyMap.signatures.Count == 0)
                _keyMap.Add(new KeySignature());

            if (_timingMap == null)
                _timingMap = new TimingMap();
            if (_timingMap.signatures.Count == 0)
                _timingMap.Add(new TimeSignature(0f));
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            // name
            sb.Append(name + " info:");

            // music path
            sb.Append("\nMusic path: " + musicPath);

            // file path
            sb.Append("\nFile path: " + filePath);

            // # maps
            sb.Append("\nInstrument count: " + noteMaps.Count);
            foreach (var m in noteMaps)
            {
                var inst = "\n  " + m.instrument + " (" + m.notes.Count + " notes)";
                sb.Append(inst);
                sb.Append("\n  Tuning: " + m.tuning.ToString());
            }

            // # key signatures
            var keyCount = keyMap.signatures.Count;
            sb.Append("\nKeycount: " + keyCount);
            foreach (var k in keyMap.signatures)
            {
                var s = k.scale.ToString();
                var time = k.time.ToString("f3");
                sb.Append("\n  " + s + " @" + time);
            }

            // # time signatures
            var timeCount = timingMap.signatures.Count;
            sb.Append("\nTimecount: " + timeCount);
            foreach (var t in timingMap.signatures)
            {
                sb.Append("\n  " + t.ToString());
            }

            // Debug.Log(sb.ToString());
            return sb.ToString();
        }

        public string FilePath()
        {
            return filePath;
            // throw new System.NotImplementedException();
        }
    }
}
