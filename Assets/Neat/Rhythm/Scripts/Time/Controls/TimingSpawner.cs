using Neat.States;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Neat.Music
{
    // spawns timing bar gameObjects relative to time signatures
    [ExecuteAlways]
    public class TimingSpawner : MonoBehaviour, OutputState, SkipState, Loadable
    {
        // inspector
        public RectTransform container;
        public TimingUI prefab;
        public Metronome metronome;
        public bool showFullWindow;

        [SerializeField] private Snapping _snap;
        public Snapping snap
        {
            get
            {
                if (_snap == null)
                    _snap = new Snapping(timingMap, Snapping.Setting.Beat, 1);
                return _snap;
            }
        }

        // properties
        public ChartPlayer controller { get; private set; }
        public TimingMap timingMap => controller.chart.timingMap;
        public List<TimingUI> dividers { get; private set; }

        private TimingUI earliestDiv
        {
            get
            {
                if (dividers.Count > 0)
                    return dividers[0];
                else
                    return null;
            }
        }
        private TimingUI latestDiv
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
        private bool canDraw
        {
            get
            {
                return timingMap != null && timingMap.signatures.Count > 0;
            }
        }

        private void Awake()
        {
            controller = GetComponent<ChartPlayer>();
            dividers = new List<TimingUI>();

            DiscardAll();
        }

        public void ApplySnapping()
        {
            _snap = new Snapping(timingMap, snap.per, snap.count);
            Spawn(snap.GetTimings(controller.timer.fullTimespan));
        }
        public void ApplySnapping(Snapping s)
        {
            _snap = s;
            Spawn(snap.GetTimings(controller.timer.fullTimespan));

        }
        public void ResetSnapping()
        {
            _snap = new Snapping(timingMap, Snapping.Setting.Beat, 1);
        }

        // updating time
        public void SetTime(float f)
        {
            if (canDraw)
            {
                // calculates beats
                //Spawn(timingMap.TimingsBetween(f, controller.timer.maxTime));

                ApplySnapping();
                // Spawn(snap.GetTimings(controller.timer.fullTimespan));

                UpdateTime(f); // ????
            }
            else
            {
                Debug.LogError("Could not draw bars");
                // print("null: " + timingMap == null);
                // print("count: " + timingMap.signatures.Count);
                //Debug.LogError("Could not draw");
            }
        }
        public void UpdateTime(float time)
        {
            // draw up to end
            var next = latestBeat.Next();
            while (next.time <= controller.timer.maxTime)
            {
                Spawn(next);
                next = next.Next();
            }

            float minTime = showFullWindow ? controller.timer.minTime : controller.time;
            minTime = controller.time;

            // remove up to start
            Timing ticked = null;
            while (dividers.Count > 0 && earliestDiv.beat.time < minTime)
            {
                ticked = earliestDiv.beat;
                Discard(earliestDiv);
            }

            // play metronome only once, even if multiple were passed.
            if (ticked != null && controller.player.isPlaying)
                PlayMetronome(ticked);
        }

        // spawning and destroying
        public void Spawn(List<Timing> timings)
        {
            DiscardAll();
            foreach (var timing in timings)
                Spawn(timing);
        }
        public void Spawn(Timing next)
        {
            // print("Adding timing...");
            var bd = GameObject.Instantiate(prefab, container);
            bd.gameObject.SetActive(true);
            bd.controller = controller;
            bd.UpdateBeat(next);

            dividers.Add(bd);
        }

        public void Discard(TimingUI bd)
        {
            dividers.Remove(bd);
            Destroyer.Destroy(bd.gameObject);
        }
        public void DiscardAll()
        {
            while (dividers.Count > 0)
                dividers.RemoveAt(0);

            Destroyer.DestroyChildren<TimingUI>(container.transform);
        }

        // skip controls
        public void SkipForward()
        {
            var beat = earliestBeat;
            if (IsOnBeat(earliestBeat))
                beat = beat.Next();

            controller.SkipTo(beat.time);
        }
        public void SkipBack()
        {
            controller.SkipTo(earliestBeat.Prev().time);
        }

        private void PlayMetronome(Timing b)
        {
            metronome.Play(b.isMeasureStart);
        }
        private bool IsOnBeat(Timing b)
        {
            float t = controller.time;
            float max = t + Mathf.Epsilon;
            float min = t - Mathf.Epsilon;

            return b.time >= min && b.time <= max;
        }

        public void OnLoad(Chart c)
        {
            ResetSnapping();
            SetTime(0f);
        }
    }
}