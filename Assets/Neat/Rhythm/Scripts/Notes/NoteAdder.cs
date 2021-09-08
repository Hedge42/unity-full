using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public class NoteAdder : MonoBehaviour
    {
        public ChartController controller;

        public Transform overlayContainer;
        public Transform trackContainer;

        public TimingBar timing;

        public NoteUI[] uis;

        public void StandardTuning()
        {
            var tuning = new GuitarTuning();

            List<Note> _notes = new List<Note>();
            foreach (int note in tuning.stringValues)
                _notes.Add(new Note(note));
            Note[] inputs = _notes.ToArray();
        }
        public void CreateKeys()
        {

        }
    }
}
