using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// Inherit from this class with T as the Trainer Profile class
/// </summary>
/// <typeparam name="Trainer Profile"></typeparam>
public interface ISettingUI<T>
{
    /// <summary>
    /// Load profile to fields
    /// </summary>
    void LoadFields(T profile);

    /// <summary>
    /// Creates warning object for each relevant settings object
    /// </summary>
    void CreateWarnings(GameObject warningPrefab, Transform container);

    /// <summary>
    /// Set validation events for each relevant input
    /// </summary>
    void SetUIValidation(UnityAction endAction = null);

    void Apply(ref T profile);
}