using System;
using System.Collections.Generic;
using UnityEngine;
using Neat.FileManagement;

namespace Neat.Music
{
    public class FretboardSerializer : MonoBehaviour
    {
        public readonly string path = "C:/Users/tyler/Desktop/Fretboards";

        public Fretboard fretboard { get; private set; }

        public new string name;

        private void Awake()
        {
            fretboard = GetComponent<Fretboard>();
        }

        public void Save()
        {
            // FileManager.SerializeBinary(this, );
        }
        public static void Load()
        {

        }
    }
}
