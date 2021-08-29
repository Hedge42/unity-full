using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neat.Guitar
{
    public class BeatDrawer : MonoBehaviour
    {
        public ChartPlayer scroller;

        public RectTransform container;

        public RectTransform prefab;

        private List<RectTransform> beats;

        private List<Beat> current;

        public void Update()
        {
        }

        public void DrawBars(float startTime, float endTime)
        {
            DestroyAll();


            var map = scroller.chart.timingMap;
            var ts = map.GetSignaturesBetween(startTime, endTime);
            foreach (var t in ts)
            {
                print(t.ToString());
                // print(t.TimePerBar());
            }

            var bars = map.GetBarsBetween(startTime, endTime);
            print(bars.Count + " bars:");
            foreach (var bar in bars)
            {
                print(bar.ToString());
                Create(bar.time);
            }

            // var beats = map.GetBeats
        }

        public void DrawNext(float seconds)
        {
            float startTime = scroller.time;
            float endTime = startTime + seconds;
        }

        public void UpdateBeats()
        {
            // how many 
        }

        public void DestroyAll()
        {
            var children = new List<Transform>();
            foreach (Transform child in container.transform)
            {
                children.Add(child);
            }
            foreach (var child in children)
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }

            beats = new List<RectTransform>();
        }

        public void Create(float time)
        {
            var rect = Instantiate(prefab, container);
            rect.gameObject.SetActive(true);
            rect.anchoredPosition = Vector3.right * time * scroller.distancePerSecond;
        }

        public void Destroy(RectTransform beat)
        {
            Destroy(beat.gameObject);
        }
    }
}