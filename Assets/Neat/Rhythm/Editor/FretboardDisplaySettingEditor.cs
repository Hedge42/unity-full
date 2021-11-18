using UnityEditor;
using UnityEngine;

namespace Neat.Music
{
    [CustomEditor(typeof(FretboardDisplaySetting))]
    public class FretboardDisplaySettingEditor : Editor
    {
        FretboardDisplaySetting _target;

        private void OnEnable()
        {
            _target = (FretboardDisplaySetting)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private FretboardDisplaySetting DrawDisplayPreferences()
        {
            // display preferences
            var setting = _target;
            setting.borderMode = (Fret.BorderMode)EditorGUILayout.EnumPopup("Border display", setting.borderMode);
            setting.fretMode = (Fret.PlayableMode)EditorGUILayout.EnumPopup("Playable display", setting.fretMode);
            return setting;
        }

        private static void DrawFretRange(FretboardDisplaySetting setting)
        {
            // fret range
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Fret range");
            float fMin = (float)setting.minFret;
            float fMax = (float)setting.maxFret;
            fMin = (float)EditorGUILayout.IntField((int)fMin, GUILayout.Width(40));
            EditorGUILayout.MinMaxSlider(ref fMin, ref fMax, 0, 24);
            fMax = (float)EditorGUILayout.IntField((int)fMax, GUILayout.Width(40));
            setting.minFret = (int)fMin;
            setting.maxFret = (int)fMax;
            GUILayout.EndHorizontal();
        }
    }
}
