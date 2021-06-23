using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Audio.Vizualization
{
    public class VolumeScaler : MonoBehaviour
    {
        public VisualizerListener peer;

        public bool scaleX;
        public bool scaleY;
        public bool scaleZ;
        public bool useBuffer;

        [Range(-2f, 2f)]
        public float range;

        private Vector3 startScale;

        private void Awake()
        {
            startScale = transform.localScale;
        }
        private void Update()
        {
            ScaleX();
            ScaleY();
            ScaleZ();
        }
        private void OnDisable()
        {
            transform.localScale = startScale;
        }

        private void ScaleX()
        {
            if (scaleX)
            {
                float newX = 1f;
                if (useBuffer)
                    newX = startScale.x + (range * peer.amplitudeBufferred);
                else
                    newX = startScale.x + (range * peer.amplitude);

                transform.localScale = new Vector3(newX, transform.localScale.y, transform.localScale.z);
            }
            else
                transform.localScale = new Vector3(startScale.x, transform.localScale.y, transform.localScale.z);
        }
        private void ScaleY()
        {
            if (scaleY)
            {
                float newY = 1f;
                if (useBuffer)
                    newY = startScale.y + (range * peer.amplitudeBufferred);
                else
                    newY = startScale.y + (range * peer.amplitude);

                transform.localScale = new Vector3(transform.localScale.x, newY, transform.localScale.z);
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, startScale.y, transform.localScale.z);
        }
        private void ScaleZ()
        {
            if (scaleZ)
            {
                float newZ = 1f;
                if (useBuffer)
                    newZ = startScale.z + (range * peer.amplitude);
                else
                    newZ = startScale.z + (range * peer.amplitude);

                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZ);
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startScale.z);
        }
    }
}