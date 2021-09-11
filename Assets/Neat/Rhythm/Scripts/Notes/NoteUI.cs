using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Neat.States;

namespace Neat.Music
{
    public class NoteUI : MonoBehaviour
    {
        // references

        // new NoteTextSetter(main, sub)
        // new GuitarNoteText(main, sub)
        // new PianoNoteText(main, sub)

        public TextMeshProUGUI tmpMain;
        public TextMeshProUGUI tmpSub;
        public Image foreground;
        public Image background;
        public Button btn;
        public RectMask2D mask;


        [HideInInspector]
        public TrackNote note;
        [HideInInspector]
        public UIEventHandler eventHandler;

        private RectTransform _rect;
        public RectTransform rect
        {
            get
            {
                if (!_rect)
                    _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }
        public KeyOverlay overlay { get; set; }

        public void SetData(TrackNote n, KeyOverlay o)
        {
            note = n;
            overlay = o;

            UpdateText();
        }
        public void UpdateText()
        {
            // fret for now
            if (note.GetType() == typeof(TrackNote))
            {
                var gNote = (TrackNote)note;
                tmpMain.text = gNote.fret.ToString();
                tmpSub.text = gNote.FullName();
            }
            else
            {
                tmpMain.text = note.Name();
                tmpSub.text = "";
            }
        }
        public void SetInput<T>() where T : UIEventHandler
        {
            if (eventHandler != null)
                Destroyer.Destroy(eventHandler);

            eventHandler = btn.gameObject.AddComponent<T>();
        }
        public void ToggleHighlight(bool value)
        {
            mask.enabled = value;
        }
    }
}
