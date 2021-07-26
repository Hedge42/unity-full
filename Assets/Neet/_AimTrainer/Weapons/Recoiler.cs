using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoiler : MonoBehaviour
{
    public bool previewGunRecoil;
    public Vector3 gizmoLength;
    [Range(.01f, 1f)]
    public float awayTime;
    [Range(.1f, 1f)]
    public float returnTime;
    public Vector3 gunPosition;
    public Vector3 gunRotation;
    public Vector3 cameraRotation;
    public float gunPosNoise;
    public float gunRotNoise;
    public float camRotNoise;

    public Curve awayCurve;
    public Curve returnCurve;

    private Random r;
    private Coroutine currentRecoil;
    private int numRecoils;

    private void OnDrawGizmos()
    {
        // preview recoil transform
        if (previewGunRecoil)
        {
            Vector3 endPos = transform.parent.position + gunPosition;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.parent.position, endPos);

            Quaternion rot = Quaternion.Euler(
                new Vector3(gunRotation.x, gunRotation.y, gunRotation.z));

            // z
            Vector3 zBack = endPos + rot * Vector3.back * gizmoLength.z;
            Vector3 zFront = endPos + rot * Vector3.forward * gizmoLength.z;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(zBack, zFront);

            // x
            Vector3 xLeft = endPos + rot * Vector3.left * gizmoLength.x;
            Vector3 xRight = endPos + rot * Vector3.right * gizmoLength.x;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(xLeft, xRight);

            // y
            Gizmos.color = Color.yellow;
            Vector3 yDown = endPos + rot * Vector3.down * gizmoLength.y;
            Vector3 yUp = endPos + rot * Vector3.up * gizmoLength.y;
            Gizmos.DrawLine(yDown, yUp);

        }
    }

    private void Awake()
    {
    }


    public void StartRecoil()
    {
        if (currentRecoil != null)
        {
            StopCoroutine(currentRecoil);
            currentRecoil = null;
        }

        currentRecoil = StartCoroutine(Recoil());
    }
    private IEnumerator Recoil()
    {
        // with noise
        Vector3 _gunPos = AddNoise(gunPosition, gunPosNoise);
        Vector3 _gunRot = AddNoise(gunRotation, gunRotNoise);
        Vector3 _camRot = AddNoise(cameraRotation, camRotNoise);

        Quaternion gunTargetRotation = Quaternion.Euler(
                new Vector3(_gunRot.x, _gunRot.y, _gunRot.z));
        Quaternion cameraTargetRotation = Quaternion.Euler(
                new Vector3(_camRot.x, _camRot.y, _camRot.z));
        Vector3 gunTargetPosition = _gunPos;


        float startAway = Time.time;
        float endAway = startAway + awayTime;
        while (Time.time < endAway)
        {
            float ratio = (Time.time - startAway) / awayTime;
            if (awayCurve != null)
                ratio = awayCurve.Ferp(ratio);

            Quaternion gunRot = Quaternion.LerpUnclamped(Quaternion.identity, gunTargetRotation, ratio);
            Quaternion camRot = Quaternion.LerpUnclamped(Quaternion.identity, cameraTargetRotation, ratio);
            Vector3 gunPos = Vector3.LerpUnclamped(Vector3.zero, gunTargetPosition, ratio);

            transform.localRotation = gunRot;
            transform.localPosition = gunPos;

            CameraController.active.transform.localRotation = camRot;

            yield return null;
        }

        float startReturn = Time.time;
        float endReturn = startReturn + returnTime;
        while (Time.time < endReturn)
        {
            float ratio = (Time.time - startReturn) / returnTime;
            if (returnCurve != null)
                ratio = returnCurve.Ferp(ratio);

            Quaternion gunRot = Quaternion.LerpUnclamped(gunTargetRotation, Quaternion.identity, ratio);
            Quaternion cameraRot = Quaternion.LerpUnclamped(cameraTargetRotation, Quaternion.identity, ratio);
            Vector3 gunPos = Vector3.LerpUnclamped(gunTargetPosition, Vector3.zero, ratio);

            transform.localRotation = gunRot;
            transform.localPosition = gunPos;

            CameraController.active.transform.localRotation = cameraRot;

            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        CameraController.active.transform.localRotation = Quaternion.identity;
    }

    public Vector3 AddNoise(Vector3 v, float noiseRange)
    {
        noiseRange = Mathf.Abs(noiseRange);
        Vector3 noiseVector = new Vector3()
        {
            x = Random.Range(-noiseRange, noiseRange),
            y = Random.Range(-noiseRange, noiseRange),
            z = Random.Range(-noiseRange, noiseRange),
        };
        return v + noiseVector;
    }

    private IEnumerator _Recoil()
    {
        numRecoils += 1;

        transform.Rotate(gunRotation);
        transform.Translate(gunPosition);
        Camera.main.transform.Rotate(cameraRotation);

        Vector3 fixedGunRot = Vector3.zero;
        Vector3 fixedGunPos = Vector3.zero;
        Vector3 fixedCamRot = Vector3.zero;

        float startTime = Time.fixedTime;
        while (Time.fixedTime < startTime + awayTime)
        {
            Vector3 gunRot = -gunRotation * Time.fixedDeltaTime / awayTime;
            Vector3 gunPos = -gunPosition * Time.fixedDeltaTime / awayTime;
            Vector3 camRot = -cameraRotation * Time.fixedDeltaTime / awayTime;

            transform.Translate(gunPos, Space.World);
            transform.Rotate(gunRot, Space.World);
            Camera.main.transform.Rotate(camRot);

            fixedGunRot += gunRot;
            fixedGunPos += gunPos;
            fixedCamRot += camRot;

            yield return new WaitForFixedUpdate();
        }

        Vector3 gunRotError = gunRotation + fixedGunRot;
        Vector3 gunPosError = gunPosition + fixedGunPos;
        Vector3 camRotError = cameraRotation + fixedCamRot;

        print("GunRotError: " + gunRotError);
        print("GunPosError: " + gunPosError);
        print("CamRotError: " + camRotError);

        transform.Rotate(gunRotError, Space.World);
        transform.Translate(gunPosError, Space.World);
        Camera.main.transform.Rotate(camRotError, Space.World);

        numRecoils -= 1;

        // reset to defaults
        if (numRecoils == 0)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            Camera.main.transform.localRotation = Quaternion.identity;
        }
    }
}
