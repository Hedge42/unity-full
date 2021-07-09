using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace Neet.AimTrainer
{
    public class ScoreResultsUI : MonoBehaviour
    {
        private const string DEC_F = "f1";
        private const string SEP = " -- ";

        public Transform container;

        // overall stats
        public TextMeshProUGUI title;
        public TextMeshProUGUI datePlayed;
        public TextMeshProUGUI timeElapsed;
        public TextMeshProUGUI overallAccuracy;

        // click stats
        public TextMeshProUGUI clickAccuracy;
        public TextMeshProUGUI clickTimeAverage;
        public TextMeshProUGUI clicksMissed;
        public TextMeshProUGUI clicksTimedOut;

        // tracking stats
        public TextMeshProUGUI trackSuccess;
        public TextMeshProUGUI trackTime;

        // player stats
        public TextMeshProUGUI distancePerSuccessfulTarget;

        // only relevant in Training Room scene -- maybe could be moved
        public Toggle saveScoreToggle;
        public UnityAction saveScoreAction;
        public UnityAction clearScoreAction;

        /// <summary>
        /// Sets results text values
        /// </summary>
        public void UpdateGUI(PresetProfile p, ScoreProfile s)
        {
            // overall stats
            title.text = p.name + " results";
            timeElapsed.text = s.timeElapsed.ToString(DEC_F) + " seconds";
            datePlayed.text = s.datePlayed;

            overallAccuracy.text = s.OverallRatio + SEP
                + s.OverallRate.ToString(DEC_F) + "%";

            // click stats
            clickAccuracy.text = s.ClickRatio + SEP
                + s.ClickRate.ToString(DEC_F) + "%";

            clicksMissed.text = s.ClicksMissedRatio + SEP
                + s.ClicksMissedRate.ToString(DEC_F) + "%";

            clicksTimedOut.text = s.ClickTimeoutRatio + SEP
                + s.ClickTimeoutRate.ToString(DEC_F) + "%";

            clickTimeAverage.text = s.ClickTimeAverage.ToString("f2") + "s";

            // tracking stats
            trackSuccess.text = s.TrackSuccessRatio + SEP
                + s.TrackSuccessRate.ToString(DEC_F) + "%";

            trackTime.text = s.TrackRatio + SEP + s.TrackRate.ToString(DEC_F) + "%";


            // player movement stats
            distancePerSuccessfulTarget.text =
                s.DistancePerSuccessfulTarget.ToString(DEC_F) + "m";

            UpdateVisibility(p);
        }

        /// <summary>
        /// Disables irrelevant stat items
        /// </summary>
        private void UpdateVisibility(PresetProfile p)
        {
            bool isOverallRelevant = p.timingProfile.canClickTimeout
                && p.trackingProfile.canTrackTimeout;
            bool isClickTimeoutRelevant = p.timingProfile.canClickTimeout;
            bool isClickRelevant = p.timingProfile.canClickTimeout
                || p.aimProfile.failTargetOnMissClick;
            bool isTrackTimeRelevant = p.trackingProfile.canTrack;
            bool isTrackSuccessRelevant = p.trackingProfile.canTrackTimeout;
            bool isDistanceRelevant = p.movementProfile.canMove;

            GetParentObject(overallAccuracy).SetActive(isOverallRelevant);

            // click stats
            GetParentObject(clickAccuracy).SetActive(isClickRelevant);
            GetParentObject(clicksMissed).SetActive(isClickRelevant);
            GetParentObject(clickTimeAverage).SetActive(isClickRelevant);
            GetParentObject(clicksTimedOut).SetActive(isClickTimeoutRelevant);

            // track stats
            GetParentObject(trackTime).SetActive(isTrackTimeRelevant);
            GetParentObject(trackSuccess).SetActive(isTrackSuccessRelevant);

            // player stats
            GetParentObject(distancePerSuccessfulTarget).SetActive(isDistanceRelevant);
        }
        private GameObject GetParentObject(Component c)
        {
            var t = c.transform;
            while (t.parent != container)
                t = t.parent;
            return t.gameObject;
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
    }
}