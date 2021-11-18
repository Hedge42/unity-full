using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Neat.Music
{
    public class FretboardSpacer : MonoBehaviour
    {
        private const int maxFret = 25;
        private Fretboard fretboard;

        public enum xAlignment
        {
            LeftToRight,
            RightToLeft
        }
        public enum yAlignment
        {
            BottomToTop,
            TopToBottom,
        }

        public xAlignment xA;
        public yAlignment yA;

        public static void Fix(Fretboard fretboard)
        {
            (Axis, Axis) grid = MakeGrid(fretboard.tuning, fretboard.size);
            Axis xAxis = grid.Item1;
            Axis yAxis = grid.Item2;

            foreach (Fret f in fretboard.frets)
            {
                var i = f.fretNum;
                var j = f.stringNum;

                f.rect.anchoredPosition = new Vector2(xAxis[i], -yAxis[j]);

                var width = xAxis.GetLength(i);
                var height = yAxis.GetLength(j);

                f.rect.sizeDelta = new Vector2(width, height);
            }
        }
        public void Fix()
        {
            fretboard = GetComponent<Fretboard>();
            Fix(fretboard);
        }

        public static (Axis, Axis) MakeGrid(GuitarTuning tuning, Vector2 size)
        {
            // var spacing = width / Mathf.Pow(2f, (float)maxFret / 12f);
            var _x = xAxisNormal();
            _x.Scale(size.x);
            var x = _x.positions;

            var _y = yAxisNormal(tuning.numStrings);
            _y.Scale(size.y);
            var y = _y.positions;

            return (_x, _y);
        }
        private static Axis xAxisNormal()
        {
            float distance = 0f;
            float[] positions = new float[maxFret];
            for (int i = 0; i < maxFret; i++)
            {
                float delta = Mathf.Pow(.5f, i / 12f);
                positions[i] = distance;
                distance += delta;
            }

            return new Axis(positions, distance);
        }
        private static Axis yAxisNormal(int numStrings)
        {
            var values = new float[numStrings];
            for (int i = 0; i < numStrings; i++)
                values[i] = i;
            return new Axis(values, numStrings);
        }


        private static float NormalWidth(int i)
        {
            return Mathf.Pow(.5f, i / 12f);
        }
        private static float NormalPos(int i)
        {
            return Mathf.Pow(2f, (float)i / 12f);
        }
    }
}
