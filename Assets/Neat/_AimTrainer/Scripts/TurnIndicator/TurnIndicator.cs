using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Neat.Demos.AimTrainer
{
    [ExecuteAlways]
    [RequireComponent(typeof(Canvas))]
    public class TurnIndicator : MonoBehaviour
    {
        public RectTransform rotator;

        private Camera cam;
        private Camera Cam
        {
            get
            {
                if (cam == null)
                    cam = Camera.main;
                return cam;
            }
        }

        // [SerializeField] // for debugging
        private Transform target;
        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        private void OnDisable()
        {
            rotator.gameObject.SetActive(false);
        }


        private void Update()
        {
            if (Target != null)
            {
                // find angle in degrees camera forward and target position
                float angle = Vector3.SignedAngle(Cam.transform.forward, Target.transform.position, Vector3.up);

                // transform angle rotates in opposite direction, and arrow points up
                rotator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -(angle - 90)));

                // only show if not visible
                rotator.gameObject.SetActive(!InFOV(angle));
            }
            else
            {
                rotator.gameObject.SetActive(false);
            }
           
        }

        private bool InFOV(float angle)
        {
            // angle from camera forward
            angle = Mathf.Abs(angle);

            // fieldOfView is vertical by default
            return angle < Camera.VerticalToHorizontalFieldOfView(Cam.fieldOfView, 16f / 9f) / 2f;
        }
    }
}
