using UnityEditor;
using UnityEngine;

namespace Neat.Music
{
    [CustomEditor(typeof(IKTargetGroup), true)]
    public class IKTargetControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Find targets in children"))
            {
                (target as IKTargetGroup).FindTargets();
            }
        }
    }
}
