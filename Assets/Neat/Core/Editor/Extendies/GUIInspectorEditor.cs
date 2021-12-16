using UnityEditor;
using UnityEngine;

namespace Neat.Tools
{
    public class GUIInspectorEditor : Editor
    {
        // just has to call the GUIInspector's OnGUI
        public GUIInspector inspector;

        public static GUIInspectorEditor CreateInstance(Object target)
        {
            var editor = Editor.CreateEditor(target, typeof(GUIInspectorEditor)) as GUIInspectorEditor;
            editor.inspector = GUIInspector.from(target);
            return editor;
        }

        public override void OnInspectorGUI()
        {
            EditorFunctions.GUIScriptField(inspector);
            if (GUILayout.Button("Open in GUI Window"))
                GUIWindow.Open(target, (target as MonoBehaviour).gameObject);

            inspector.OnGUI();
        }
    }
}
