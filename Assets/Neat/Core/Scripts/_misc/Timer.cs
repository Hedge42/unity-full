using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Neat.Demos.AimTrainer
{
    public class Timer
    {
        public const string ELAPSED_KEY = "elapsed";

        public bool isActive { get; private set; }
        public float timeLimit { get; private set; }
        public float timeElapsed { get; private set; }
        public float timeRemaining { get { return timeLimit - timeElapsed; } }

        public event Action<float> onTimerTick;
        public event Action onTimerFinished;

        bool useUnscaledTime = false;



        public void StartTimer(float timeLimit)
        {
            timeElapsed = 0;
            isActive = true;
            this.timeLimit = timeLimit;
        }
        public void StartStopwatch()
        {
            timeElapsed = 0;
            isActive = true;
            timeLimit = 0;
        }
        public void UpdateTimer()
        {
            if (!isActive)
                return;

            float delta = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            timeElapsed += delta;
            onTimerTick?.Invoke(delta);

            if (timeLimit > 0 && timeElapsed >= timeLimit)
                TimerFinished();
        }
        public void PauseTimer(bool isPaused)
        {
            isActive = !isPaused;
        }
        public void TimerFinished()
        {
            isActive = false;
            onTimerFinished?.Invoke();
        }
    }
}