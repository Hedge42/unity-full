using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neat.Tools.UI
{
    [ExecuteInEditMode()]
    public class ProgressBar : MonoBehaviour
    {

        public int min;
        public int max;
        public int current;
        public Image mask;
        public Image fill;
        public Color color;

        private void Update()
        {
            GetCurrentFill();
        }
        void GetCurrentFill()
        {
            float currentOffset = current - min;
            float maxOffset = max - min;
            float fillAmount = currentOffset / maxOffset;
            mask.fillAmount = fillAmount;

            fill.color = color;
        }
    }
}