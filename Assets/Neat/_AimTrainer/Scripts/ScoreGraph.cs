using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Tools.UI;
using Neat.Demos.AimTrainer;

namespace Neat.Demos.AimTrainer
{
    public class ScoreGraph : MonoBehaviour
    {
        public GameObject tooltipPrefab;
        public ScoreResultsUI results;
        public UISwitcher switcher;
        public LineGraph graph;

        public enum ScoreType
        {
            TimeElapsed,
            TargetsDestroyed,
            OverallAccuracy,
            ClickAccuracy,
            TrackTime,
            TrackAccuracy,
        }

        private void Start()
        {
            SetupTooltip(results.overallAccuracy.transform, ScoreType.OverallAccuracy);
            SetupTooltip(results.clickAccuracy.transform, ScoreType.ClickAccuracy);
            SetupTooltip(results.trackSuccess.transform, ScoreType.TrackAccuracy);
            SetupTooltip(results.trackTime.transform, ScoreType.TrackTime);
            SetupTooltip(results.timeElapsed.transform, ScoreType.TimeElapsed);
        }

        private NeatEventHandler SetupTooltip(Transform t, ScoreType type)
        {
            GameObject tooltipGO = Instantiate(tooltipPrefab, t);
            tooltipGO.SetActive(true);
            tooltipGO.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            NeatEventHandler e = t.GetComponent<NeatEventHandler>();

            e.onPointerClick += delegate
            {
                switcher.SwitchTo(2);
                Generate(PresetProfile.current, type);
            };
            return e;
        }

        public void Generate(PresetProfile profile, ScoreType type)
        {
            List<float> values = new List<float>();

            foreach (ScoreProfile score in profile.scores)
            {
                if (type == ScoreType.ClickAccuracy)
                    values.Add(score.ClickRate);
                else if (type == ScoreType.OverallAccuracy)
                    values.Add(score.OverallRate);
                else if (type == ScoreType.TargetsDestroyed)
                    values.Add(score.targetsSuccessful);
                else if (type == ScoreType.TimeElapsed)
                    values.Add(score.timeElapsed);
                else if (type == ScoreType.TrackAccuracy)
                    values.Add(score.TrackSuccessRate);
                else if (type == ScoreType.TrackTime)
                    values.Add(score.TrackRate);
            }

            float max = 0f;

            if (type == ScoreType.ClickAccuracy || type == ScoreType.OverallAccuracy
                || type == ScoreType.TrackAccuracy)
            {
                max = 100f;
            }
            else
            {
                max = Max(values);
            }

            graph.SetTitle(type.ToString());
            graph.SetYSuffix("");
            graph.Generate(values, 0, max);
        }
        private float Max(List<float> values)
        {
            float max = 0f;
            foreach (float f in values)
                if (f > max)
                    max = f;

            return max;
        }
    }
}