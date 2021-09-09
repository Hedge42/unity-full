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

        public Transform container;


        public List<NoteUI> notes;

        public NoteUI CreateTrackNote(Note n)
        {
            // new gameobject with data
            NoteUI ui = Instantiate(overlay.existing[n.lane], container);
            ui.gameObject.SetActive(true);
            ui.SetData(n, overlay);
            ui.SetInput<NoteInputHandler>();
            
            // setting up
            UpdateTransform(ui);
            return ui;
        }

        public void UpdateTransform(NoteUI ui)
        {
            // switch to left-alignment
            ui.rect.pivot = new Vector2(0, ui.rect.pivot.y);

            float dps = controller.ui.scroller.distancePerSecond;

            // note length
            var overlayNote = overlay.existing[ui.note.lane];
            ui.rect.sizeDelta = overlayNote.rect.sizeDelta; // duration ???

            // position calculations
            var x = ui.note.on * controller.ui.scroller.distancePerSecond;
            var y = overlayNote.rect.position.y;

            // anchored x, global y
            ui.rect.anchoredPosition = new Vector2(x, 0);
            ui.rect.position = new Vector3(ui.rect.position.x, y);

            print("Note placed: " + ui.note.FullFullName());
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