using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    [Serializable]
    public struct FretBounds
    {
        public int numStrings;
        public int numFrets;

        public FretBounds(int _strings, int _frets)
        {
            numStrings = _strings;
            numFrets = _frets;
        }
    }
}
