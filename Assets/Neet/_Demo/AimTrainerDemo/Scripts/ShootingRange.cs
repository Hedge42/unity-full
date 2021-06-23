using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRange : MonoBehaviour
{
    public Collider left;
    public Collider right;
    public Collider floor;
    public Collider front;
    public Collider back;

    public Transform spawn;

    public float walkWayDepth;

    public Vector3 size;

    public bool validate;

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        ScaleTerrain();
        ScaleSpawnVolume();
    }

    private void Awake()
    {
        Init();
    }

    void ScaleTerrain()
    {
        // set positions
        left.transform.localPosition = new Vector3(-size.x / 2, 0, 0);
        right.transform.localPosition = new Vector3(size.x / 2, 0, 0);
        back.transform.localPosition = new Vector3(0, 0, size.z / 2);
        front.transform.localPosition = new Vector3(0, 0, -size.z / 2);

        // set scales
        left.transform.localScale = new Vector3(1, size.y, size.z + walkWayDepth);
        right.transform.localScale = new Vector3(1, size.y, size.z + walkWayDepth);
        back.transform.localScale = new Vector3(size.x, size.y, 1);
        front.transform.localScale = new Vector3(size.x, size.y, 1);
    }

    void ScaleSpawnVolume()
    {
        // get the distance between
        // the left face of the center wall
        // and the right face of the left wall

        // position + half the width
        var leftFacePos = left.transform.position + left.transform.rotation * Vector3.right * left.bounds.extents.x;
        var rightFacePos = right.transform.position + right.transform.rotation * Vector3.left * right.bounds.extents.x;

        // stretch the leftVolume to fit between these cubes
        var spawnCenter = (left.transform.position + right.transform.position) / 2;
        var spawnWidth = Vector3.Distance(leftFacePos, rightFacePos);

        // shorthand
        var lp = spawn.transform.position;
        var ls = spawn.transform.localScale;

        // TODO stretch Y and Z
        spawn.transform.position = new Vector3(spawnCenter.x, lp.y, lp.z);
        spawn.transform.localScale = new Vector3(spawnWidth, ls.y, ls.z);
    }
}
