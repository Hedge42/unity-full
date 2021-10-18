using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    public class FretClickHandler : MonoBehaviour
    {
        // use EventTrigger
        public FretUI fret { get; private set; }

        private void Awake()
        {
            fret = GetComponent<FretUI>();
        }

        public void OnClick()
        {
            print("hello");
        }
    }
}
