using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neat.Music
{
    public class TimeSignatureWindow : MonoBehaviour
    {
        // can only edit one at once
        private static TimeSignatureWindow _instance;
        public static TimeSignatureWindow instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<TimeSignatureWindow>(true);
                return _instance;
            }
        }

        // ui references
        public Button bpmUp;
        public Button bpmDown;
        public TMP_InputField bpmText;
        public TMP_InputField numText;
        public TMP_InputField denText;

        // data
        private TimeSignature timeSignature;
        private float bpm;
        private int num;
        private int den;


        private ChartController _controller;
        public ChartController controller
        {
            get
            {
                if (_controller == null)
                    _controller = FindObjectOfType<ChartController>();
                return _controller;
            }
        }
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

        private void Start()
        {
            SetEvents();
        }

        public static void Edit(TimeSignatureUI t)
        {
            instance.gameObject.SetActive(true);
            instance.Load(t);
        }

        public void Load(TimeSignatureUI t)
        {
            SetTimeSignature(t.timeSignature);
            rect.anchoredPosition = t.rect.anchoredPosition;
        }
        public static void Close()
        {
            instance.gameObject.SetActive(false);
        }

        public void SetTransform(TimeSignatureUI t)
        {
            rect.SetParent(t.transform);
        }

        public void SetTimeSignature(TimeSignature t)
        {
            timeSignature = t;
            bpm = t.beatsPerMinute;
            num = t.numerator;
            den = t.denominator;

            bpmText.text = t.beatsPerMinute.ToString("f3");
            // offsetText.text = t.offset.ToString("f3");
            numText.text = t.numerator.ToString();
            denText.text = t.denominator.ToString();
        }
        private void ApplyChanges()
        {
            timeSignature.beatsPerMinute = bpm;
            timeSignature.numerator = num;
            timeSignature.denominator = den;
        }

        private void SetEvents()
        {
            bpmUp.onClick.AddListener(OnBpmUp);
            bpmDown.onClick.AddListener(OnBpmDown);

            // bpmText.onValueChanged.AddListener(OnBpmTextChange);
            bpmText.onSubmit.AddListener(OnBpmTextChange);
            bpmText.onDeselect.AddListener(OnBpmTextChange);
            bpmText.onEndEdit.AddListener(OnBpmTextChange);

            numText.onValueChanged.AddListener(OnNumTextChange);
            denText.onValueChanged.AddListener(OnDenTextChange);
        }
        private void OnBpmTextChange(string s)
        {
            print("bpm text changed");
            if (float.TryParse(s, out float f))
            {
                f = Mathf.Clamp(f, 20f, 300f);

                bpm = f;
                UpdateBpmText();
            }
            else
            {
                UpdateBpmText();
            }
        }
        private void UpdateBpmText()
        {
            bpmText.text = bpm.ToString("f3");
        }
        private void OnBpmUp()
        {
            print("bpm up click");
            bpm += 1;
            UpdateBpmText();
        }
        private void OnBpmDown()
        {
            print("bpm down click");
            bpm -= 1;
            UpdateBpmText();
        }
        private void OnNumTextChange(string s)
        {
            if (int.TryParse(s, out int value))
            {
                value = Mathf.Clamp(value, 1, 64);
                num = value;
            }

            numText.text = num.ToString();
        }
        private void OnDenTextChange(string s)
        {
            if (int.TryParse(s, out int value))
            {
                value = Mathf.Clamp(value, 1, 64);
                den = value;
            }

            denText.text = den.ToString();
        }
    }
}