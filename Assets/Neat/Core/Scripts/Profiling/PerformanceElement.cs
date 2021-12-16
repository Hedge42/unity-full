using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Neat.Tools
{
    public class PerformanceElement
    {
        public string name;
        public TimeSpan timeSpan;
        public DateTime startTime;
        public Stopwatch stopwatch;

        public static PerformanceElement Start(string name)
        {
            var element = new PerformanceElement()
            {
                name = name,
                stopwatch = new Stopwatch(),
                startTime = DateTime.Now,
                timeSpan = new TimeSpan()
            };

            element.stopwatch.Start();
            return element;
        }
        public PerformanceElement Stop()
        {
            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;
            Debug.Log($"{name}: stopped in {timeSpan.Milliseconds} milliseconds");
            //Debug.Log($"{name}: finished in {timeSpan.Ticks} Ticks");
            return this;
        }

        public override string ToString()
        {
            Debug.Log($"{name}: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
            return base.ToString();
        }
    }
}
