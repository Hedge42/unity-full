using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Neat.Music
{
    public class NoteOverlay : MonoBehaviour
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
            UpdateOverlay(new GuitarTuning().Notes());
        }

        public List<NoteUI> UpdateOverlay(List<Note> notes)
        {
            DestroyUI();
            Destroyer.DestroyChildren<NoteUI>(container);
            existing = new List<NoteUI>();
            for (int i = 0; i < notes.Count; i++)
                existing.Add(CreateOverlayNote(notes[i]));
            return existing;
        }
        public void DestroyUI()
        {
            Destroyer.DestroyChildren<NoteUI>(container);
        }
        public NoteUI CreateOverlayNote(Note n)
        {
            // create with overlay-specific event settings
            NoteUI ui = Instantiate(controller.notePrefab, container.transform);
            ui.gameObject.SetActive(true);
            ui.SetData(n, this);
            ui.eventHandler = ui.gameObject.AddComponent<NoteOverlayHandler>();
            return ui;
        }
        public float GetY(int lane)
        {
            for (int i = 0; i < existing.Count; i++)
                if (i == lane)
                    return existing[i].rect.anchoredPosition.y;

            Debug.Log("No Y found (count=" + existing.Count + " index=[" + lane + "])");
            return 0f;
        }
    }
}
