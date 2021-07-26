using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Audio.Vizualization
{
    public class WaveColorizer : MonoBehaviour
    {
        public bool isEnabled;
        public VisualizerListener peer;

        [Range(0, 7)]
        public int band;

        public Color lerpTo;
        private Color startColor;

        private Material material;

        private void Awake()
        {
            material = GetComponent<Renderer>().material;
            startColor = material.color;
        }

        private void Update()
        {
            if (isEnabled)
                material.color = Color.Lerp(startColor, lerpTo, peer.freqBand[band] / 2);

            else
                material.color = startColor;
        }
    }
}
