using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neat.Music
{
    public class FretClickHandler : MonoBehaviour
    {
        // use EventTrigger
        private FretUI ui;
        public FretObject fret { get; private set; }

        public bool hovering;
        private Color startColor;

        public Color hoverColor = Color.red;

        private Image fill;

        public Color color
        {
            get
            {
                return fill.color;
            }
            set
            {
                fill.color = value;
            }
        }

        private void Awake()
        {
            ui = GetComponent<FretUI>();
            fill = ui.fill;
            startColor = fill.color;
        }

        private void Update()
        {


            var fret = ui.fret;
            if (fret == null)
                return;

            // method?
            var mouse = Input.mousePosition;
            var inverse = fret.rect.InverseTransformPoint(mouse);
            hovering = fret.rect.rect.Contains(inverse);

            Debug.Log("Hovering: " + hovering);

            if (hovering)
            {
                color = hoverColor;
            }
            else
            {
                color = startColor;
            }
        }
    }
}
