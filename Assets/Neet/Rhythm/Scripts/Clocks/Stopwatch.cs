using System;
using System.Collections;
using UnityEngine;

namespace Neat.States
{
    public class Stopwatch : ClockState
    {
        private float time;
        public bool isActive
        {
            get;
            private set;
        }

        private MonoBehaviour mono;

        public event Action<float> onTick;

        public Stopwatch(MonoBehaviour mono)
        {
            this.mono = mono;
        }
        public float GetTime()
        {
            return time;
        }
        public void SetTime(float t)
        {
            time = Mathf.Max(t, 0f);
        }

        public void Start()
        {
            isActive = true;
            mono.StartCoroutine(_Start());
        }
        public IEnumerator _Start()
        {
            while (isActive)
            {
                time += Time.deltaTime;
                onTick?.Invoke(time);
                yield return new WaitForEndOfFrame();
            }
        }
        public void Pause()
        {
            isActive = false;
        }
        public void Stop()
        {
            Pause();
            SetTime(0f);
        }
    }
}