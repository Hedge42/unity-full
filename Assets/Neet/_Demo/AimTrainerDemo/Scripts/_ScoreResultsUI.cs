using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class _ScoreResultsUI : MonoBehaviour
{
    private const string DEC_F = "f1";

    public Transform container;

    // results screen references
    public TextMeshProUGUI title;
    public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI datePlayed;

    public TextMeshProUGUI overallAccuracy;
    public TextMeshProUGUI clickAccuracy;
    public TextMeshProUGUI trackSuccess;
    public TextMeshProUGUI trackTime;
    public TextMeshProUGUI distancePerSuccessfulTarget;

    public Toggle saveScoreToggle;
    public UnityAction saveScoreAction;
    public UnityAction clearScoreAction;

    public void UpdateGUI(PresetProfile p, ScoreProfile s)
    {
        title.text = p.name + " results";
        timeElapsed.text = s.timeElapsed.ToString(DEC_F) + " seconds";
        datePlayed.text = s.datePlayed;

        // accuracy stats
        overallAccuracy.text = Ratio(s.targetsSuccessful, s.targetsAttempted);
        clickAccuracy.text = Ratio(s.clicksSuccessful, s.clicksAttempted);
        trackSuccess.text = Ratio(s.tracksSuccessful, s.tracksAttempted);
        trackTime.text = s.trackTimeSuccessful.ToString(DEC_F) + "s/" +
            s.trackTimeAttempted.ToString(DEC_F) + "s -- "
            + GetPercentage(s.tracksSuccessful, s.trackTimeAttempted);

        // player movement stats
        distancePerSuccessfulTarget.text =
            CullNaN(s.totalDistance / s.targetsSuccessful) + "m";

        UpdateVisibility(p);
    }

    private string GetPercentage(float a, float b)
    {
        var value = 100 * a / b;
        return CullNaN(value);
    }
    private string CullNaN(float value)
    {
        if (float.IsNaN(value))
            return "0";
        else
            return value.ToString(DEC_F);
    }
    private string Ratio(int successful, int attempted)
    {
        return successful + "/" + attempted + " -- "
            + GetPercentage((float)successful, (float)attempted);
    }

    public void SetToggleState(bool value)
    {
        if (saveScoreToggle != null)
            saveScoreToggle.isOn = value;
    }
    public void OnOK()
    {
        if (saveScoreToggle.isOn)
            saveScoreAction?.Invoke();

        // clear the score
        clearScoreAction?.Invoke();
    }

    

    private void UpdateVisibility(PresetProfile p)
    {
        GetParentObject(clickAccuracy).SetActive(
            p.timingProfile.canClickTimeout || p.aimProfile.failTargetOnMissClick);

        GetParentObject(trackTime).SetActive(p.trackingProfile.canTrack);
        GetParentObject(trackSuccess).SetActive(p.trackingProfile.canTrackTimeout);

        GetParentObject(distancePerSuccessfulTarget)
            .SetActive(p.movementProfile.canMove);
    }

    private GameObject GetParentObject(Component c)
    {
        var t = c.transform;
        while (t.parent != container)
            t = t.parent;
        return t.gameObject;
    }
}
