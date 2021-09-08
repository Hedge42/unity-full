using UnityEngine;
using UnityEditor;

namespace Neat.Music
{

    [CustomEditor(typeof(NoteOverlay))]
    public class NoteOverlayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var _target = (NoteOverlay)target;

            if (GUILayout.Button("Destroy"))
            {
                _target.DestroyUI();
            }
            else if (GUILayout.Button("From Tuning"))
            {
                _target.UpdateOverlay(new GuitarTuning().Notes());
            }
        }
    }
}
