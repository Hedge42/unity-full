using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    public class NoteTrack : MonoBehaviour
    {
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
        private NoteOverlay _overlay;
        public NoteOverlay overlay
        {
            get
            {
                if (!_overlay) // does this work wtf?
                    _overlay = GetComponent<NoteOverlay>();
                return _overlay;
            }
        }

        public Transform container;
        public NoteMap map
        {
            get { return controller.track.noteMap; }
        }



        public List<NoteUI> notes;

        public void _SetTime(float time)
        {
            throw new System.NotImplementedException();

            Destroyer.DestroyChildren<NoteUI>(container);
            notes = new List<NoteUI>();

            var _notes = map.GetNotes(time, controller.scroller.maxTime);
            foreach (Note n in _notes)
            {
                
            }
        }

        public NoteUI CreateTrackNote(Note n)
        {
            NoteUI ui = Instantiate(controller.notePrefab, container);
            ui.gameObject.SetActive(true);
            ui.SetData(n, overlay);
            ui.UpdateTrackPosition(n, overlay);
            return ui;
        }
    }

    public static class Destroyer
    {
        public static void Destroy(GameObject o)
        {
            if (Application.isPlaying)
                GameObject.Destroy(o);
            else
                GameObject.DestroyImmediate(o);

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