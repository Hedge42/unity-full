using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Synthesis
{

    public class Synth : MonoBehaviour
    {
        public List<SynthNote> notes = new List<SynthNote>();

        public WaveGenerator waves;

        [Range(0, 1)]
        public float gain;

        private void OnAudioFilterRead(float[] data, int channels)
        {
            print("Reading " + data.Length + " values & " + channels + "channels");
            for (int i = 0; i < data.Length; i += channels)
            {
                print("data[" + i + "] = " + data[i]);
                data[i] = GetOuput();

                if (channels == 2)
                    data[i + 1] = data[i];
            }
        }
        public float GetOuput()
        {
            float output = 0;

            // possible to throw out-of-range exception with input
            for (int j = 0; j < notes.Count; j++)
            {
                notes[j].UpdatePhase();
                output += gain * waves.Get(notes[j].phase);
            }
            return output;
        }
        public SynthNote NoteOn(int value)
        {
            SynthNote sn = new SynthNote(value);
            print("Note on → " + value);
            notes.Add(sn);
            return sn;
        }
        public void NoteOff(SynthNote note)
        {
            if (notes.Contains(note))
            {
                print("Note off → " + note.value);
                notes.Remove(note);
            }
        }
    }
}
