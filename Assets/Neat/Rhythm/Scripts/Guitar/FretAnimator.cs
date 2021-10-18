using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{

    public class FretAnimator : MonoBehaviour
    {
        // Animation States:
        // Incoming
        enum AnimState
        {
            Hidden,
            Visible, // Passive ???
            Incoming,
            Playing,
            Stopping, // ????
        }

        public AnimationCurve fadeInCurve;
        public AnimationCurve fadeOutCurve;

        [Range(0f, 2f)]
        public float fadeInTime;
        [Range(0f, 2f)]
        public float fadeOutTime;

        public Color incomingColor;
        public Color outgoingColor;
        public Color playingColor;

        [Range(0, 20)]
        public float fadeInScale;
        [Range(0, 20)]
        public float fadeOutScale;

        public bool hideOnFinish;

        private Color textStartColor;
        private Color fillStartColor;

        private FretUI _ui;
        private FretUI ui
        {
            get
            {
                if (_ui == null)
                    _ui = GetComponent<FretUI>();
                return _ui;
            }
        }

        private void Start()
        {
            textStartColor = ui.tmp.color;
        }

        public void SetAnimationState(Note n, ChartPlayer p)
        {
            fadeInTime = p.timer.approachRate;
            fadeOutTime = Mathf.Abs(p.time - p.timer.minTime); // ????

            float time = p.time;
            float minTime = n.timeSpan.on - fadeInTime;
            float maxTime = n.timeSpan.off + fadeOutTime;



            TimeSpan fullSpan = new TimeSpan(minTime, maxTime);
            TimeSpan fadeInSpan = new TimeSpan(minTime, n.timeSpan.on);
            TimeSpan activeSpan = new TimeSpan(n.timeSpan.on, n.timeSpan.off); // ????? 
            TimeSpan fadeOutSpan = new TimeSpan(n.timeSpan.off, maxTime);

            // incoming
            if (fadeInSpan.Contains(time))
            {
                float t = (time - fadeInSpan.on) / fadeInTime;
                t = fadeInCurve.Evaluate(t);

                ui.tmp.color = Color.Lerp(incomingColor, playingColor, t);
                ui.tmp.transform.localScale = Vector3.Lerp(Vector3.one * fadeInScale, Vector3.one, t);
            }

            // playing
            else if (n.timeSpan.Contains(time))
            {
                ui.tmp.color = playingColor;
                ui.tmp.transform.localScale = Vector3.one;
            }

            // stopping
            else if (fadeOutSpan.Contains(time))
            {
                float t = (time - fadeOutSpan.on) / fadeOutTime;
                t = fadeOutCurve.Evaluate(t);

                ui.tmp.color = Color.Lerp(playingColor, outgoingColor, t);
                ui.tmp.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * fadeOutScale, t);
            }

            else
            {
                // ???
                ui.tmp.color = Color.clear;
                ui.tmp.transform.localScale = Vector3.one;
            }
        }

        public void SetScaleState(MusicScale s)
        {
            // if the scale contains this fret's note
            // set it's color to playing? visible?
        }

        // fade in
        public void FadeIn()
        {
            StartCoroutine(_FadeIn());
        }
        private IEnumerator _FadeIn()
        {
            // immediately set the color to the new color
            // lerp back to the original 

            float startTime = Time.time;
            while (Time.time < startTime + fadeInTime)
            {
                float t = (Time.time - startTime) / fadeInTime;
                t = fadeInCurve.Evaluate(t);

                // change colors
                ui.tmp.color = Color.Lerp(incomingColor, textStartColor, t);
                ui.tmp.transform.localScale = Vector3.Lerp(Vector3.one * fadeInScale, Vector3.one, t);


                yield return new WaitForEndOfFrame();
            }

            ui.tmp.color = textStartColor;
            ui.tmp.transform.localScale = Vector3.one;

            // StartCoroutine(Hold());
        }

        // ?????
        public void Hold()
        {
            StartCoroutine(_Hold());
        }
        private IEnumerator _Hold()
        {
            while (true)
            {
                // noise? 
                // modify tmp.transform.


                // lerp ?
                ui.tmp.color = playingColor;


                yield return null;
            }
        }
        public void Release()
        {
            StopCoroutine(_Hold());
        }
        public void FadeOut()
        {
            StartCoroutine(_FadeOut());
        }
        private IEnumerator _FadeOut()
        {
            ui.fret.Show();
            float startTime = Time.time;
            while (Time.time < startTime + fadeOutTime)
            {
                float t = (Time.time - startTime) / fadeOutTime;
                t = fadeOutCurve.Evaluate(t);


                // lerp from start color to end color
                // change colors
                ui.tmp.color = Color.Lerp(textStartColor, outgoingColor, t);
                ui.tmp.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * fadeOutScale, t);


                yield return new WaitForEndOfFrame();
            }

            ui.tmp.color = textStartColor;
            ui.tmp.transform.localScale = Vector3.one;

            if (hideOnFinish)
                ui.fret.Hide();
        }

        
    }
}