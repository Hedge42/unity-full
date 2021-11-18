using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Extensions;
using Neat.Experimental;
using Neat.Attributes;

namespace Neat.Music
{
    [RequireComponent(typeof(Fretboard))]
    public class FretboardDisplaySetting : MonoBehaviour
    {
        private Fretboard _fretboard;
        private Fretboard fretboard => this.CacheGetComponent(ref _fretboard);

        [Disabled]
        public int minFret;
        public int maxFret;
        public Fret.BorderMode borderMode;
        public Fret.PlayableMode fretMode;
    }
}
