using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.States;

namespace Neat.Audio.Music
{
    // this class is so dumb
    public class ChartUI : MonoBehaviour
    {
        // data
        public GameObject timeSignaturePrefab;
        public NoteUI notePrefab;
        private NoteHighway _scroller;
        private TimingSpawner _timingBar;

        // properties
        public NoteHighway scroller
        {
            get
            {
                if (_scroller == null)
                    _scroller = GetComponent<NoteHighway>();
                return _scroller;
            }
        }
        public TimingSpawner timingBar
        {
            get
            {
                if (_timingBar == null)
                    _timingBar = GetComponent<TimingSpawner>();
                return _timingBar;
            }
        }


    }
}