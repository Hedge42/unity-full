using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Neat.States;

namespace Neat.Music
{
    // please rename me
    [ExecuteAlways]
    public class NoteHighway : MonoBehaviour, OutputState, Loadable
    {
        // public NoteUI notePrefab;

        public RectTransform judgementLine;
        public TextMeshProUGUI judgementText;
        public RectTransform movingContainer;
        public RectTransform staticWindow;

        [SerializeField] private ChartPlayer _player;
        [Range(0, 2)] public float approachRate; // set me


        public ChartPlayer player
        {
            get
            {
                if (_player == null)
                    _player = GetComponent<ChartPlayer>();
                return _player;
            }
        }

        private KeyOverlay _overlay;
        public KeyOverlay overlay => _overlay == null ? _overlay = GetComponent<KeyOverlay>() : _overlay;

        public HighwayInputHandler input => GetComponent<HighwayInputHandler>();

        public float distancePerSecond
        {
            get
            {

                return distance / approachRate;
            }
        }
        public float distance
        {
            get
            {
                // from the judgement line to the end of the window
                return staticWindow.rect.width - judgementLine.anchoredPosition.x;

            }
        }

        public void UpdateTime(float newTime)
        {
            SetTime(newTime);
        }
        public void SetTime(float newTime)
        {
            // update text and position
            judgementText.text = newTime.ToString("f3");
            //movingContainer.anchoredPosition = Vector2.left * newTime * distancePerSecond;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            // moving backwards
            movingContainer.anchoredPosition = judgementLine.anchoredPosition
                - GetLocalPosition(_player.time);
        }

        public Vector2 GetLocalPosition(float time)
        {
            return Vector2.right * time * distancePerSecond;
        }
        private Vector2 GetAnchoredPosition(float time)
        {
            return judgementLine.anchoredPosition + GetLocalPosition(time);
        }
        private void UpdateWidth(AudioClip clip)
        {
            var newx = clip.length * distancePerSecond;
            movingContainer.sizeDelta = new Vector2(newx, movingContainer.sizeDelta.y);
        }
        public void OnLoad(Chart c)
        {
            // ??
            // player = GetComponent<ChartPlayer>();
            player.player.onClipReady.RemoveListener(UpdateWidth);
            player.player.onClipReady.AddListener(UpdateWidth);

            Debug.Log(GetType().ToString() + " loading...", this);
            SetTime(0f);
        }
    }
}
