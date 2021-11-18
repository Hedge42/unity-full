using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neat.Music
{
    [Serializable]
    public class ColorPalette
    {
        public ColorPaletteUI ui;
        private List<SerializableColor> __colors;
        public ColorPalette(ColorPaletteUI ui)
        {
            this.ui = ui;

            __colors = new List<SerializableColor>();
            __colors = ui.colors.Select(c => new SerializableColor(c)).ToList();
        }

        private List<SerializableColor> _colors;

        public Color selectedColor;
        public Color startColor;
        public List<Color> colors;

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/using-indexers
        public Color this[int index]
        {
            get
            {
                return colors[index];
            }
        }


        public List<Color> Spectrum(Color startColor, int count)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < count; i++)
            {
                float t = (float)i / (float)count;
                colors.Add(Shift(startColor, t));
            }
            return colors;
        }
        public List<Color> Alternate(List<Color> colors)
        {
            var outputs = new Color[colors.Count];
            for (int i = 0; i < colors.Count; i++)
            {
                // mod odd indexes
                bool mod = i % 2 == 1;
                if (mod)
                {
                    int nextOddIndex = (i + 2) % colors.Count;
                    if (nextOddIndex % 2 == 0)
                        nextOddIndex += 1;
                    outputs[i] = colors[nextOddIndex];
                }
                else
                    outputs[i] = colors[i];
            }
            return new List<Color>(outputs);
        }
        public List<Color> Alternate2(List<Color> colors)
        {
            var outputs = new List<Color>();
            // foreach even index
            for (int i = 0; i < colors.Count; i += 2)
                outputs.Add(colors[i]);

            // foreach odd index
            for (int i = 1; i < colors.Count; i += 2)
                outputs.Add(colors[i]);

            return outputs;
        }

        public Color NextChannel(Color input)
        {
            var output = new Color();
            output.g = input.r;
            output.b = input.g;
            output.r = input.b;
            output.a = input.a;
            return output;
        }
        private Color Shift(Color a, float t)
        {
            Color b = NextChannel(a);
            Color c = NextChannel(b);

            int i = GetColorIndex(t);
            float local_t = GetLocalT(t);

            if (i == 0)
                return Color.Lerp(a, b, local_t);
            else if (i == 1)
                return Color.Lerp(b, c, local_t);
            else if (i == 2)
                return Color.Lerp(c, a, local_t);
            else
                throw new System.ArgumentException();
        }
        private int GetColorIndex(float t)
        {
            return (int)(t * 3f);
        }
        private float GetLocalT(float t)
        {
            return (t * 3f) - (float)GetColorIndex(t);
        }

        public override string ToString()
        {
            return colors.ToArray().ToString();
        }

        public string FilePath()
        {
            return "OK";
        }
    }

    public class ColorPaletteUI : MonoBehaviour
    {
        [SerializeReference]
        private ColorPalette _data;
        private ColorPalette data
        {
            get
            {
                if (_data == null || _data.ui != this)
                    _data = new ColorPalette(this);
                return _data;
            }
        }

        public Color selectedColor;
        public Color startColor;
        public List<Color> colors;

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/using-indexers
        public Color this[int index]
        {
            get
            {
                return colors[index];
            }
        }


        public List<Color> Spectrum(Color startColor, int count)
        {
            List<Color> colors = new List<Color>();
            for (int i = 0; i < count; i++)
            {
                float t = (float)i / (float)count;
                colors.Add(Shift(startColor, t));
            }
            return colors;
        }
        public List<Color> Alternate(List<Color> colors)
        {
            var outputs = new Color[colors.Count];
            for (int i = 0; i < colors.Count; i++)
            {
                // mod odd indexes
                bool mod = i % 2 == 1;
                if (mod)
                {
                    int nextOddIndex = (i + 2) % colors.Count;
                    if (nextOddIndex % 2 == 0)
                        nextOddIndex += 1;
                    outputs[i] = colors[nextOddIndex];
                }
                else
                    outputs[i] = colors[i];
            }
            return new List<Color>(outputs);
        }
        public List<Color> Alternate2(List<Color> colors)
        {
            var outputs = new List<Color>();
            // foreach even index
            for (int i = 0; i < colors.Count; i += 2)
                outputs.Add(colors[i]);

            // foreach odd index
            for (int i = 1; i < colors.Count; i += 2)
                outputs.Add(colors[i]);

            return outputs;
        }

        public Color NextChannel(Color input)
        {
            var output = new Color();
            output.g = input.r;
            output.b = input.g;
            output.r = input.b;
            output.a = input.a;
            return output;
        }
        private Color Shift(Color a, float t)
        {
            Color b = NextChannel(a);
            Color c = NextChannel(b);

            int i = GetColorIndex(t);
            float local_t = GetLocalT(t);

            if (i == 0)
                return Color.Lerp(a, b, local_t);
            else if (i == 1)
                return Color.Lerp(b, c, local_t);
            else if (i == 2)
                return Color.Lerp(c, a, local_t);
            else
                throw new System.ArgumentException();
        }
        private int GetColorIndex(float t)
        {
            return (int)(t * 3f);
        }
        private float GetLocalT(float t)
        {
            return (t * 3f) - (float)GetColorIndex(t);
        }
    }
}
