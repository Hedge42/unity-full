using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // this class is used as a better alternative to FindObjectsWithTag("Player")


    private static List<Player> _instances;
    public static List<Player> instances
    {
        get
        {
            if (_instances == null)
                _instances = new List<Player>(); ;

            return _instances;
        }
    }

    public static Player main
    {
        get
        {
            if (instances.Count > 0)
                return instances[0];
            else
                return null;
        }
    }
    private int _id;
    //[ShowInInspector] 
    public int id { get { return _id; } }
    //[ShowInInspector] 
    public static int numInScene { get { return instances.Count; } }


    private Vector3 startPosition;
    private Quaternion startRotation;

    private void OnValidate()
    {
        FindPlayers();
    }
    private void Awake()
    {
        FindPlayers();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Init()
    {
        if (!instances.Contains(this))
        {

            instances.Add(this);
            this._id = instances.Count;
        }
    }

    void FindPlayers()
    {
        var players = FindObjectsOfType<Player>();
        _instances = new List<Player>(players);
        int id = 0;
        foreach (var p in _instances)
            p._id = id++;
    }

    public void ResetTransform()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
