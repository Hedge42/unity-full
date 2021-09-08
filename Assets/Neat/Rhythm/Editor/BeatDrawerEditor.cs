using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Music
{
    [CustomEditor(typeof(TimingBar))]
    public class BeatDrawerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (TimingBar)target;
        }
    }
}
