using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Audio.Music
{
    [CreateAssetMenu(menuName = "Neat/Color Palette")]
    public class ColorPaletteObject : ScriptableObject
    {
        public Color baseColor;

        public List<Color> colors;

        //[Gra]
        public Gradient gradient;

        [ContextMenu("Make spectrum")]
        public void Spectrum()
        {
            colors = baseColor.Spectrum(colors.Count);
        }

        public SerializableColor[] ToSerializable()
        {
            var arr = new SerializableColor[colors.Count];

            for (int i = 0; i < colors.Count; i++)
                arr[i] = new SerializableColor(colors[i]);

            return arr;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            colors = baseColor.Spectrum(colors.Count);
        }
#endif
    }
}
