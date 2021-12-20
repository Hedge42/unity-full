using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static Camera active;

    public Camera current;

    public UnityAction<Camera> onEnabled;

    private Vector3 localStartPos;
    private Vector3 camInversePoint;

    private void OnValidate()
    {
        EnableCamera(current);
    }

    private void Awake()
    {
        EnableCamera(current);

        localStartPos = transform.localPosition;

        camInversePoint = transform
            .InverseTransformPoint(active.transform.position);

        UpdateTransform();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(switchCamKey))
        //{
        //    useFirstPerson = !useFirstPerson;
        //    UpdateCamera();
        //}

        // active.transform.position = transform.TransformPoint(localStartPos);
        // active.transform.position = GetComponent<Motor>().rb.position + transform.rotation * transform.TransformPoint(camInversePoint);
    }

    private void UpdateTransform()
    {
        try
        {
            GetComponent<Motor>().onTransformUpdate += delegate (Transform t)
            {
                active.transform.localPosition = localStartPos;
            };
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }


    private void EnableCamera(Camera cam)
    {
        onEnabled?.Invoke(cam);
        current = cam;
        active = current;
    }
}
