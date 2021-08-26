using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Guitar
{
    [CustomEditor(typeof(SongScroller))]
    public class SongScrollerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (SongScroller)target;
        }
    }
}
