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
        public Note note;
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

        public Color color
        {
            get => foreground.color;
            set => foreground.color = value;
        }

        public void SetData(Note n, KeyOverlay o)
        {
            note = n;
            overlay = o;

            UpdateText();
        }

        public void UpdateUI()
        {
            UpdateTransform();
            UpdateText();
            UpdateColor();
        }
        public void UpdateTransform()
        {
            // switch to left-alignment
            rect.pivot = new Vector2(0, rect.pivot.y);

            float dps = overlay.controller.ui.scroller.distancePerSecond;
            var overlayNote = overlay.existing[note.lane];

            // note length
            var length = note.timeSpan.duration * dps;
            rect.sizeDelta = new Vector2(length, overlayNote.rect.sizeDelta.y);
            // print("Timespan: " + note.timeSpan.duration + " — Length: " + length);

            // position calculations
            var x = note.timeSpan.on * overlay.controller.ui.scroller.distancePerSecond;
            var y = overlayNote.rect.position.y;

            // anchored x, global y
            rect.anchoredPosition = new Vector2(x, 0);
            rect.position = new Vector3(rect.position.x, y);
        }
        public void UpdateText()
        {
            // fret for now
            if (note.GetType() == typeof(Note))
            {
                var gNote = (Note)note;
                tmpMain.text = gNote.fret.ToString();
                tmpSub.text = gNote.FullName();
            }
            else
            {
                tmpMain.text = note.Name();
                tmpSub.text = "";
            }
        }
        public void UpdateColor()
        {
            foreground.color = overlay.GetComponent<ColorPaletteUI>().colors[note.lane];
        }
        public void SetAlpha(float f)
        {
            color = color.SetAlpha(f);
        }

        public void SetInput<T>() where T : UIEventHandler
        {
            if (eventHandler != null)
                Destroyer.Destroy(eventHandler);

            eventHandler = btn.gameObject.AddComponent<T>();
        }
        public void Select(bool enabled)
        {
            if (enabled)
            {
                // select
                foreground.color = overlay.pallete.selectedColor;

                // mask.enabled = value;
            }
            else
            {
                // deselect
                foreground.color = overlay.pallete.colors[note.lane];
            }
        }
        public Vector3[] Bounds()
        {
            var arr = new Vector3[4];
            rect.GetWorldCorners(arr);
            return arr;
        }

        public static new UnityEngine.Object Instantiate(UnityEngine.Object prefab)
        {
            return null;
        }
    }
}
