using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            + (100 * s.trackTimeSuccessful / s.trackTimeAttempted).ToString(DEC_F) + "%";

        // player movement stats
        distancePerSuccessfulTarget.text = (s.totalDistance / s.targetsSuccessful)
            .ToString(DEC_F) + "m";

        UpdateVisibility(p);
    }

    private string Ratio(int successful, int attempted)
    {
        return successful + "/" + attempted + " -- "
            + (100 * (float)successful / attempted).ToString(DEC_F) + "%";
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
