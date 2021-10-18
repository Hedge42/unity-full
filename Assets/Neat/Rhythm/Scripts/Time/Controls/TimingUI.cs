using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Neat.Music;
using UnityEngine.UI;
using Neat.States;

namespace Neat.Music
{
    // Timing represented as a GameObject
    // relative to the time signature
    public class TimingUI : MonoBehaviour
    {
        public TextMeshProUGUI tmpBeat;
        public Image image;

        private TimeSignatureUI tsm;


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

        public Timing beat { get; private set; }

        // this has to be set to draw position
        public ChartPlayer controller { get; set; }

        public void UpdateBeat(Timing b)
        {
            this.beat = b;

            UpdateText();
            UpdatePosition();
            UpdateColor();
            HandleTimeSignature();
        }
        private void UpdateText()
        {
            tmpBeat.text = beat.timeString + "\n" + beat.measureString;
        }
        private void UpdatePosition()
        {
            //rect.localPosition = controller.ui.scroller.GetLocalPosition(beat.time); // TODO: spacing class?
            rect.anchoredPosition = controller.ui.scroller.GetLocalPosition(beat.time); // TODO: spacing class?
        }
        private void UpdateColor()
        {
            float alpha = 1f / (float)((int)beat.beatType + 1);
            alpha = Mathf.Lerp(.4f, 1f, alpha);
            var color = new Color(1f, 1f, 1f, alpha);
            image.color = color;
        }
        private void HandleTimeSignature()
        {
            if (tsm != null)
            {
                if (Application.isPlaying)
                    Destroy(tsm);
                else
                    DestroyImmediate(tsm);
                tsm = null;
            }

            if (beat.isSignatureStart)
            {
                GameObject p = controller.ui.timeSignaturePrefab;
                tsm = TimeSignatureUI.Create(p, this.transform, beat.signature, controller);
            }
        }
    }
}