using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Audio.Vizualization
{

    public class BandScaler : MonoBehaviour
    {
        public VisualizerListener listener;

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

        [HideInInspector]
        public Vector3 baseScale;

        #region Mobobehavior
        private void Awake()
        {
            baseScale = transform.localScale;
        }
        private void Update()
        {
            Scale();
        }
        #endregion


        #region Methods
        private void Scale()
        {
            float targetX;
            float targetY;
            float targetZ;

            float scaleAmount = useBuffer ? listener.freqBandBuffer[band8] * range : listener.freqBand[band8] * range;

            targetX = shouldScaleX ? scaleAmount + baseScale.x : baseScale.x;
            targetY = shouldScaleY ? scaleAmount + baseScale.y : baseScale.y;
            targetZ = shouldScaleZ ? scaleAmount + baseScale.z : baseScale.z;

            transform.localScale = new Vector3(targetX, targetY, targetZ);
        }
        #endregion
    }
}