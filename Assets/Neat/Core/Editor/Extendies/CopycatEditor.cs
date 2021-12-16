using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    public class CopycatEditor : Editor
    {
        public static CopycatEditor CreateInstance(Object target)
        {
            //var obj = ScriptableObject.CreateInstance<CopycatEditor>();
            var obj = Editor.CreateEditor(target, typeof(CopycatEditor));
            return obj as CopycatEditor;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open in Editor Window"))
                InspectorWindow.Open(target);

            // https://gist.github.com/rutcreate/d550aa1ae4052e0a0b37
            SerializedProperty prop = serializedObject.GetIterator();
            CopycatHeader(prop);

            while (prop.NextVisible(false))
                //DrawProperties(prop, false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
        }

        private void CopycatHeader(SerializedProperty prop)
        {
            prop.NextVisible(true); // move to script component
            GUI.enabled = false;
            EditorFunctions.EditorScriptField(this);
            GUI.enabled = true;
        }

    }
}
