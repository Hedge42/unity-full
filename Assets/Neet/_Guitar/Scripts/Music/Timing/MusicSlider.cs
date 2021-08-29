using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Neat.Music
{
    // instead: audio slider
    [RequireComponent(typeof(Slider))]
    public class MusicSlider : MonoBehaviour
    {
        // TODO
        public MusicPlayer player;

        public TextMeshProUGUI tmp;

        public UnityEvent<float> onDragStart;
        public UnityEvent<float> onDragEnd;
        public UnityEvent<float> onDrag;

        public bool interactable
        {
            get { return slider.interactable; }
            set { slider.interactable = value; }
        }
        public float value
        {
            get { return slider.value; }
            set { slider.value = value; }
        }
        public float range
        {
            get { return slider.maxValue; }
            set { slider.maxValue = value; }
        }
        public UnityEvent<float> onValueChanged
        {
            get { return slider.onValueChanged; }
            set { slider.onValueChanged = (Slider.SliderEvent)value; }
        }


        private Slider _slider;
        private Slider slider
        {
            get
            {
                if (_slider == null)
                    _slider = GetComponent<Slider>();
                return _slider;
            }
        }

        private void Start()
        {
            slider.onValueChanged.AddListener(UpdateText);
            slider.interactable = false;
        }

        public void UpdateTime(float value)
        {
            value = Mathf.Clamp(value, 0f, range);
            slider.value = value;
        }

        private void UpdateText(float value)
        {
            value = Mathf.Clamp(value, 0f, range);

            // idk how this works lol
            // https://stackoverflow.com/questions/40867158/how-can-i-format-a-float-number-so-that-it-looks-like-real-time
            tmp.text = $"{(int)value / 60}:{value % 60:00.000}";
            // tmpTime.text = value.ToString("f3");
        }

        public void UpdateRange()
        {
            slider.maxValue = range;
        }

        private bool isDragging;
        public void OnValueChanged()
        {
            onValueChanged?.Invoke(slider.value);

            if (isDragging)
                onDrag?.Invoke(slider.value);
        }
        public void DragStart()
        {
            isDragging = true;
            onDragStart?.Invoke(slider.value);
        }
        public void PointerUp()
        {
            isDragging = false;
            onDragEnd?.Invoke(slider.value);
        }
    }
}