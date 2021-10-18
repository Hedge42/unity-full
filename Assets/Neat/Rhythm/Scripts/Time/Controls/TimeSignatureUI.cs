using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Neat.Music
{
    // represent a TimeSignature as a GameObject
    public class TimeSignatureUI : MonoBehaviour
    {
        // idea - 
        // list of text items above a beat bar
        // preserve y- values on invalid beats, or not?

        // references
        public TextMeshProUGUI tmp;
        public Button btn;

        private RectTransform _rect;
        public RectTransform rect
        {
            get
            {
                if (_rect == null)
                    _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }

        public TimeSignature timeSignature { get; private set; }

        public ChartPlayer controller { get; private set; }

        public static TimeSignatureUI Create(GameObject prefab, Transform container, TimeSignature t, ChartPlayer controller)
        {
            var mono = GameObject.Instantiate(prefab, container).GetComponent<TimeSignatureUI>();
            mono.UpdateTimeSignature(t);
            mono.btn.onClick.AddListener(mono.ToggleWindow);
            mono.rect.anchoredPosition = Vector2.zero;
            mono.gameObject.SetActive(true);
            mono.controller = controller;
            return mono;
        }

        private void ToggleWindow()
        {
            bool close = TimeSignatureWindow.instance.gameObject.activeInHierarchy
                && TimeSignatureWindow.instance.ui == this;

            if (close)
            {
                TimeSignatureWindow.Close();
            }
            else
            {
                TimeSignatureWindow.Open(this);
            }
        }

        public void UpdateTimeSignature(TimeSignature t)
        {
            this.timeSignature = t;

            tmp.text = t.numerator + "/" + t.denominator + "\n" + t.beatsPerMinute.ToString("f0");
        }
    }
}
