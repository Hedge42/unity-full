using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neat.Music;
using System.Linq;
using UnityEngine.Events;

namespace Neat.Guitar
{
    public class BeatDrawer : MonoBehaviour
    {
        public RectTransform container;
        public BeatDivider prefab;

        private ChartController _chartPlayer;
        public ChartController controller
        {
            get
            {
                if (_chartPlayer == null)
                    _chartPlayer = GetComponent<ChartController>();
                return _chartPlayer;
            }
        }

        private List<BeatDivider> _dividers;
        private List<BeatDivider> dividers
        {
            get
            {
                if (_dividers == null)
                    _dividers = new List<BeatDivider>();
                return _dividers;
            }
        }


        private List<Beat> _beats;
        public List<Beat> beats
        {
            get
            {
                if (_beats == null)
                    _beats = new List<Beat>();
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

        private BeatDivider earliestDiv
        {
            get
            {
                if (dividers.Count > 0)
                    return dividers[0];
                else
                    return null;
            }
        }
        private BeatDivider latestDiv
        {
            get
            {
                if (dividers.Count > 0)
                    return dividers[dividers.Count - 1];
                else
                    return null;
            }
        }

        private Beat earliestBeat
        {
            get
            {
                if (earliestDiv == null)
                    return timingMap.Earliest(controller.time);

                else
                    return earliestDiv.beat;

            }
        }
        private Beat latestBeat
        {
            get
            {
                if (latestDiv == null)
                    return earliestBeat;
                else
                    return latestDiv.beat;
            }
        }

        public UnityEvent<Beat> onBeatPassed;


        // this class is dependent on a timing map to do anything
        private bool canDraw
        {
            get
            {
                return timingMap != null && timingMap.timeSignatures.Count > 0;
            }
        }

        private void Start()
        {
            controller.player.onSourceTick += OnTimeTick;
            controller.player.onSkip += OnTimeSet;
        }

        public void OnTimeSet(float f)
        {
            if (canDraw)
            {
                // calculates beats
                DiscardAll();
                var _beats = timingMap.NextBeatsBetween(f, controller.maxTime);
                foreach (Beat b in _beats)
                    Add(b);
                // CreateAll();
            }
            // else error?
        }
        public void CreateAll()
        {
            //var next = timingMap.Earliest(controller.time);

            var beats = timingMap.NextBeatsBetween(controller.time, controller.maxTime);
            foreach (Beat b in beats)
                Add(b);

            //while (next.time < controller.time)
            //    next = next.next;

            //while (next.time < controller.maxTime)
            //{
            //    Add(next);
            //    next = next.next;
            //}
        }

        public void OnTimeTick(float f)
        {
            // draw until maxTime
            if (latestBeat.time <= controller.maxTime)
            {
                Add(latestBeat.next);
            }

            if (dividers.Count > 0)
            {
                // trim past beats
                if (earliestDiv.beat.time <= controller.time)
                {
                    // tick
                    onBeatPassed?.Invoke(earliestDiv.beat);
                    Discard(earliestDiv);
                }
            }
        }

        private void Discard(BeatDivider bd)
        {
            dividers.Remove(bd);
            SafeDestroy(bd.gameObject);
        }
        public void Add(Beat next)
        {
            var bd = GameObject.Instantiate(prefab, container);
            bd.gameObject.SetActive(true);
            bd.drawer = this;
            bd.UpdateBeat(next);

            // dic.Add(next, bd);
            //beats.Add(next);
            dividers.Add(bd);
        }


        public void DiscardAll()
        {
            while (dividers.Count > 0)
                dividers.RemoveAt(0);

            var children = new List<Transform>();
            foreach (Transform child in container.transform)
                if (child.GetComponent<BeatDivider>() != null)
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
    }
}