using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Neat.Audio.Vizualization
{

    public class BandColorizer : MonoBehaviour
    {
        public VisualizerListener listener;

        [Range(0, 7)]
        public int band;
        public bool useBuffer;
        public Color lerpTo;

        [HideInInspector]
        public Color baseColor;

        private Material mat;

        private void Awake()
        {
            mat = GetComponent<Renderer>().material;
            baseColor = mat.color;
        }
        private void Update()
        {
            UpdateColor();
        }
        private void OnDisable()
        {
            mat.color = baseColor;
        }

        private void UpdateColor()
        {
            mat.color = Color.Lerp(baseColor, lerpTo, useBuffer ? listener.freqBandBuffer[band] : listener.freqBand[band]);
        }
    }
}