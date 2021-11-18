using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Neat.Music
{
    public class FretAnimation
    {
        public Note note { get; private set; }
        public Fret fret { get; private set; }
        public TextMeshProUGUI tmpBase { get; private set; }
        public TextMeshProUGUI tmpFader { get; private set; }

        private ChartPlayer p;
        private FretboardAnimator fa;

        public FretAnimation(FretboardAnimator fa, Fret f, Note n, ChartPlayer p)
        {
            this.fa = fa;
            this.fret = f;
            this.p = p;
            this.note = n;

            // duplicate TMP gameObject twice...
            var _tmp = f.mono.tmp;

            var go = GameObject.Instantiate(_tmp.gameObject, _tmp.transform.parent);
            var go2 = GameObject.Instantiate(_tmp.gameObject, _tmp.transform.parent);

            go.name = "Text Anim Base";
            go2.name = "Text Anim Fader";

            this.tmpBase = go.GetComponent<TextMeshProUGUI>();
            this.tmpFader = go2.GetComponent<TextMeshProUGUI>();

            // Debug.Log("Spawned ", go);

            Update();
        }

        public void Update()
        {
            var n = note;

            var laneColor = p.GetComponent<ColorPaletteUI>().colors[n.lane];
            float h, s, v;
            Color.RGBToHSV(laneColor, out h, out s, out v);
            laneColor = Color.HSVToRGB(h, 1f, 1f);

            var playingColor = Color.Lerp(laneColor, Color.white, .7f);
            var incomingColor = Color.Lerp(laneColor, Color.white, .75f);

            float time = p.time;
            float minTime = n.timeSpan.on - fa.fadeInTime;
            float maxTime = n.timeSpan.off + fa.fadeOutTime;

            TimeSpan fullSpan = new TimeSpan(minTime, maxTime);
            TimeSpan incomingTimeSpan = new TimeSpan(minTime, n.timeSpan.on);
            TimeSpan offPulseTimeSpan = new TimeSpan(n.timeSpan.off, maxTime);
            TimeSpan onPulseTimeSpan = new TimeSpan(n.timeSpan.on, n.timeSpan.on + fa.fadeOutTime);



            // incoming
            if (incomingTimeSpan.Contains(time))
            {
                float fadeInT = (time - incomingTimeSpan.on) / fa.fadeInTime;
                fadeInT = fa.fadeInCurve.Evaluate(fadeInT);

                //var color = Color.Lerp(incomingColor, Color.white, .8f);
                var color = incomingColor;
                var start = new Color(color.r, color.g, color.b, 0f);
                var end = new Color(color.r, color.g, color.b, 1f);

                tmpFader.color = Color.Lerp(start, end, fadeInT);
                tmpFader.transform.localScale = Vector3.Lerp(Vector3.one * fa.fadeInScale, Vector3.one, fadeInT);

                tmpBase.color = Color.Lerp(Color.clear, fa.visibleColor, fadeInT);
            }

            // active
            else if (n.timeSpan.Contains(time))
            {
                // note-on pulse
                if (onPulseTimeSpan.Contains(time))
                {
                    float onPulseT = (time - onPulseTimeSpan.on) / fa.fadeOutTime;
                    tmpFader.color = Color.Lerp(playingColor, fa.outgoingColor, onPulseT);
                    tmpFader.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * fa.fadeOutScale, onPulseT);
                }
                else
                {
                    tmpFader.color = Color.clear;
                    tmpFader.transform.localScale = Vector3.one;
                }

                //tmpBase.color = fa.playingColor;
                tmpBase.color = playingColor;
                tmpBase.transform.localScale = Vector3.one;
            }
            else if (offPulseTimeSpan.Contains(time))
            {
                float offPulseT = (time - offPulseTimeSpan.on) / fa.fadeOutTime;
                offPulseT = fa.fadeOutCurve.Evaluate(offPulseT);

                tmpBase.color = Color.Lerp(fa.visibleColor, Color.clear, offPulseT);
                tmpFader.color = Color.clear;
                tmpFader.transform.localScale = Vector3.one;
            }
            else
            {
                Hide();
            }
        }

        public void Hide()
        {
            tmpBase.color = tmpFader.color = Color.clear;
            tmpBase.transform.localScale = tmpFader.transform.localScale = Vector3.one;
        }
        public void Destroy()
        {
            try
            {
                Destroyer.Destroy(tmpBase.gameObject);
                Destroyer.Destroy(tmpFader.gameObject);
            }
            catch (Exception e)
            {
                if (e is MissingReferenceException)
                    Debug.LogWarning(e.Message);
                else
                    Debug.LogError(e.Message);
            }
        }
    }
}