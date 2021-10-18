using Neat.States;
using System;
using UnityEngine;

namespace Neat.Music
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
