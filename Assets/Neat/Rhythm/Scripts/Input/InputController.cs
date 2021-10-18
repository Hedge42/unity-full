using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.States;

namespace Neat.Music
{
    public class InputController : MonoBehaviour
    {
        public new bool enabled;
        public InputState state { get; private set; }
        public bool hasInput => state != null;

        public void GetInput()
        {
            if (hasInput)
                state.GetInput();
        }

        public void SetInputState(InputState s)
        {
            state = s;
        }

        private void idk()
        {
            // make chart input
            new ChartPlayerInput(GetComponent<ChartPlayer>());

            // new KeyOverlayInput(GetComponent<KeyOverlayInput>());
            // new NoteInputHandler(GetComponent<NoteInputHandler>());
        }
    }
}