using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Audio.Vizualization
{
    public class VolumeColorizer : MonoBehaviour
    {
        public VisualizerListener peer;

        public bool modifyColor;
        public bool useBuffer;

        public Color lerpTo;

        private Color startColor;
        private Material mat;

        private void Awake()
        {
            mat = GetComponent<Renderer>().material;
            startColor = mat.color;
        }
        private void Update()
        {
            LerpColor();
        }
        private void OnDisable()
        {
            mat.color = startColor;
        }

        private void LerpColor()
        {
            mat.color = useBuffer ? Color.Lerp(startColor, lerpTo, peer.amplitudeBufferred) : Color.Lerp(startColor, lerpTo, peer.amplitude);
        }
    }
}