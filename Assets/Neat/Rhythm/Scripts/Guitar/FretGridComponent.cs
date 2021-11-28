using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Tools;

namespace Neat.Audio.Music
{
    public class FretGridComponent : MonoBehaviour
    {
        public FretGrid f;

        public Vector2 scale;

        [SerializeField] private FretBounds _bounds;
        [SerializeField] private Fretboard _fretboard;
        private Fretboard fretboard => this.CacheGetComponent(ref _fretboard);

        public RectTransform prefab;

#if UNITY_EDITOR
        private void OnValidate()
        {
            //_bounds.numStrings = fretboard.tuning.numStrings;
            //_bounds.numFrets = Fretboard.MAX_FRETS;


            f = new FretGrid(scale, _bounds);
        }
#endif
        public void Instantiate()
        {

        }
    }
}
