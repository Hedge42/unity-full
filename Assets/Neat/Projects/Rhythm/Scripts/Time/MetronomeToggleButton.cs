using UnityEngine;
using UnityEngine.UI;

namespace Neat.Audio.Music
{
    [RequireComponent(typeof(Button))]
    public class MetronomeToggleButton : MonoBehaviour
    {
        public Metronome metronome;
        public Color disabledColor;

        private Color enabledColor; // startColor
        private Button _button;

        private Button button
        {
            get
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                return _button;
            }
        }

        private void Awake()
        {
            enabledColor = button.image.color;

            SetClickEvent();
        }

        private void SetClickEvent()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Click);
        }

        public void Click()
        {
            metronome.mute = !metronome.mute;

            UpdateColor(metronome.mute);
            UpdateImage();
        }
        private void UpdateColor(bool mute)
        {
            button.image.color = mute ? disabledColor : enabledColor;
        }
        private void UpdateImage()
        {
            // TODO
        }
    }
}
