using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.DDD
{
    public class CameraArm : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;

        Camera camera;

        private void Start()
        {
            camera = Camera.main;
        }

        private void OnValidate()
        {
            UpdateCameraTransform(Camera.main);
        }



        private void Update()
        {
            UpdateCameraTransform(camera);
        }

        void UpdateCameraTransform(Camera camera)
        {
            camera.transform.position = target.TransformPoint(offset);
            camera.transform.rotation = target.rotation;
        }
    }
}