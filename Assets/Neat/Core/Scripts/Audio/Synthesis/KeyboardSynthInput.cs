using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Synthesis
{
    public class KeyboardSynthInput : MonoBehaviour
    {
        // map KeyCode → int

        [Serializable]
        public class InputKey
        {
            public KeyCode key;
            public int value;
            public SynthNote playing;
        }

        public List<InputKey> map = new List<InputKey>();

        public Synth synth;

        private void Awake()
        {
            synth = GetComponent<Synth>();
        }
        private void Update()
        {
            GetInput();
        }

        public void GetInput()
        {
            foreach (InputKey k in map)
            {
                if (Input.GetKeyDown(k.key))
                    k.playing = synth.NoteOn(k.value);
                else if (Input.GetKeyUp(k.key))
                    synth.NoteOff(k.playing);
            }
        }
    }
}
