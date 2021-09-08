using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neat.Extensions;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PolygonMaker))]
public class PolygonMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var _target = (PolygonMaker)target;
        if (GUI.changed || GUILayout.Button("(Re)Spawn"))
        {
            _target.Init();
        }
    }
}
#endif

public class PolygonMaker : MonoBehaviour
{
    public bool active;
    public GameObject template;
    public Material mat;
    public bool removeTemplate;
    [Range(3, 59)]
    public int numSide = 3;

    public float distance = 5;
    public Vector3 size;
    public bool isBottomAnchor;
    public Color color;
    public float tilingMultiplier;

    // public 

    private GameObject[] spawned;


    private void OnValidate()
    {
        // Init();
    }
    private void Awake()
    {
        Init();
    }

    internal void Init()
    {
        if (active)
        {
            Respawn();
            Fix();
        }
    }

    internal void Respawn()
    {
        foreach (Transform child in transform)
            StartCoroutine(Destroy(child.gameObject));

        spawned = new GameObject[numSide];

        if (template != null)
            spawned[0] = Instantiate(template);
        else
            spawned[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);

        spawned[0].transform.parent = this.transform;


        if (mat != null)
            spawned[0].GetComponent<MeshRenderer>().sharedMaterial = mat;
        spawned[0].GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(size.x / 2 * tilingMultiplier, size.y / 2 * tilingMultiplier);
        spawned[0].SetColor(color);

        for (int i = 1; i < numSide; i++)
        {
            spawned[i] = Instantiate(spawned[0], transform);
            spawned[i].SetColor(color);
        }
    }
    IEnumerator Destroy(GameObject go)
    {
        // https://answers.unity.com/questions/1318576/destroy-child-objects-onvalidate.html
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }
    void Fix()
    {
        var startRot = transform.rotation;

        for (int i = 0; i < numSide; i++)
        {
            transform.Rotate(0, (360 / numSide), 0);
            var go = spawned[i];
            go.name = i.ToString();
            go.SetActive(true);
            go.transform.localPosition = transform.forward * distance;
            go.transform.LookAt(transform.position);
            go.transform.localScale = new Vector3(size.x, size.y, size.z);

            if (isBottomAnchor)
                go.transform.localPosition += Vector3.up * go.transform.localScale.y / 2;
        }
    }
}
