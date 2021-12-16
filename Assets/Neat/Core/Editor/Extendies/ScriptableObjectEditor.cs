using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Neat.Tools
{
    // [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEditor : Editor
    {
        protected object[] instances;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("hello"))
            {
                DisplayAllInstances();
            }
        }

        // display all instances?
        public void DisplayAllInstances()
        {
            var p = PerformanceElement.Start("Finding ScriptableObject instances...");
            instances = EditorFunctions.GetAllInstances(target.GetType());
            p.Stop();

            Debug.Log($"Instances found: {instances.Length}");
        }
    }
}
