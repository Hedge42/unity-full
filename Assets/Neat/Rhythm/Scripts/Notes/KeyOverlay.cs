using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Neat.Music
{
    public class KeyOverlay : MonoBehaviour
    {
        public Transform container;

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
        private NoteTrack _track;
        public NoteTrack track
        {
            get
            {
                if (!_track)
                    _track = GetComponent<NoteTrack>();
                return _track;
            }
        }

        public List<NoteUI> existing { get; private set; }

        private void Start()
        {
            UpdateOverlay(new GuitarTuning().TrackNotes());
        }
        public List<NoteUI> UpdateOverlay(List<TrackNote> notes)
        {
            DestroyUI();
            Destroyer.DestroyChildren<NoteUI>(container);
            existing = new List<NoteUI>();
            for (int i = 0; i < notes.Count; i++)
                existing.Add(SpawnKey(notes[i]));

            SetColors();

            return existing;
        }
        public NoteUI SpawnKey(TrackNote n)
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
            var colors = GetComponent<ColorPalette>().colors;
            for (int i = 0; i < existing.Count; i++)
            {
                existing[i].foreground.color = colors[i];
            }
        }
        public void DestroyUI()
        {
            Destroyer.DestroyChildren<NoteUI>(container);
        }
    }
}
