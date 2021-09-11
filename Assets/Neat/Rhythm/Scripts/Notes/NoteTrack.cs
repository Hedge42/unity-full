using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    public class NoteTrack : MonoBehaviour
    {
        // inspector
        public Transform container;
        public List<NoteUI> notes;

        // references
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
        private KeyOverlay _overlay;
        public KeyOverlay overlay
        {
            get
            {
                if (!_overlay) // does this work wtf?
                    _overlay = GetComponent<KeyOverlay>();
                return _overlay;
            }
        }

        // methods
        public NoteUI CreateTrackNote(TrackNote n)
        {
            // new gameobject with data
            NoteUI ui = Instantiate(overlay.existing[n.lane], container);
            ui.gameObject.SetActive(true);
            ui.SetInput<NoteInputHandler>();

            // set data?
            TrackNote g = n.Clone();
            g.timeSpan = NewTimeSpan();
            ui.SetData(g, overlay);

            // setting up
            UpdateTransform(ui);

            print("Placed " + g.FullName() + " @ " + g.timeSpan.label);

            return ui;
        }
        public void UpdateTransform(NoteUI ui)
        {
            // switch to left-alignment
            ui.rect.pivot = new Vector2(0, ui.rect.pivot.y);

            float dps = controller.ui.scroller.distancePerSecond;
            var overlayNote = overlay.existing[ui.note.lane];

            // note length
            var length = ui.note.timeSpan.duration * dps;
            ui.rect.sizeDelta = new Vector2(length, overlayNote.rect.sizeDelta.y);
            print("Timespan: " + ui.note.timeSpan.duration + " — Length: " + length);

            // position calculations
            var x = ui.note.timeSpan.on * controller.ui.scroller.distancePerSecond;
            var y = overlayNote.rect.position.y;

            // anchored x, global y
            ui.rect.anchoredPosition = new Vector2(x, 0);
            ui.rect.position = new Vector3(ui.rect.position.x, y);
        }
        private TimeSpan NewTimeSpan()
        {
            float on = overlay.controller.time;
            float off = on + GetDuration();
            return new TimeSpan(on, off);
        }
        private float GetDuration()
        {
            // find length of note
            var controller = overlay.controller;
            var timingMap = controller.chart.timingMap;
            TimeSignature signature = timingMap.GetSignatureAtTime(controller.time);
            float duration = 0f;
            if (signature != null)
                duration = signature.TimePerDivision(signature.denominator);
            else
                duration = Mathf.Min(1f, timingMap.
                    Earliest(controller.time).time - controller.time);
            return duration;
        }
    }

    // file for me pls
    public static class Destroyer
    {
        public static void Destroy(GameObject o)
        {
            if (Application.isPlaying)
                GameObject.Destroy(o);
            else
                GameObject.DestroyImmediate(o);

        }
        public static void Destroy(Component c)
        {
            if (c == null)
            {
                Debug.Log("Stop, it's already dead!");
                return;
            }

            if (Application.isPlaying)
                GameObject.Destroy(c);
            else
                GameObject.DestroyImmediate(c);
        }
        public static void DestroyChildren<T>(Transform t)
        {
            var children = new List<Transform>();
            foreach (Transform child in t)
                if (child.GetComponent<T>() != null)
                    children.Add(child);

            foreach (Transform child in children)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(child.gameObject);
                else
                    GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
}