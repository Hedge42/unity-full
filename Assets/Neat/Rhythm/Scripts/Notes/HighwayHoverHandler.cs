using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Tools.Extensions;

namespace Neat.Audio.Music
{
    public class HighwayHoverHandler
    {
        private ChartPlayer controller;
        private NoteHighway scroller;
        private KeyOverlay overlay;
        private NoteSpawner spawner;
        private NoteUI prefab;
        private Transform container;

        private NoteUI[] _instances;
        private NoteUI[] instances
        {
            get => _instances == null ? _instances = SpawnInstances() : _instances;
        }
        private NoteUI instance => instances[0];
        // pool of semi-transparent notes to preview change?

        private Note hovered;
        private bool isHoveredExisting;

        public bool enabled;

        public HighwayHoverHandler()
        {
            controller = ChartPlayer.instance;
            scroller = controller.scroller;
            overlay = scroller.overlay;
            spawner = overlay.noteSpawner;
            prefab = controller.ui.notePrefab;
            container = spawner.container;
            // instances = SpawnInstances();
            // Enable(true);
        }
        public void GetInput()
        {
            if (!enabled)
                return;

            UpdateHovered();
            UpdateInstance();
            //var hoveredNote = HoveredNote();
            //if (!instance.note.Equals(hoveredNote))
            //{
            //    instance.note = hoveredNote;
            //    instance.UpdateUI();
            //    instance.color.SetAlpha(.5f); // relative vs. absolute?
            //}
        }
        private void UpdateHovered()
        {
            var hoveredUI = ExistingNoteUI();
            this.isHoveredExisting = hoveredUI != null;

            if (hoveredUI != null)
                this.hovered = hoveredUI.note;
            else
                this.hovered = PotentialNote();

            Debug.Log("Hovered: " + this.hovered.ToString());
        }
        private void UpdateInstance()
        {
            if (hovered == null || isHoveredExisting)
            {
                // hide
                instance.SetAlpha(0f);
            }
            else if (instance.note != hovered)
            {
                instance.note = hovered;
                instance.UpdateUI();
                instance.color.SetAlpha(.5f);
            }
        }
        public void Enable(bool value)
        {
            enabled = value;
            if (enabled)
            {
                instance.SetAlpha(.5f);
            }
            else
            {
                instance.SetAlpha(0f);
            }
        }

        private NoteUI[] SpawnInstances()
        {
            // Debug.Log("Spawner: " + (spawner == null));
            // Debug.Log("Overlay: " + (overlay == null));
            // Debug.Log("Existing: " + (overlay.existing == null));

            // var noteui = spawner.Spawn(overlay.existing[0].note, true);
            var noteui = GameObject.Instantiate(prefab, spawner.container);
            var note = new Note(controller.noteMap.tuning, 0, 0);
            note.timeSpan = new TimeSpan(0f, 0f);
            noteui.SetData(note, overlay);
            noteui.name = "HOVERING";

            // noteui.note = new Note(controller.noteMap.tuning, 0, 0);
            noteui.UpdateUI();
            noteui.color.SetAlpha(.5f);

            return new NoteUI[] { noteui };
        }

        public bool Conflicts(Note n)
        {
            return n.Overlaps(controller.noteSpan.notes.ToArray());
        }

        public Note PotentialNote()
        {
            var mouse = Input.mousePosition;
            var note = overlay.Key(mouse).note;
            var timing = HoveredTiming(mouse);
            note.timeSpan = new TimeSpan(timing);
            return note;
        }
        public NoteUI ExistingNoteUI()
        {
            var mouse = Input.mousePosition;
            foreach (NoteUI nui in spawner.active)
            {
                if (nui.rect.rect.Contains(mouse))
                {
                    return nui;
                }
            }
            return null;
        }

        public Timing HoveredTiming(Vector2 mouse)
        {
            var mouseTime = MouseTime(mouse);
            var snap = controller.ui.timingBar.snap;
            var t = snap.Earliest(mouseTime);
            return t;
        }


        public float MouseTime(Vector2 mouse)
        {
            var localX = scroller.movingContainer.InverseTransformPoint(mouse).x;
            var mouseTime = localX / scroller.distancePerSecond;

            return mouseTime;
        }
    }
}
