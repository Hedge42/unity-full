using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static Camera active;

    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;

    public bool useFirstPerson;
    public KeyCode switchCamKey;

    public UnityAction<Camera> onFirstPersonEnabled;
    public UnityAction<Camera> onThirdPersonEnabled;

    private Vector3 localStartPos;

    private Vector3 camInversePoint;

    private void OnValidate()
    {
        UpdateCamera();
    }

    private void Awake()
    {
        UpdateCamera();

        localStartPos = transform.localPosition;

        camInversePoint = transform
            .InverseTransformPoint(active.transform.position);

        SubscribeToMotor();
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

    private void SubscribeToMotor()
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

    private void UpdateCamera()
    {
        if (useFirstPerson)
        {
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
            active = firstPersonCamera;

            onFirstPersonEnabled?.Invoke(firstPersonCamera);
        }
        else
        {
            thirdPersonCamera.enabled = true;
            firstPersonCamera.enabled = false;
            active = thirdPersonCamera;

            onThirdPersonEnabled?.Invoke(thirdPersonCamera);
        }
    }
}
