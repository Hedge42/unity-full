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
    }
}