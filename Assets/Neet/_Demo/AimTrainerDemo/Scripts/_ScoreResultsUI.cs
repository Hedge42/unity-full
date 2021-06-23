using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _ScoreResultsUI : MonoBehaviour
{
    public Transform container;

    // results screen references
    public TextMeshProUGUI title;
    public TextMeshProUGUI timeElapsed;
    public TextMeshProUGUI successRate;
    public TextMeshProUGUI successRatio;
    public TextMeshProUGUI flickRatio;
    public TextMeshProUGUI flickSuccessRate;
    public TextMeshProUGUI trackRatio;
    public TextMeshProUGUI trackSuccessRate;
    public TextMeshProUGUI trackRate;
    public TextMeshProUGUI dateSaved;

    public void UpdateGUI(PresetProfile p, ScoreProfile s)
    {
        title.text = p.name + " results";

        timeElapsed.text = s.timeElapsed.ToString("f1") + " seconds";
        successRate.text = s.SuccessRate;
        successRatio.text = s.SuccessRatio;

        flickRatio.text = s.FlickSuccessRatio;
        flickSuccessRate.text = s.FlickSuccessRate;

        trackRatio.text = s.TrackSuccessRatio;
        trackSuccessRate.text = s.TrackSuccessRate;
        trackRate.text = s.TrackRate;

        dateSaved.text = s.dateSaved;

        UpdateVisibility(p);
    }

    private void UpdateVisibility(PresetProfile p)
    {
        GetParentObject(flickRatio).SetActive(p.timingProfile.canClickTimeout);
        GetParentObject(flickSuccessRate).SetActive(p.timingProfile.canClickTimeout);

        GetParentObject(trackRate).SetActive(p.trackingProfile.canTrack);

        // don't show these if there is a click timeout, otherwise it's redundant
        GetParentObject(trackRatio).SetActive(p.trackingProfile.canTrack
            && p.timingProfile.canClickTimeout);
        GetParentObject(trackSuccessRate).SetActive(p.trackingProfile.canTrackTimeout
            && p.timingProfile.canClickTimeout);
    }

    private GameObject GetParentObject(Component c)
    {
        var t = c.transform;
        while (t.parent != container)
            t = t.parent;
        return t.gameObject;
    }
}
