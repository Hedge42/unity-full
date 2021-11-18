using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    [Serializable]
    public class SerializableColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color color
        {
            get
            {
                return new Color(r, g, b, a);
            }
            set
            {
                r = value.r;
                g = value.g;
                b = value.b;
                a = value.a;
            }
        }
        public SerializableColor(Color c)
        {
            color = c;
        }
    }
}