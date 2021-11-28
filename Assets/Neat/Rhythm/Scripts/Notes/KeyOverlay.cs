using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Neat.Audio.Music
{
    public class KeyOverlay : MonoBehaviour, Loadable
    {
        public Transform container;

        private ChartPlayer _controller;
        public ChartPlayer controller
        {
            get
            {
                if (_controller == null)
                    _controller = GetComponent<ChartPlayer>();
                return _controller;
            }
        }
        private NoteSpawner _track;
        public NoteSpawner noteSpawner
        {
            get
            {
                if (!_track)
                    _track = GetComponent<NoteSpawner>();
                return _track;
            }
        }
        private ColorPaletteUI _pallete;
        public ColorPaletteUI pallete
        {
            get
            {
                if (_pallete == null)
                    _pallete = GetComponent<ColorPaletteUI>();
                return _pallete;
            }
            set
            {
                _pallete = value;
            }
        }

        public List<NoteUI> existing;

        public int numLanes => controller.noteMap.tuning.numStrings;

        public List<NoteUI> UpdateOverlay()
        {
            var notes = controller.noteMap.tuning.Notes();

            DestroyUI();
            Destroyer.DestroyChildren<NoteUI>(container);
            existing = new List<NoteUI>();
            for (int i = 0; i < notes.Count; i++)
                existing.Add(SpawnKey(notes[i]));

            // https://answers.unity.com/questions/1276433/get-layoutgroup-and-contentsizefitter-to-update-th.html
            // fixes an issue where notes don't get the right y-position
            LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());

            SetColors();
            return existing;
        }
        private NoteUI SpawnKey(Note n)
        {
            // create with overlay-specific event settings
            NoteUI ui = Instantiate(controller.ui.notePrefab, container.transform);
            ui.gameObject.SetActive(true);
            ui.SetData(n, this);
            ui.SetInput<KeyInputHandler>();

            // special

            return ui;
        }

        private void SetColors()
        {
            var colors = GetComponent<ColorPaletteUI>().colors;
            foreach (NoteUI n in existing)
                n.UpdateColor();

            //for (int i = 0; i < existing.Count; i++)
            //{
            //    //existing[i].foreground.color = colors[i];
            //    existing[i].UpdateColor();
            //}
        }
        public void DestroyUI()
        {
            Destroyer.DestroyChildren<NoteUI>(container);
        }

        public void OnLoad(Chart c)
        {
            UpdateOverlay();

            controller.player.onSkip.AddListener(SetTime);
            controller.player.onTick.AddListener(UpdateTime);

            controller.noteSpan.onNoteOn.AddListener(NoteOn);
            controller.noteSpan.onNoteOff.AddListener(NoteOff);
        }

        public void NoteOn(Note note)
        {
            print("Note on - " + note.ToString());

            var k = Key(note.lane);
            k.note = note;
            k.UpdateText();
        }
        public void NoteOff(Note note)
        {
            // ???
        }

        public void SetTime(float f)
        {
            ShowRecentNotes();
        }
        public void UpdateTime(float f)
        {
            var turnOn = controller.noteSpan.playing;

            //foreach (var n in controller.noteSpan.playing)
            //{

            //}
        }

        public NoteUI Key(Note match)
        {
            return existing.Where(n => n.Equals(match)).FirstOrDefault();
        }
        public NoteUI Key(int k)
        {
            return existing.Where(n => n.note.lane == k).FirstOrDefault();
        }
        public NoteUI Key(Vector2 position)
        {
            NoteUI nearest = null;
            var minDist = float.MaxValue;
            foreach (NoteUI k in existing)
            {
                var bounds = k.Bounds();
                var dist = bounds.Min(v => Vector2.Distance(position, v));
                if (dist < minDist)
                {
                    nearest = k;
                    minDist = dist;
                }
            }
            return nearest;
        }
        public NoteUI YAligned()
        {
            return Key(Input.mousePosition);
        }

        public void ShowRecentNotes()
        {
            // foreach lane
            // get the most recent note from the notemap

            for (int i = 0; i < existing.Count; i++)
            {
                var ui = existing[i];

                // find most recent note in this lane
                var inLane = controller.noteMap.notes.Where(n => n.lane == i);
                var prev = inLane.Where(n => n.timeSpan.on <= controller.time);
                // https://stackoverflow.com/questions/1101841/how-to-perform-max-on-a-property-of-all-objects-in-a-collection-and-return-th
                // var maxObject = list.OrderByDescending(item => item.Height).First();
                var mostRecent = prev.OrderByDescending(n => n.timeSpan.on);
                if (mostRecent.Count() != 0)
                {
                    // update fret to most recent note
                    var note = mostRecent.First();
                    ui.note.fret = note.fret;
                }
                else
                {
                    // set to default
                    ui.note.fret = 0;
                }

                // update ui
                ui.note.UpdateFret();
                ui.UpdateText();
            }
        }
    }
}
