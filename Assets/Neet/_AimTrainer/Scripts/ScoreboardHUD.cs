using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Neet.AimTrainer
{
    public class ScoreboardHUD : MonoBehaviour
    {
        public Transform container;

        public TextMeshProUGUI time;
        public TextMeshProUGUI accuracy;
        public TextMeshProUGUI targets;

        bool isTargetsRelevant;
        bool isAccuracyClicks;
        bool isAccuracyTracking;

        public void UpdateText(ScoreProfile s, ChallengeProfile c, bool isChallenge)
        {
            // time remaining vs elapsed
            if (isChallenge && c.isTimeLimit)
                time.text = (c.timeLimit - s.timeElapsed).ToString("f1");
            else
                time.text = s.timeElapsed.ToString("f1");

            // targets remaining vs ratio
            if (targets.enabled)
            {
                if (isChallenge && c.isTargetLimit)
                    targets.text = s.targetsAttempted + " / " + c.targetLimit.ToString();
                else
                    targets.text = s.OverallRatio;
            }

            if (isAccuracyClicks)
                accuracy.text = s.ClickRate.ToString("f0") + "%";
            else if (isAccuracyTracking)
                accuracy.text = s.TrackRate.ToString("f0") + "%";
            else
                accuracy.text = s.OverallRate.ToString("f0") + "%";
        }

        /// <summary>
        /// Updates target visibility and determines what accuracy should refer to
        /// </summary>
        public void Initialize(PresetProfile p)
        {
            isTargetsRelevant = p.timingProfile.canClickTimeout
                || p.aimProfile.failTargetOnMissClick
                || p.trackingProfile.canTrackTimeout
                || p.trackingProfile.canTrackDestroy;
            UIHelpers.GetParentUnderContainer(targets.transform, container)
                .gameObject.SetActive(isTargetsRelevant);


            isAccuracyClicks = !p.trackingProfile.canTrack;
            isAccuracyTracking = !(p.timingProfile.canClickTimeout
                || p.aimProfile.failTargetOnMissClick)
                && p.trackingProfile.canTrack;
        }

        public void ResetText()
        {
            time.text = "--";
            targets.text = "--";
            accuracy.text = "--";
        }
    }
}