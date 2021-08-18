using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Neet.Audio;
using TMPro;

namespace Neet.Guitar
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

        public float distancePerSecond;

        public float time;
        public int mIndex;
        private bool isPause = true;

        private TimeMarker[] timeMarkers;

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
                SkipTo(mIndex - 1);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                SkipTo(mIndex + 1);
            }
        }

        private void Start()
        {
            mIndex = 0;
            time = 0f;

            MakeTimeMarkers();
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
                    var idx = GetMarkerIndex(time);

                    // update index and play metronome
                    if (idx > mIndex)
                    {
                        mIndex = idx;
                        GetComponent<SoundBank>().Play(0);
                    }
                }

                UpdateScrollPosition();

                yield return new WaitForEndOfFrame();

            }
        }

        // TODO limit number of markers spawned
        private void UpdateTimingPoints()
        {
            foreach (TimeMarker t in timeMarkers)
            {
                t.gameObject.transform.position = judgementLine.position + Vector3.right * distancePerSecond * (t.time - time);
            }
        }

        public void SkipTo(float time)
        {
            if (time < 0)
                time = 0f;

            this.time = time;
            this.mIndex = GetMarkerIndex(time);

            UpdateScrollPosition();
        }
        public void SkipTo(int timeIdx)
        {
            // NOTE this cuases double-skip at begninning if marker 0 has time 0
            if (timeIdx < 0)
                timeIdx = -1;

            this.mIndex = timeIdx;
            this.time = GetMarkerTime(mIndex);

            UpdateScrollPosition();
        }
        private float GetMarkerTime(int i)
        {
            if (i < 0)
                return 0f;
            else if (i >= timeMarkers.Length)
                return chart.duration;
            else
                return timeMarkers[i].time;
        }
        private int GetMarkerIndex(float f)
        {
            int i = mIndex;

            // seek previous until begninning found or reference time is passed
            while (i > 0 && GetMarkerTime(i) > f)
                i--;

            while (i < timeMarkers.Length && GetMarkerTime(i) < f)
                i++;

            // being passed the final marker will result in a marker time at the end of the chart
            return i;
        }
        public void UpdateScrollPosition()
        {
            scrollArea.position = judgementLine.position + Vector3.left * time * distancePerSecond;
        }

        public void AddLaneObject(int gtrString, int fret)
        {
            var go = Instantiate(laneObjectPrefab, laneObjectContainer);

            // (judgementLine, stringY)
            //go.GetComponent<RectTransform>().anchoredPosition = new Vector2(judgementLine.position.x, strings)

            go.GetComponentInChildren<TextMeshProUGUI>().text = fret.ToString();

            // length = snapping length

            // store in chart
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
        }
        public void MakeTimeMarkers()
        {
            DestroyTimeMarkers();

            timeMarkers = null;

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
                    endTime = chart.timeSignatures[i + 1].startTime;

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
                        + Vector3.right * distancePerSecond * _time;

                    _timeMarkers.Add(new TimeMarker(go, _time));

                    _time += 60 / chart.timeSignatures[i].bpm;
                }
            }

            timeMarkers = _timeMarkers.ToArray();
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