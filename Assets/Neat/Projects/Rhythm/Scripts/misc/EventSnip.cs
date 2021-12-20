using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


namespace Neat.Experimental
{
    public class EventBased
    {
        // less code
        // less security
        // less information

        public event Action<float> onTick;
        public event Action<float> onSkip;

        public void Tick(float time)
        {
            onTick?.Invoke(time);
        }
        public void Skip(float time)
        {
            onSkip?.Invoke(time);
        }
    }
    public class Interfaced
    {
        // more code
        // more security
        // more information

        public interface Tickable
        {
            void Skip(float time);
            void Tick(float time);
        }


        public List<Tickable> tickables = new List<Tickable>();

        public void Add(Tickable t)
        {
            if (!tickables.Contains(t))
                tickables.Add(t);
            else
                Debug.Log("Duplicate entry");
        }
        public void Tick(float time)
        {
            foreach (var t in tickables)
                t.Tick(time);
        }
        public void Skip(float time)
        {
            foreach (var t in tickables)
                t.Skip(time);
        }
    }
}