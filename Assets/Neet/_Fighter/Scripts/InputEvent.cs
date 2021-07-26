using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    public class InputEvent
    {
        /// <summary>
        /// Direction held in numpad-format
        /// </summary>
        public int dirNum { get; set; }

        /// <summary>
        /// Direction held as bit value
        /// </summary>
        public int dirBit { get; set; }

        /// <summary>
        /// Direction-press in numpad-format
        /// </summary>
        public int dirDownNum { get; set; }

        /// <summary>
        /// Direction-press as bit value
        /// </summary>
        public int dirDownBit { get; set; }

        /// <summary>
        /// Buttons held as bit value
        /// </summary>
        public int btn { get; set; }

        /// <summary>
        /// Buttons pressed as bit value
        /// </summary>
        public int btnDown { get; set; }

        public InputEvent()
        {
            dirNum = 5;
        }
    }
}
