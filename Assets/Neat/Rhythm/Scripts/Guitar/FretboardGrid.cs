using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Music
{
    public class FretboardGrid
    {

        float spacing;
        float width;
        float length;


        Vector2 GetPosition(int x, int y)
        {
            return new Vector2(GetX(x), GetY(y));
        }
        float GetX(int column)
        {
            return spacing * Mathf.Pow(2f, column / 12f);
        }
        float GetY(int row)
        {
            return spacing * row;
        }
    }
}
