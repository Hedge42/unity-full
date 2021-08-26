using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Neet.Audio;
using Neet.File;

namespace Neet.Guitar
{
	using Input = UnityEngine.Input;
	public class SongScroller : MonoBehaviour
	{
		public MusicPlayer player;
		public Chart chart;

		public RectTransform movingContainer;
		public float distancePerSecond;
		// public event Action onFinish;

		private string songPath;

        private void Start()
        {
			player.onTick += UpdateTime;

			if (chart == null)
            {
				chart = new Chart();
            }
        }
		private void UpdateTime(float f)
        {
			movingContainer.anchoredPosition = Vector2.left * f * distancePerSecond;
        }

        public void Update()
		{
		}
	}
}