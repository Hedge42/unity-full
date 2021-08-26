using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Audio.Vizualization
{
    public class Spawn3DVisualizers : MonoBehaviour
    {
        #region Classes
        // Classes
        [System.Serializable]
        public class BandScalerValues
        {
            // already has band

            // copy + paste from BandScaler.cs
            [Range(-5, 5)]
            public float range;

            public bool useBuffer;
            public bool use64;

            public bool shouldScaleX;
            public bool shouldScaleY;
            public bool shouldScaleZ;

            [HideInInspector]
            public Vector3 baseScale;
        }
        [System.Serializable]
        public class BandColorizerValues
        {
            public bool useBuffer;
            public Color lerpTo;
            [HideInInspector]
            public Color baseColor;
        }
        public enum SpawnType
        {
            Line,
            Arc,
        }
        #endregion

        #region Variables
        [Header("Object settings")]
        public PrimitiveType primitiveType;
        public Material material;

        [Space]
        [Header("Band settings")]
        [Range(0, 7)]
        public int bandStart;
        [Range(0, 7)]
        public int bandEnd;
        // public bool reverse; TODO

        [Space]
        [Header("Spawn settings")]
        public bool mirror;
        public float xDistance;
        public Vector3 startScale;
        [Space]
        public SpawnType spawnType;
        public float arcSpawnDistance;
        [Range(0, 30)]
        public float arcSpawnDegrees;



        [Space]
        [Header("Scaler")]
        public bool useBandScaler;
        public BandScalerValues bandScalerValues;

        [Space]
        [Header("Colorizer")]
        public bool useBandColorizer;
        public BandColorizerValues bandColorizerValues;


        private GameObject[] objs;
        #endregion

        #region Monobehavior
        private void Start()
        {
            Spawn();

            if (spawnType == SpawnType.Arc)
                LineToArc();
        }
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                LineToArc();
            }
        }
        #endregion

        #region Methods
        // On-call
        public void Spawn()
        {
            // Debug & initialization
            if (bandStart > bandEnd)
            {
                Debug.LogWarning("Start index cannot be larger than end index.");
                return;
            }
            ResetObjects();

            // Creating objects
            int count = 0;
            int cloneCount = 0;
            for (int i = bandStart; i <= bandEnd; i++)
            {
                GameObject go = CreateObject(count);

                if (useBandScaler)
                    InitializeBandScaler(go, i);
                if (useBandColorizer)
                    InitializeBandColorizer(go, i);

                cloneCount = HandleClone(count, cloneCount, go);
                count++;
            }
        }

        // Helpers
        private void ResetObjects()
        {
            if (objs != null && objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null)
                    {
                        Destroy(objs[i]);
                        objs[i] = null;
                    }
                }
            }

            objs = mirror ? new GameObject[(((bandEnd - bandStart) + 1) * 2) - 1] : new GameObject[(bandEnd - bandStart) + 1];
        }
        private void InitializeBandScaler(GameObject g, int band)
        {
            BandScaler bandScaler = g.AddComponent<BandScaler>();
            bandScaler.band8 = band;
            bandScaler.useBuffer = bandScalerValues.useBuffer;
            bandScaler.range = bandScalerValues.range;
            bandScaler.shouldScaleX = bandScalerValues.shouldScaleX;
            bandScaler.shouldScaleY = bandScalerValues.shouldScaleY;
            bandScaler.shouldScaleZ = bandScalerValues.shouldScaleZ;
        }
        private void InitializeBandColorizer(GameObject g, int band)
        {
            BandColorizer bandColorizer = g.AddComponent<BandColorizer>();
            bandColorizer.band = band;
            bandColorizer.useBuffer = bandColorizerValues.useBuffer;
            bandColorizer.lerpTo = bandColorizerValues.lerpTo;
        }

        private GameObject CreateObject(int count)
        {
            GameObject go = GameObject.CreatePrimitive(primitiveType);
            go.transform.SetParent(gameObject.transform);
            go.GetComponent<Renderer>().material = material;
            go.transform.localPosition = new Vector3(xDistance * count, 0, 0);

            // is this backwards? TODO
            go.transform.localScale = startScale;
            return go;
        }
        private int HandleClone(int count, int cloneCount, GameObject go)
        {
            objs[count + cloneCount] = go;
            if (mirror && count != 0)
            {
                GameObject clone = Instantiate(go, gameObject.transform);
                clone.transform.localPosition = new Vector3(-clone.transform.localPosition.x, clone.transform.localPosition.y, clone.transform.localPosition.z);
                cloneCount++;
                objs[count + cloneCount] = clone;
            }

            return cloneCount;
        }
        private void LineToArc()
        {
            for (int i = 0; i < objs.Length; i++)
            {
                // i == 0 -> 0 -> 0*
                // i == 1 -> 1 -> 30
                // i == 2 -> 1 -> -30*
                // i == 3 -> 2 -> 60
                // i == 4 -> 2 -> -60*

                // Vector3 distance = Vector3.forward * arcSpawnDistance;

                // Vector forward from gameObject arcSpawnDistance units away
                Vector3 forward = transform.localRotation * Vector3.forward;
                forward *= arcSpawnDistance;

                // multiply by arcSpawnDegrees for # of (?30s)
                int someNum = (i + 1) / 2;

                // Left side?
                if (i % 2 == 0)
                    someNum *= -1;

                // degrees to rotate by
                float rotateBy = arcSpawnDegrees * someNum;

                // get position
                objs[i].transform.localPosition = Quaternion.Euler(0, rotateBy, 0) * forward;

                // get rotation
                // rotate the object away from the spawner
                //objs[i].transform.localRotation.SetLookRotation(objs[i].transform.localPosition);
                objs[i].transform.LookAt(gameObject.transform.localPosition);
            }
        }
        #endregion
    }
}