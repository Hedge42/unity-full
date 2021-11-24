using UnityEditor;
using UnityEngine;
using Neat.Extensions;
using Neat.Tools;

namespace Neat.Attributes
{
    [CustomEditor(typeof(AttributesDemo))]
    public class AttributesDemoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // open editor window
            // open gui window
            MakeWindowButtons();

            base.OnInspectorGUI();
        }

        void MakeWindowButtons()
        {
            if (GUILayout.Button("Open in Editor Window"))
            {
                ExtendedEditorWindow.Open(target as AttributesDemo);
            }
            else if (GUILayout.Button("Open in GUI Window"))
            {
                GUIWindowDrawer.instance.Open(target as AttributesDemo);
            }
        }
    }
}
