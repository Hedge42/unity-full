using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// exp
namespace Neat.Music
{
    public class TimeSpan
    {
        public float on;
        public float off;

        public float duration => off - on;

        public string label => on.ToString("f3") + "-" + off.ToString("f3");

        public TimeSpan(float on, float off)
        {
            this.on = on;
            this.off = off;
        }

    }
}
