using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IKTargetGroup : MonoBehaviour
{
    public IKTarget[] targets;

    // needed for interaction...
    [ContextMenuItem("Equip", "Equip")]
    public IKController controller;

    public void FindTargets()
    {
        targets = GetComponentsInChildren<IKTarget>();
    }
}

public static partial class Extensions
{
    public static T Find<T>(this Transform transform)
    {
        foreach (var child in transform) { }
        throw new System.NotImplementedException();
    }
}
