using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject player;

    private void OnValidate()
    {
        // Init();
    }
    private void Init()
    {
        if (player == null)
        {
            if (Player.main != null)
                player = Player.main.gameObject;
        }

        FacePlayer();
    }
    private void Awake()
    {
        Init();
    }
    private void OnDrawGizmos()
    {
        if (player != null)
            Gizmos.DrawLine(transform.position, player.transform.position);
    }
    private void Update()
    {
        FacePlayer();
    }
    private void FacePlayer()
    {
        transform.LookAt(player.transform);
        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rot.y, 0);
    }
}
