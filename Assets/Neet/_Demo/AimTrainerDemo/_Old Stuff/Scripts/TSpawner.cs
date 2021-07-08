using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TSpawner : MonoBehaviour
{
    public abstract void Init();
    public abstract TargetSetting GetSetting();
    public abstract void ApplySetting(TargetSetting s);
}
