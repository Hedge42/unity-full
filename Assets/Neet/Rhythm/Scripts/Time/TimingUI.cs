using Neat.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Music
{
    public class TimingUI : MonoBehaviour, OutputState, SkipState
    {
        public RectTransform container;
        public BeatUI prefab;

        public Metronome metronome;

        private ChartController _controller;
        public ChartController controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartController>();
                return _controller;
            }
        }

        // one line?
        private List<BeatUI> _dividers;
        private List<BeatUI> dividers
        {
            get
            {
                if (_dividers == null)
                    _dividers = new List<BeatUI>();
                return _dividers;
            }
        }

        // one line?
        private List<Timing> _beats;
        public List<Timing> beats
        {
            get
            {
                if (_beats == null)
                    _beats = new List<Timing>();
                return _beats;
            }
            set
            {
                _beats = value;
            }
        }
        public TimingMap timingMap
        {
            get
            {
                return controller.chart.timingMap;
            }
        }
        private BeatUI earliestDiv
        {
            get
            {
                if (dividers.Count > 0)
                    return dividers[0];
                else
                    return null;
            }
        }
        private BeatUI latestDiv
        {
            get
            {
                if (dividers.Count > 0)
                    return dividers[dividers.Count - 1];
                else
                    return null;
            }
        }

        private Timing earliestBeat
        {
            get
            {
                if (earliestDiv == null)
                    return timingMap.Earliest(controller.time);

                else
                    return earliestDiv.beat;

            }
        }
        private Timing latestBeat
        {
            get
            {
                if (latestDiv == null)
                    return earliestBeat;
                else
                    return latestDiv.beat;
            }
        }

        public bool showFullWindow;


        // this class is dependent on a timing map to do anything
        private bool canDraw
        {
            get
            {
                return timingMap != null && timingMap.timeSignatures.Count > 0;
            }
        }

        public void SetTime(float f)
        {
            if (canDraw)
            {
                // calculates beats
                DiscardAll();
                var _beats = timingMap.TimingsBetween(f, controller.scroller.maxTime);
                foreach (Timing b in _beats)
                    Add(b);

                UpdateTime(f);
            }
            else
            {
                print("null: " + timingMap == null);
                print("count: " + timingMap.timeSignatures.Count);

                //Debug.LogError("Could not draw");
            }
        }
        public void _UpdateTime(float f)
        {
            // create until window end
            //if (latestBeat.time <= controller.scroller.maxTime)
            //Add(latestBeat.next);

            // draw up to end
            var next = latestBeat.Next();
            while (next.time <= controller.scroller.maxTime)
            {
                Add(next);
                next = next.Next();
            }

            // destroy until window start
            if (dividers.Count > 0)
            {
                if (earliestDiv.beat.time <= controller.time)
                {
                    // tick
                    PlayMetronome(earliestDiv.beat);
                    Discard(earliestDiv);
                }
            }
        }
        public void UpdateTime(float time)
        {
            // draw up to end
            var next = latestBeat.Next();
            while (next.time <= controller.scroller.maxTime)
            {
                Add(next);
                next = next.Next();
            }

            float minTime = showFullWindow ? controller.scroller.minTime : controller.time;
            minTime = controller.time;

            // remove up to start
            Timing ticked = null;
            while (dividers.Count > 0 && earliestDiv.beat.time < minTime)
            {
                ticked = earliestDiv.beat;
                Discard(earliestDiv);
            }

            // play metronome only once, even if multiple were passed.
            if (ticked != null && controller.clock.isActive)
                PlayMetronome(ticked);
        }

        public void SkipForward()
        {
            if (IsOnBeat(earliestBeat))
                controller.SetTime(earliestBeat.Next().time);
            else
                controller.SetTime(earliestBeat.time);
        }
        public void SkipBack()
        {
            controller.SetTime(earliestBeat.Prev().time);
        }
        private bool IsOnBeat(Timing b)
        {
            float t = controller.time;
            float max = t + Mathf.Epsilon;
            float min = t - Mathf.Epsilon;

            return b.time >= min && b.time <= max;
        }

        public void CreateAll()
        {
            var beats = timingMap.TimingsBetween(controller.time, controller.scroller.maxTime);
            foreach (Timing b in beats)
                Add(b);
        }
        public void Add(Timing next)
        {
            var bd = GameObject.Instantiate(prefab, container);
            bd.gameObject.SetActive(true);
            bd.controller = controller;
            bd.UpdateBeat(next);

            dividers.Add(bd);
        }
        public void Discard(BeatUI bd)
        {
            dividers.Remove(bd);
            SafeDestroy(bd.gameObject);
        }
        public void DiscardAll()
        {
            while (dividers.Count > 0)
                dividers.RemoveAt(0);

            var children = new List<Transform>();
            foreach (Transform child in container.transform)
                if (child.GetComponent<BeatUI>() != null)
                    children.Add(child);

            foreach (var child in children)
                SafeDestroy(child.gameObject);

        }
        public void SafeDestroy(GameObject gameObject)
        {
            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }

        private void PlayMetronome(Timing b)
        {
            metronome.Play(b.isMeasureStart);
        }
    }
}