using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Neat.Guitar;
using UnityEngine.UI;

namespace Neat.Music
{
    public class BeatDivider : MonoBehaviour
    {
        public TextMeshProUGUI tmpBeat;
        public TextMeshProUGUI tmpTime;
        public Image image;


        private RectTransform _rect;
        public RectTransform rect
        {
            get
            {
                if (_rect == null)
                    _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }
        public string beatText
        {
            get { return tmpBeat.text; }
            set { tmpBeat.text = value; }
        }
        public string timeText
        {
            get { return tmpTime.text; }
            set { tmpTime.text = value; }
        }

        public Beat beat { get; private set; }

        // this has to be set to draw position
        public BeatDrawer drawer { get; set; }

        public void UpdateBeat(Beat b)
        {
            this.beat = b;

            UpdateText();
            UpdatePosition();
            UpdateColor();
        }
        private void UpdateText()
        {
            tmpTime.text = beat.timeString;
            tmpBeat.text = beat.measureString;
        }
        private void UpdatePosition()
        {
            rect.anchoredPosition = Vector3.right * beat.time
                * drawer.controller.distancePerSecond;
        }
        private void UpdateColor()
        {
            float alpha = 1 / ((int)beat.beatType + 1);
            var color = new Color(1f, 1f, 1f, alpha);
            image.color = color;
        }
    }
}