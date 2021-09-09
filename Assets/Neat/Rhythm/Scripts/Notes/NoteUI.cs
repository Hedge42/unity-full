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
        public KeyOverlay overlay { get; private set; }
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

        public void SetData(Note n, KeyOverlay o)
        {
            note = n;
            overlay = o;

            UpdateText();
        }
        public void UpdateText()
        {
            // fret for now
            tmpMain.text = note.fret.ToString();
            tmpSub.text = note.FullName();
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
