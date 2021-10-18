using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    [ExecuteAlways]
    public class NoteSpawner : MonoBehaviour, Loadable
    {
        // NoteSpawner.cs?

        // inspector
        public Transform container;

        // props
        private ChartPlayer _controller;
        private ChartPlayer controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartPlayer>();
                return _controller;
            }

        }

        private KeyOverlay _overlay;
        private KeyOverlay overlay
        {
            get
            {
                if (_overlay == null)
                    _overlay = GetComponent<KeyOverlay>();
                return _overlay;
            }
        }

        public List<NoteUI> active = new List<NoteUI>();

        public void SetTime(float newTime)
        {
            // spawn all notes in the time range
            // respawn all active notes

            // creates gameObject instance for each note
            // without modifying note data

            Clear();
            var notes = controller.noteMap.GetNotes(controller.timer.fullTimespan);

            Debug.Log( //GetType().ToString() +
                "Respawning " + notes.Count +  " notes", this);

            foreach (Note n in notes)
                Spawn(n);
        }
        public void UpdateTime(float newTime)
        {
            // destroy notes whose off-time is less than player-time
            // spawn notes whose on-time is less than player-windowEndTime

            // controller.ui.scroller.maxTime;
        }

        public void Clear()
        {
            Destroyer.DestroyChildren<NoteUI>(container);
        }

        public NoteUI Spawn(Note n, bool newTimeSpan = false, bool addToNoteMap = false)
        {
            if (newTimeSpan)
            {
                n = n.Clone();
                n.timeSpan = NewTimeSpan();
            }
            if (addToNoteMap)
            {
                controller.noteMap.Add(n);
            }

            // new gameobject with data
            //print(n.lane + " / " + overlay.existing.Count);

            NoteUI ui = Instantiate(overlay.existing[n.lane], container);
            ui.gameObject.SetActive(true);
            ui.SetInput<NoteInputHandler>();
            ui.SetData(n, overlay);
            ui.UpdateTransform();

            return ui;
        }

        // move this
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


        public void OnLoad(Chart c)
        {
            Debug.Log(GetType().ToString() + " loading...", this);

            controller.player.onTick.AddListener(UpdateTime);
            controller.player.onSkip.AddListener(SetTime);

            SetTime(0f);
        }
    }
}