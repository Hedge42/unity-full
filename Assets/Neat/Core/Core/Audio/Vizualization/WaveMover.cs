using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Audio.Vizualization
{
    public class WaveMover : MonoBehaviour
    {
        public VisualizerListener peer;

        [Range(0, 7)]
        public int band;

        [Range(0, 10)]
        public float range;
        public bool unsigned;

        public bool useBuffer;

        public bool shouldMoveX;
        public bool shouldMoveY;
        public bool shouldMoveZ;

        private float startX;
        private float startY;
        private float startZ;

        private void Awake()
        {
            startX = transform.position.x;
            startY = transform.position.y;
            startZ = transform.position.z;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            MoveX();
            MoveY();
            MoveZ();
        }
        private void MoveX()
        {
            if (shouldMoveX)
            {
                if (useBuffer)
                    transform.localPosition = new Vector3((peer.freqBandBuffer[band] * range) + startX, transform.localPosition.y, transform.localPosition.z);
                else
                    transform.localPosition = new Vector3((peer.freqBand[band] * range) + startX, transform.localPosition.y, transform.localPosition.z);
            }
            else
                transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);
        }
        private void MoveY()
        {
            if (shouldMoveY)
            {
                if (useBuffer)
                    transform.localPosition = new Vector3(transform.localPosition.x, (peer.freqBand[band] * range) + startY, transform.localPosition.z);
                else
                    transform.localPosition = new Vector3(transform.localPosition.x, (peer.freqBandBuffer[band] * range) + startY, transform.localPosition.z);
            }
            else
                transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
        }
        private void MoveZ()
        {
            if (shouldMoveZ)
            {
                if (useBuffer)
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (peer.freqBandBuffer[band] * range) + startZ);
                else
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, (peer.freqBand[band] * range) + startZ);
            }
            else
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startZ);
        }
    }
}
