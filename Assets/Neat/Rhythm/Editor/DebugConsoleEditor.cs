using UnityEditor;
using UnityEngine;
using Neat.Extensions;
using Neat.Tools;

namespace Neat.Console
{
    public class DebugConsoleEditorWindow : ExtendedEditorWindow
    {
        //[MenuItem("Cheat Console Editor")]
        //public static void Open(DebugConsole console)
        //{
        //    var window = GetWindow<DebugConsoleEditorWindow>("Cheat Console Editor");
        //    window.serializedObject = new SerializedObject(console);
        //}
        //private void OnGUI()
        //{
        //    // serializedObject.get
        //    // currentProperty = serializedObject.FindProperty("_commands");
        //    // DrawProperties(currentProperty, true);

        //    //var x = serializedObject.GetIterator();
        //    //x.Next(true);
        //    //while (x.NextVisible(false))
        //    //    DrawProperties(x, true);
        //    // DrawField("commands", true);
        //    //DrawField("show", true);
        //    //DrawField("submit", true);
        //    //DrawField("cancel", true);

        //    // DrawSidebar(serializedObject.FindProperty("tests"));

        //    // DrawField("commands", true);

        //    //var it = serializedObject.GetIterator();
        //    //it.Next(true);

        //    //while (it.NextVisible(false))
        //    //{
        //    //DrawProperties(it, false);
        //    //}


        //    // currentProperty = serializedObject.FindProperty("_commands");
        //    // DrawProperties(currentProperty, true);

        //    //var x = serializedObject.GetIterator();

        //    //while (x.NextVisible(false))
        //    //{

        //    //}




        //    //EditorGUILayout.BeginHorizontal();
        //    //EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        //    //DrawSidebar(currentProperty);
        //    //EditorGUILayout.EndVertical();

        //    //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        //    //if (selectedProperty != null)
        //    //{
        //    //    DrawSelectedPropertiesPanel();
        //    //}
        //    //else
        //    //{
        //    //    EditorGUILayout.LabelField("Select something...");
        //    //}
        //    //EditorGUILayout.EndVertical();
        //    //EditorGUILayout.EndHorizontal();

        //    // Apply();
        //}
    }


    [CustomEditor(typeof(DebugConsole))]
    public class DebugConsoleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DebugConsole _target = target as DebugConsole;

            if (GUILayout.Button("Open Window"))
            {
                Debug.Log("lol");
            }

            //foreach (var cmd in _target.commands)
            //{
                //GUILayout.Label(cmd.ToString());
            //}


            // https://youtu.be/mTjYA3gC1hA?t=255
            //var serializedProperty = serializedObject.GetIterator();
            //while (serializedProperty.NextVisible(false))
            //{
            //    PropertyField prop = new PropertyField(serializedProperty);
            //    // prop.SetEnabled(prop.name != "help");
            //}
        }
    }
}