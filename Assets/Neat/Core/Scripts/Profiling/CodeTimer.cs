using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Tools;
using Debug = UnityEngine.Debug;

namespace Neat.Tools
{
    // this is the table
    [Serializable]
    public class PerformanceLog
    {
        // TODO
        public string name;
        public List<PerformanceElement> elements;
        public void Log()
        {
            // use stringBuilder for custom logger
            var x = new StringBuilder();
            x.AppendLine($"{elements.Count} entries for {name}:");

            for (int i = 0; i < elements.Count; i++)
            {
                var t = elements[i];
                x.AppendLine($"\n\t[{i}] {t.timeSpan.TotalMilliseconds}ms");
            }
            Debug.Log(x.ToString());
        }

        // calculated properties
        public TimeSpan max => elements.OrderByDescending(t => t.timeSpan.Ticks).FirstOrDefault().timeSpan;
        public TimeSpan min => elements.OrderBy(t => t.timeSpan.Ticks).FirstOrDefault().timeSpan;

        public double averageTicks => elements.Average(e => e.timeSpan.Ticks);
        public TimeSpan average => new TimeSpan((long)averageTicks); // timy amt of long precision

        public double callsPerSecond => elements.Count / new TimeSpan(lastCall.Ticks - firstCall.Ticks).TotalSeconds;

        public DateTime firstCall => elements.FirstOrDefault().startTime;
        public DateTime lastCall => elements.Last().startTime;
    }
}
