using System;
using System.Collections;
namespace Neat.States
{
    public interface ClockState
    {
        bool isActive { get; }
        event Action<float> onTick;

        void Start();
        IEnumerator _Start();
        void Pause();
        void Stop();
        float GetTime();
        void SetTime(float t);
    }
}