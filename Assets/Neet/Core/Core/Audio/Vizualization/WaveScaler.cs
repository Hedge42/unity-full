using UnityEngine;

namespace Neat.Audio.Vizualization
{
    public class WaveScaler : MonoBehaviour
    {
        public VisualizerListener peer;

        [Range(0, 7)]
        public int band8;
        [Range(0, 63)]
        public int band64;

        [Range(-5, 5)]
        public float range;

        public bool useBuffer;
        public bool use64;

        public bool shouldScaleX;
        public bool shouldScaleY;
        public bool shouldScaleZ;

        private float startX;
        private float startY;
        private float startZ;

        #region Mobobehavior
        private void Awake()
        {
            startX = transform.localScale.x;
            startY = transform.localScale.y;
            startZ = transform.localScale.z;
        }
        private void Update()
        {
            Scale();


            DetectMinMax();
        }
        #endregion


        #region Methods
        private void Scale()
        {
            ScaleX();
            ScaleY();
            ScaleZ();
        }

        private void ScaleX()
        {
            if (shouldScaleX)
            {
                if (use64)
                {
                    if (useBuffer)
                        transform.localScale = new Vector3((peer.freqBandBuffer64[band64] * range) + startX, transform.localScale.y, transform.localScale.z);
                    else
                        transform.localScale = new Vector3((peer.freqBand64[band64] * range) + startX, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    if (useBuffer)
                        transform.localScale = new Vector3((peer.freqBandBuffer[band8] * range) + startX, transform.localScale.y, transform.localScale.z);
                    else
                        transform.localScale = new Vector3((peer.freqBand[band8] * range) + startX, transform.localScale.y, transform.localScale.z);
                }
            }
            else
                transform.localScale = new Vector3(startX, transform.localScale.y, transform.localScale.z);
        }
        private void ScaleY()
        {
            if (shouldScaleY)
            {
                if (use64)
                {
                    if (useBuffer)
                        transform.localScale = new Vector3(transform.localScale.x, (peer.freqBandBuffer64[band64] * range) + startY, transform.localScale.z);
                    else
                        transform.localScale = new Vector3(transform.localScale.x, (peer.freqBand64[band64] * range) + startY, transform.localScale.z);

                }
                else
                {
                    if (useBuffer)
                        transform.localScale = new Vector3(transform.localScale.x, (peer.freqBandBuffer[band8] * range) + startY, transform.localScale.z);
                    else
                        transform.localScale = new Vector3(transform.localScale.x, (peer.freqBand[band8] * range) + startY, transform.localScale.z);
                }
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, startY, transform.localScale.z);

        }
        private void ScaleZ()
        {
            if (shouldScaleZ)
            {
                if (use64)
                {
                    if (useBuffer)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (peer.freqBandBuffer64[band64] * range) + startZ);
                    else
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (peer.freqBand64[band64] * range) + startZ);
                }
                else
                {
                    if (useBuffer)
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (peer.freqBandBuffer[band8] * range) + startZ);
                    else
                        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (peer.freqBand[band8] * range) + startZ);
                }
            }
            else
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, startZ);
        }


        #region Debugging
        private float min = 1;
        private float max = 0;
        private void DetectMinMax()
        {
            if (peer.freqBand[band8] > max)
            {
                max = peer.freqBand[band8];
                // print("Max: " + max);
            }

            else if (peer.freqBand[band8] < min)
            {
                min = peer.freqBand[band8];
                // print("Min " + min);
            }
        }
        #endregion
        #endregion
    }
}
