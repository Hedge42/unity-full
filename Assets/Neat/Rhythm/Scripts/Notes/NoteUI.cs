using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Neat.Music
{
    public class NoteUI : MonoBehaviour
    {
        public UIEventHandler eventHandler;

        public TextMeshProUGUI text;
        public Image background;

        public Note note;

        public NoteOverlay overlay { get; private set; }

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

        public void SetData(Note n, NoteOverlay o)
        {
            note = n;
            overlay = o;
            text.text = n.FullName();
        }

        public void UpdateTrackPosition(Note n, NoteOverlay o)
        {
            // requires reference to overlay
            float dps = o.controller.scroller.distancePerSecond;
            float width = n.duration * dps;
            float xPos = n.on * dps;
            float yPos = o.GetY(n.lane);
            rect.position = new Vector2(xPos, yPos);
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
        }
    }
}
