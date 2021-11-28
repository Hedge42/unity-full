using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Neat.Demos.Fighter
{

    [CustomEditor(typeof(GizmoDrawer))]
    public class GizmoColorPaletteEditor : Editor
    {
        private bool colorsDropdown;
        private bool hitboxDropdown;
        private bool stateDropown;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GizmoDrawer _target = (GizmoDrawer)target;

            var colors = _target.colors;


            // conditionally show transform origin
            _target.useTransformOrigin = EditorGUILayout.
                Toggle("Use transform origin", _target.useTransformOrigin);

            if (_target.useTransformOrigin)
                _target.localPreviewOrigin = _target.transform.position;

            EditorGUI.BeginDisabledGroup(_target.useTransformOrigin);
            _target.localPreviewOrigin = EditorGUILayout.
                Vector3Field("Origin", _target.localPreviewOrigin);
            EditorGUI.EndDisabledGroup();



            if (_target.colors != null)
            {
                colorsDropdown = EditorGUILayout.Foldout(colorsDropdown, "Colors");

                if (colorsDropdown)
                {
                    colors.clrDefault = EditorGUILayout.ColorField("Default", colors.clrDefault);

                    hitboxDropdown = EditorGUILayout.Foldout(hitboxDropdown, "Hitbox colors");
                    if (hitboxDropdown)
                    {
                        colors.clrHitboxLow = EditorGUILayout.ColorField("Low", colors.clrHitboxLow);
                        colors.clrHitboxMid = EditorGUILayout.ColorField("Mid", colors.clrHitboxMid);
                        colors.clrHitboxHigh = EditorGUILayout.ColorField("High", colors.clrHitboxHigh);
                        colors.clrHitboxOverhead = EditorGUILayout.ColorField("Overhead", colors.clrHitboxOverhead);
                    }

                    stateDropown = EditorGUILayout.Foldout(stateDropown, "State colors");
                    if (stateDropown)
                    {
                        colors.clrStateAir = EditorGUILayout.ColorField("Air", colors.clrStateAir);
                        colors.clrStateStand = EditorGUILayout.ColorField("Stand", colors.clrStateStand);
                        colors.clrStateCrouch = EditorGUILayout.ColorField("Crouch", colors.clrStateCrouch);
                        colors.clrStateDowned = EditorGUILayout.ColorField("Downed", colors.clrStateDowned);
                        colors.clrStateAttack = EditorGUILayout.ColorField("Attack", colors.clrStateAttack);
                        colors.clrStateGuard = EditorGUILayout.ColorField("Guard", colors.clrStateGuard);
                    }
                }
            }
        }
    }
}
