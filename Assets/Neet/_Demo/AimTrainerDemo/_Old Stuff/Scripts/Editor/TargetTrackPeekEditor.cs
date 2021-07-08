using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Neet.Functions.EditorFunctions;

[CustomEditor(typeof(TargetTrackPeek))]
public class TargetTrackPeekEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var _target = (TargetTrackPeek)target;

        // GUIMinMaxSlider("Speed range", ref TargetSetting.current.speedMin, ref TargetSetting.current.speedMax, .1f, 5);
        // GUIMinMaxSlider("Delay range", ref TargetSetting.current.delayMin, ref TargetSetting.current.delayMax, 0, 10);
        GUIMinMaxSlider("Walk/Run range", ref _target.playerMotor.walkSpeed, ref _target.playerMotor.runSpeed, 1, 20);
    }
}
