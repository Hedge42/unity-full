using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Neat.Music
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
        private ColorPalette _pallete;
        public ColorPalette pallete
        {
            get
            {
                if (_pallete == null)
                    _pallete = GetComponent<ColorPalette>();
                return _pallete;
            }
            set
            {
                _pallete = value;
            }
        }

        public List<NoteUI> existing { get; private set; }

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
            var colors = GetComponent<ColorPalette>().colors;
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
        }
    }
}
