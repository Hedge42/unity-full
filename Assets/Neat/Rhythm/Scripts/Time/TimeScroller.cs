using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Neat.Music;

namespace Neat.States
{
    public class TimeScroller : MonoBehaviour, OutputState
    {
        public RectTransform judgementLine;
        public TextMeshProUGUI judgementText;
        public RectTransform movingContainer;
        public RectTransform staticWindow;
        public float distancePerSecond;

        private float time;
        public float maxTime
        {
            get
            {
                var offset = staticWindow.rect.width - judgementLine.anchoredPosition.x;
                return time + (offset / distancePerSecond);
            }
        }
        public float minTime
        {
            get
            {
                return time - (judgementLine.anchoredPosition.x / distancePerSecond);
            }
        }

        public Vector3 GetPosition(float time)
        {
            return judgementLine.position + GetLocalPosition(time);
        }
        public Vector3 GetLocalPosition(float time)
        {
            return Vector3.right* time *distancePerSecond;
        }

        public void SetTime(float time)
        {
            Set(time);
        }

        public void UpdateTime(float time)
        {
            Set(time);
        }

        private void Set(float time)
        {
            // update text and position
            this.time = time;
            judgementText.text = time.ToString("f3");
            movingContainer.anchoredPosition = Vector2.left * time * distancePerSecond;
        }

        public void UpdateWidth(AudioClip clip)
        {
            var newx = clip.length * distancePerSecond;
            movingContainer.sizeDelta = new Vector2(newx, movingContainer.sizeDelta.y);
        }
    }
}
