using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Neat.Music
{
    public class FretboardMouseHandler : MonoBehaviour
    {
        public Fretboard ui { get; private set; }

        private void Awake()
        {
            ui = GetComponent<Fretboard>();
        }

        private void Update()
        {
            var mouse = Input.mousePosition;

            var inverse = ui.panel.InverseTransformPoint(mouse);
            if (ui.panel.rect.Contains(inverse))
            {
                print("Hovering!");

                // which fret...
            }
        }
    }
}