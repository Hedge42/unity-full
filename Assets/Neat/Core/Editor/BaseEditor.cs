using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    // [CustomEditor(typeof(object), true)]
    public class BaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // EditorScriptField(this);
            base.OnInspectorGUI();
        }

        
    }
}
