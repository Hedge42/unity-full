using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neat.Music
{
    [Serializable]
    public class NoteMap
    {
        public string instrument = "keyboard";
        public float approachRate;

        public GuitarTuning tuning = new GuitarTuning(); // standard tuning
        public List<Note> notes = new List<Note>(); // better data structure?

        public event Action onEdit;

        public NoteMap()
        {
            instrument = "Keys";
        }
        public NoteMap(GuitarTuning tuning)
        {
            this.instrument = "Guitar";
            this.tuning = tuning;
        }

        public bool Add(Note newNote)
        {
            // does this note overlap with any existing note
            foreach (Note n in notes)
            {
                if (n.Overlaps(newNote))
                {
                    Debug.Log("Couldn't add overlapping note");
                    return false;
                }
            }

            // if not, add it to the list
            notes.Add(newNote);
            Sort();
            SetIDs();

            onEdit?.Invoke();
            return true;
        }
        public Note Remove(Note removeMe)
        {
            foreach (Note n in notes)
            {
                if (n.Equals(removeMe))
                {
                    notes.Remove(n);

                    Sort();
                    SetIDs();
                    onEdit?.Invoke();
                    return n;
                }
            }

            Debug.Log("Note not found for removal");
            return null;
        }
        public List<Note> GetNotes(TimeSpan timeSpan)
        {
            return GetNotes(timeSpan.on, timeSpan.off);
        }
        private List<Note> GetNotes(float startTime, float endTime)
        {
            var list = new List<Note>();
            foreach (Note n in notes)
                if (n.timeSpan.on >= startTime || n.timeSpan.off <= endTime)
                    list.Add(n);
            return list;
        }

        private void Sort()
        {
            // TODO optimize me
            bool sorted = false;
            while (!sorted)
            {
                bool flag = false;
                for (int i = 0; i < notes.Count - 1; i++)
                {
                    // sort by on-time
                    if (notes[i].timeSpan.on > notes[i + 1].timeSpan.on)
                    {
                        // swap
                        var temp = notes[i];
                        notes[i] = notes[i + 1];
                        notes[i + 1] = temp;

                        flag = true;
                    }
                }

                // break if no swaps occured
                if (!flag)
                    sorted = true;
            }
        }
        private void SetIDs()
        {
            for (int i = 0; i < notes.Count; i++)
                notes[i].id = i;
        }
        public Note Next(Note n)
        {
            // TODO optimize me
            bool hasNext = n.id < notes.Count - 1;
            if (hasNext)
                return notes[n.id + 1];

            // for each note index
            for (int i = 0; i < notes.Count - 1; i++)
            {
                // index found
                if (notes[i].Equals(n))
                {
                    // return at next index
                    return notes[i + 1];
                }
            }

            return null;
        }
        public Note Next(float from)
        {
            foreach (Note n in notes)
            {
                if (n.timeSpan.on >= from)
                {
                    return n;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return instrument;
        }
    }
}
