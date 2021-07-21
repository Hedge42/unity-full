using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neet.Fighter
{
    [System.Serializable]
    public class VectorInterpolation
    {
        public float changeX;
        public float changeY;

        public Vector2 change
        {
            get
            {
                return new Vector2(changeX, changeY);
            }
            set
            {
                changeX = value.x;
                changeY = value.y;
            }
        }

        public int startFrame = 0;
        public int endFrame = 10;
        public float power = 1f;

        public int frames { get { return endFrame - startFrame; } }


        public Vector2 Interpolate(int frame)
        {
            float t = (float)(frame - startFrame) / frames;

            return Vector2.Lerp(Vector2.zero, change, Mathf.Pow(t, power));
        }
        public Vector2 Delta(int frame)
        {
            return Interpolate(frame) - Interpolate(frame - 1);
        }

        public static Vector2 Calculate(int frame, List<VectorInterpolation> list)
        {
            Vector2 change = Vector2.zero;
            foreach (var v in list)
            {
                if (frame >= v.startFrame && frame <= v.endFrame)
                    change += v.Interpolate(frame);
            }
            return change;
        }
    }
}
