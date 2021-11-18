using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    public class FretGrid
    {
        public Vector2[,] positions { get; private set; }
        public Vector2 this[int i, int j] => positions[i, j];

        public float[] widths { get; private set; }
        public float[] points { get; private set; }


        public Vector2 scale;
        public FretBounds bounds;

        public Vector2 endPoint { get; private set; }

        public FretGrid(Vector2 _scale, FretBounds _bounds)
        {
            this.scale = _scale;
            this.bounds = _bounds;

            positions = Positions(_scale, _bounds);
            endPoint = EndPoint();
            widths = Widths();
        }

        private float[] GetWidths(float[] positions)
        {
            // positions -> widths
            float[] widths = new float[positions.Length - 1];
            for (int i = 0; i < positions.Length - 1; i++)
            {
                widths[i] = positions[i + 1] - positions[i];
            }
            return widths;
        }
        private float[] GetPositions(float[] widths)
        {
            // widths -> positions
            float[] positions = new float[widths.Length + 1];
            positions[0] = 0f;

            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + widths[i - 1];
            }
            return positions;
        }

        private float[] Widths()
        {
            float[] _widths = new float[bounds.numFrets - 1];
            _widths[0] = positions[1, 0].x - positions[0, 0].x;
            for (int i = 1; i < _widths.Length; i++)
                _widths[i] = positions[i, 0].x - positions[i - 1, 0].x;
            return _widths;
        }
        public Vector2 EndPoint()
        {
            var x = scale.x * FretPoint(bounds.numFrets);
            var y = scale.y * bounds.numStrings;

            return new Vector2(x, y);
        }
        public static Vector2[,] Positions(Vector2 _scale, FretBounds _bounds)
        {
            int _numStrings = _bounds.numStrings;
            int _numFrets = _bounds.numFrets;

            var _grid = new Vector2[_numStrings, _numFrets];

            float[] fretPoints = FretPoints(_numFrets, _scale.x);
            float[] stringPoints = StringDistances(_numStrings, _scale.y);

            for (int i_string = 0; i_string < _numStrings; i_string++)
            {
                for (int j_fret = 0; j_fret < _numFrets; j_fret++)
                {
                    var x = fretPoints[j_fret];
                    var y = stringPoints[i_string];

                    _grid[i_string, j_fret] = new Vector2(x, y);
                }
            }

            return _grid;
        }
        public static float[] StringDistances(int _length, float _scale = 1f)
        {
            float[] y_ = new float[_length];

            for (int i_string = 0; i_string < _length; i_string++)
            {
                y_[i_string] = _scale * (float)i_string;
            }

            return y_;
        }
        public static float[] FretPoints(int _length, float _scale = 1f)
        {
            // integral method

            float[] positions = new float[_length];

            for (int i_fret = 0; i_fret < _length; i_fret++)
            {
                positions[i_fret] = _scale * FretPoint(i_fret);
            }

            return positions;
        }
        public static float FretPoint(int i_fret, float _scale = 1f)
        {
            // with a scale of 1

            // using integration...
            // https://www.wolframalpha.com/input/?i=y+%3D+0.5+%5E+%28x+%2F+12%29
            float value = -17.3123f * Mathf.Exp(-0.0577623f * (float)i_fret);
            return _scale * value;
        }
        public static float FretWidth(int i_fret, float _scale = 1f)
        {
            float value = _scale * Mathf.Pow(.5f, (float)i_fret / 12);
            return value;
        }
        public static float[] Uniform(int numSegments, float size)
        {
            float[] arr = new float[numSegments];
            for (int i = 0; i < arr.Length; i++)
            {
                var s = ((float)i / (float)numSegments) * size;
                arr[i] = i;
            }
            return arr;
        }
        public static void Uniform(Fretboard f)
        {

        }

        public static float[] DistancesRecursive(float _scale, int _length)
        {
            // 'recursive' method

            float[] arr = new float[_length];
            arr[0] = 0f;

            for (int fret_i = 1; fret_i < _length; fret_i++)
            {
                float fret_f = (float)fret_i;
                float _width = _scale * Mathf.Pow(.5f, fret_f / 12f);

                arr[fret_i] = arr[fret_i - 1] + _width;
            }

            return arr;
        }

        Vector2 Get(Vector2 _scale, int _string, int _fret)
        {
            float f_string = (float)_string;
            float f_fret = (float)_fret;

            // Δx = (0.5) ^ (x / 12)
            var x = _scale.x * Mathf.Pow(.5f, _fret / 12f);
            var y = _scale.y * _string;

            return new Vector2(x, y);
        }
    }
}
