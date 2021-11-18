using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neat.Music
{
    public class OptionBar
    {
        private OptionBarUI ui;

        // components
        private RectTransform rect;
        private Transform container;
        private VerticalLayoutGroup layout;
        private float startWidth;

        private KeyCode toggleKey = KeyCode.O;

        public OptionBar(OptionBarUI ui)
        {
            this.ui = ui;
            rect = ui.GetComponent<RectTransform>();
            container = ui.container;
            startWidth = rect.sizeDelta.x;
        }

        public void Show()
        {
            rect.sizeDelta = new Vector2(startWidth, rect.sizeDelta.y);

            // left-aligned y-stretch
            // left-top alignment
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
        }
        public void Hide()
        {
            // set width to 0 ?

            rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
            
        }
    }

    public class OptionBarUI : MonoBehaviour
    {
        public Transform container;

        public Option prefab;

        private OptionBar _optionBar;
        public OptionBar optionBar
        {
            get
            {
                if (_optionBar == null)
                    _optionBar = new OptionBar(this);
                return _optionBar;
            }
        }
    }
}
