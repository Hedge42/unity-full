using System.Collections;
using System.Collections.Generic;

namespace Neet.Guitar
{
    public class Chart
    {
        // data
        public string name;
        public float duration;

        public TimeSignature[] timeSignatures;

        public Chart()
        {
            name = "name";
            duration = 15; // seconds

            timeSignatures = new TimeSignature[] { new TimeSignature() };
        }
    }
}
