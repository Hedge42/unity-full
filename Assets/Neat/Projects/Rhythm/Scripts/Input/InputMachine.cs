using Neat.States;
using System;
using UnityEngine;

namespace Neat.Audio.Music
{
    public class InputMachine : MonoBehaviour
    {
        public InputState state;

        private void Update()
        {
            state.GetInput();
        }
    }
}
