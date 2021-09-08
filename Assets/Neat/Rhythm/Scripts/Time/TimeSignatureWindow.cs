using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Neat.Music
{
    public class TimeSignatureWindow : MonoBehaviour
    {
        // singleton - can only edit one at a time
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

        // ui references - componentUI pattern?
        public Button offsetDown;
        public TMP_InputField offsetText;
        public Button offsetUp;
        public Button bpmDown;
        public TMP_InputField bpmText;
        public Button bpmUp;
        public TMP_InputField numText;
        public TMP_InputField denText;

        // data
        public TimeSignature cloned { get; private set; }
        public TimeSignatureUI ui { get; private set; }

        // references
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

        // mono
        private void Start()
        {
            SetEvents();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                ApplyAndClose();
            }
        }

        // functions
        public static void Open(TimeSignatureUI t)
        {
            instance.gameObject.SetActive(true);
            instance.Load(t);
        }
        public static void Close()
        {
            instance.gameObject.SetActive(false);
        }
        public static void Apply()
        {
            instance.ApplyChanges();
        }
        public static void ApplyAndClose()
        {
            instance.ApplyChanges();
            Close();
        }

        private void Load(TimeSignatureUI t)
        {
            // set data
            cloned = t.timeSignature.Clone();
            ui = t;

            // set position
            rect.position = t.rect.position;

            // set text
            UpdateText();
        }
        private void UpdateText()
        {
            bpmText.text = cloned.beatsPerMinute.ToString("f3");
            offsetText.text = cloned.offset.ToString("f3");
            numText.text = cloned.numerator.ToString();
            denText.text = cloned.denominator.ToString();
        }
        private void ApplyChanges()
        {
            ui.controller.chart.timingMap.Overwrite(ui.timeSignature, cloned);
            ui.controller.SetTime(cloned.offset);
        }

        private void SetEvents()
        {
            // offset
            offsetUp.onClick.AddListener(OnOffsetUp);
            offsetDown.onClick.AddListener(OnOffsetDown);
            offsetText.onSubmit.AddListener(OnOffsetTextChange);

            // bpm
            bpmUp.onClick.AddListener(OnBpmUp);
            bpmDown.onClick.AddListener(OnBpmDown);
            bpmText.onSubmit.AddListener(OnBpmTextChange);
            bpmText.onDeselect.AddListener(OnBpmTextChange);
            bpmText.onEndEdit.AddListener(OnBpmTextChange);

            // offset
            numText.onValueChanged.AddListener(OnNumTextChange);
            denText.onValueChanged.AddListener(OnDenTextChange);
        }
        private void OnBpmTextChange(string s)
        {
            print("bpm text changed");
            if (float.TryParse(s, out float f))
            {
                f = Mathf.Clamp(f, 20f, 300f);

                cloned.beatsPerMinute = f;
                UpdateBpmText();
            }
            else
            {
                UpdateBpmText();
            }
        }
        private void UpdateBpmText()
        {
            bpmText.text = cloned.beatsPerMinute.ToString("f3");
        }
        private void OnBpmUp()
        {
            cloned.beatsPerMinute += 1;
            UpdateBpmText();
        }
        private void OnBpmDown()
        {
            cloned.beatsPerMinute -= 1;
            UpdateBpmText();
        }

        private void OnOffsetTextChange(string s)
        {
            if (float.TryParse(s, out float f))
            {
                f = Mathf.Clamp(f, 20f, 300f);

                cloned.offset = f;

                UpdateOffsetText();
            }
            else
            {
                UpdateOffsetText();
            }
        }
        private void UpdateOffsetText()
        {
            offsetText.text = cloned.offset.ToString("f3");
        }
        private void OnOffsetUp()
        {
            cloned.offset += 1;
            UpdateOffsetText();
        }
        private void OnOffsetDown()
        {
            cloned.offset -= 1;
            UpdateOffsetText();
        }

        private void OnNumTextChange(string s)
        {
            if (int.TryParse(s, out int value))
            {
                value = Mathf.Clamp(value, 1, 64);
                cloned.numerator = value;
            }

            numText.text = cloned.numerator.ToString();
        }
        private void OnDenTextChange(string s)
        {
            if (int.TryParse(s, out int value))
            {
                value = Mathf.Clamp(value, 1, 64);
                cloned.denominator = value;
            }

            denText.text = cloned.denominator.ToString();
        }
    }
}