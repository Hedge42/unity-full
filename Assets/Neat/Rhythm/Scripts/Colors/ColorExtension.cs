using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Audio.Music
{
    public static class ColorExtension
    {
        public static List<Color> Spectrum(this Color startColor, int count)
        {
            List<Color> colors = new List<Color>();

            Color c = startColor;
            Color hsva = c.HSV4();
            Debug.Log("HSVA: " + hsva.ToString());
            // Color v = c.HSV4(); // this works?

            float hue = hsva.r;
            float shift = 1f / (float)count;

            for (int i = 0; i < count; i++)
            {
                Color rgb = Color.HSVToRGB(hue, hsva.g, hsva.b);
                Color _ = new Color(rgb.r, rgb.g, rgb.b, startColor.a);
                colors.Add(_);

                hue += shift;
                hue %= 1f;
            }
            return colors;
        }

        private static Color[] HSV(this Color[] colors)
        {
            Color[] f = new Color[colors.Length];

            for (int i = 0; i < colors.Length; i++)
                f[i] = colors[i].HSV();

            return f;
        }
        private static Color HSV(this Color rgb)
        {
            float h, s, v, a;
            Color.RGBToHSV(rgb, out h, out s, out v);
            a = rgb.a;

            return new Color(h, s, v, a);
        }
        private static Vector4 HSV4(this Color color)
        {
            float h, s, v, a;
            Color.RGBToHSV(color, out h, out s, out v);
            a = color.a;

            return new Vector4(h, s, v, a);
        }

        /// <param name="alpha">clapmed [0,1]</param>
        public static Color SetAlpha(this Color color, float alpha)
        {
            alpha = Mathf.Clamp(alpha, 0f, 1f);
            return new Color(color.r, color.g, color.b, alpha);
        }

        // generic extension methods???
        public static Color SetAlpha<T>(this T _t, float alpha) where T : class
        {
            return Color.red;

        }

        // public static T2 CacheComponent<T1, T2>(this T1 _this, ref T2 _data) where T1 : MonoBehaviour
        
    }
}
