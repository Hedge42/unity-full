using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat.Audio.Music
{
    public class Axis
    {
        public float[] positions;
        public float distance;
        public int count => positions.Length;

        public float this[int index] => positions[index];

        public Axis(float[] _values, float _length)
        {
            positions = _values;
            distance = _length;
        }

        public void Scale(float newDistance)
        {
            float mult = newDistance / distance;
            for (int i = 0; i < positions.Length; i++)
                positions[i] *= mult;
            distance = newDistance;
        }
        public float[] Distances()
        {
            float[] sizes = new float[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                sizes[i] = GetLength(i);
            return sizes;
        }
        public float GetLength(int i)
        {
            float f = 0f;
            if (i == count - 1)
                return distance - positions[i];
            else
                return positions[i + 1] - positions[i];
        }
        public void Invert()
        {
            for (int i = 0; i < positions.Length; i++)
                positions[i] = -positions[i];
        }

        public override string ToString()
        {
            string s = "[";
            foreach (var f in positions)
                s += f.ToString() + ",";
            s += "]";
            return s;
        }
    }
}
