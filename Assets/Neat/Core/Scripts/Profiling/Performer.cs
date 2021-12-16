using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
// using Debug = Neat.Debugger.Debug;

namespace Neat.Tools
{
    public class Performer
    {
        public string name;
        public TimeSpan timeSpan;
        public DateTime startTime;
        public Stopwatch stopwatch;

        public static Performer Start(string name = "")
        {
            var element = new Performer()
            {
                name = name,
                stopwatch = new Stopwatch(),
                startTime = DateTime.Now,
                timeSpan = new TimeSpan()
            };

            element.stopwatch.Start();
            return element;
        }
        public void Stop(string message)
        {
            Stop();
            Debug.Log($"{message} ({timeSpan.Milliseconds}ms)");
        }
        public void Stop()
        {
            stopwatch.Stop();
            timeSpan = stopwatch.Elapsed;
        }

        public string milliseconds => $"({stopwatch.Elapsed.TotalMilliseconds} ms)";

        public override string ToString()
        {
            return milliseconds;
        }
    }
}