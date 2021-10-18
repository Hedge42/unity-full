using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Music
{
    public static class Destroyer
    {
        public static void Destroy(GameObject o)
        {
            if (Application.isPlaying)
                GameObject.Destroy(o);
            else
                GameObject.DestroyImmediate(o);

        }
        public static void Destroy(Component c)
        {
            if (c == null)
            {
                Debug.Log("Stop, it's already dead!");
                return;
            }

            if (Application.isPlaying)
                GameObject.Destroy(c);
            else
                GameObject.DestroyImmediate(c);
        }
        public static void DestroyChildren<T>(Transform t)
        {
            var children = new List<Transform>();
            foreach (Transform child in t)
                if (child.GetComponent<T>() != null)
                    children.Add(child);

            foreach (Transform child in children)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(child.gameObject);
                else
                    GameObject.DestroyImmediate(child.gameObject);
            }
        }
    }
}
