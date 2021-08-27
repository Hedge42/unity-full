using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neat.Guitar
{
    public class LaneUI : MonoBehaviour
    {

        public GameObject horizontalLinePrefab;
        public GameObject verticalLinePrefab;
        public GameObject beatLinePrefab;
        public GameObject laneObjectPrefab;

        public RectTransform scrollArea;
        public Transform judgementLine;
        public Transform laneObjectContainer;
        public Transform timeMarkerContainer;
        public VerticalLayoutGroup stringLayout;
        public int numStrings; // don't store this here

        public float scrollSpeed; // seconds

        public float time;
        public int beatIndex;
        private bool isPause = true;

        private TimeMarker[] beatMarkers;
        private List<LaneObject> laneObjects;

        private RectTransform[] gStrings;

        private FretboardUI _fretboard;
        public FretboardUI fretboard
        {
            get
            {
                if (_fretboard == null)
                    _fretboard = GameObject.FindObjectOfType<FretboardUI>();
                return _fretboard;
            }
            set
            {
                _fretboard = value;
            }
        }

        private Chart _chart;
        public Chart chart
        {
            get
            {
                if (_chart == null)
                    _chart = new Chart();
                return _chart;
            }
        }

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

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                Pause(!isPause);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SkipTo(beatIndex - 1);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                SkipTo(beatIndex + 1);
            }

            var mouseWheel = UnityEngine.Input.mouseScrollDelta;
            //print(mouseWheel);
            if (Mathf.Abs(mouseWheel.y) > .8)
                SkipTo((int)(beatIndex - mouseWheel.y));
        }

        private void Start()
        {
            beatIndex = 0;
            time = 0f;

            laneObjects = new List<LaneObject>();
            TrimGuitarStrings(fretboard.tuning.numStrings);
            MakeTimeMarkers();

            SetupFretboard();
        }

        public void Pause(bool isPause)
        {
            this.isPause = isPause;
            if (!isPause)
                StartCoroutine(_Play());
        }
        private IEnumerator _Play()
        {
            while (!isPause)
            {
                time += Time.deltaTime;

                if (time >= chart.duration)
                {
                    SkipTo(0);
                    Pause(true);
                }
                else
                {
                    var idx = GetBeatIndex(time);

                    // update index and play metronome
                    if (idx > beatIndex)
                    {
                        beatIndex = idx;
                        GetComponent<Neat.Audio.SoundBank>().Play(0);
                    }
                }

                UpdateScrollPosition();

                yield return new WaitForEndOfFrame();

            }
        }

        private void SetupFretboard()
        {
            fretboard.onFretClicked += AddLaneObject;
        }

        // TODO limit number of markers spawned
        private void UpdateTimingPoints()
        {
            foreach (TimeMarker t in beatMarkers)
            {
                t.gameObject.transform.position = judgementLine.position + Vector3.right * scrollSpeed * (t.time - time);
            }
        }

        public void SkipTo(float time)
        {
            if (time < 0)
                time = 0f;

            this.time = time;
            this.beatIndex = GetBeatIndex(time);

            UpdateScrollPosition();
        }
        public void SkipTo(int timeIdx)
        {
            // NOTE this cuases double-skip at begninning if marker 0 has time 0
            if (timeIdx < 0)
                timeIdx = -1;

            this.beatIndex = timeIdx;
            this.time = GetBeatTime(beatIndex);

            UpdateScrollPosition();
        }
        private float GetBeatTime(int i)
        {
            if (i < 0)
                return 0f;
            else if (i >= beatMarkers.Length)
                return chart.duration;
            else
                return beatMarkers[i].time;
        }
        private int GetBeatIndex(float f)
        {
            int i = beatIndex;

            // seek previous until begninning found or reference time is passed
            while (i > 0 && GetBeatTime(i) > f)
                i--;

            while (i < beatMarkers.Length && GetBeatTime(i) < f)
                i++;

            // being passed the final marker will result in a marker time at the end of the chart
            return i;
        }
        public void UpdateScrollPosition()
        {
            scrollArea.position = judgementLine.position + Vector3.left * time * scrollSpeed;
        }

        public void AddLaneObject(int fret, int gString)
        {
            // print("adding...");
            var go = Instantiate(laneObjectPrefab, laneObjectContainer);
            go.GetComponentInChildren<TextMeshProUGUI>().text = fret.ToString();
            go.SetActive(true);

            var rect = go.GetComponent<RectTransform>();
            rect.position = new Vector2(judgementLine.position.x, gStrings[gString].position.y);
            // rect.anchoredPosition = rect.localPosition;

            // TODO length = snapping length
            laneObjects.Add(new LaneObject(go, fret, time, 1));
        }

        private bool OnTimingPoint()
        {
            return false;
        }

        // TODO update these
        public void TrimGuitarStrings(int numStrings)
        {
            var dif = numStrings - stringLayout.transform.childCount;

            // spawn a new one for each >0
            if (dif > 0)
            {
                for (int i = 1; i < dif; i++)
                {
                    var go = Instantiate(horizontalLinePrefab, stringLayout.transform);
                    go.SetActive(true);
                }
            }
            // destroy one for each <0
            else if (dif < 0)
            {
                var childList = new List<Transform>();

                for (int i = 0; i < (dif * -1); i++)
                    childList.Add(stringLayout.transform.GetChild(i));

                print("to destroy: " + childList.Count);

                foreach (Transform t in childList)
                {
                    if (Application.isPlaying)
                        Destroy(t.gameObject);
                    else
                        DestroyImmediate(t.gameObject);
                }
            }

            // initialize array
            List<RectTransform> _stringList = new List<RectTransform>();
            foreach (Transform t in stringLayout.transform)
            {
                _stringList.Add(t.GetComponent<RectTransform>());
            }
            gStrings = _stringList.ToArray();
        }
        public void MakeTimeMarkers()
        {
            DestroyTimeMarkers();

            beatMarkers = null;

            var _timeMarkers = new List<TimeMarker>();

            float _time = 0;

            for (int i = 0; i < chart.timeSignatures.Length; i++)
            {
                // loop every beat until end of timeSignature reached
                float endTime;

                // end is end of chart
                if (i >= chart.timeSignatures.Length - 1)
                    endTime = chart.duration;

                // end is next time signature position
                else
                    endTime = chart.timeSignatures[i + 1].offset;

                int whileCount = 0;
                while (_time < endTime)
                {
                    whileCount++;
                    if (whileCount > 1000)
                    {
                        Debug.LogError("here");
                        return;
                    }


                    var go = Instantiate(verticalLinePrefab, timeMarkerContainer);
                    go.SetActive(true);

                    go.transform.position = judgementLine.position
                        + Vector3.right * scrollSpeed * _time;

                    _timeMarkers.Add(new TimeMarker(go, _time));

                    _time += 60 / chart.timeSignatures[i].beatsPerMinute;
                }
            }

            beatMarkers = _timeMarkers.ToArray();
        }
        private void DestroyTimeMarkers()
        {
            var childList = new List<Transform>();

            foreach (Transform child in timeMarkerContainer)
                childList.Add(child);

            foreach (Transform t in childList)
            {
                if (Application.isPlaying)
                    Destroy(t.gameObject);
                else
                    DestroyImmediate(t.gameObject);
            }
        }
    }
}