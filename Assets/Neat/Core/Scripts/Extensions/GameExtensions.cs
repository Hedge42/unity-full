using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neat.Tools
{
    public static partial class Functions
    {
        public static T GetOrAddComponent<T>(this GameObject o)
        {
            if (o.GetComponent<T>() == null)
                o.AddComponent(typeof(T));
            return o.GetComponent<T>();
        }
        public static T GetOrAddComponent<T>(this Component o)
        {
            if (o.GetComponent<T>() == null)
                o.gameObject.AddComponent(typeof(T));
            return o.GetComponent<T>();
        }
        public static bool HasComponent<T>(this GameObject o)
        {
            return o.GetComponent<T>() != null;
        }
        public static bool HasComponent<T>(this Component o)
        {
            return o.GetComponent<T>() != null;
        }

        public static TimeSpan TimeBetween(DateTime a, DateTime b)
        {
            return new TimeSpan(a.Ticks - b.Ticks);
        }

        public static void SetColor(this GameObject g, Color c)
        {
            try
            {
                if (g.HasComponent<Renderer>())
                {
                    MaterialPropertyBlock block = new MaterialPropertyBlock();
                    block.SetColor("_Color", c);
                    g.GetComponent<Renderer>()?.SetPropertyBlock(block);
                }
            }
            catch
            {
                // Debug.LogWarning("Error setting color.");
            }
        }
        public static Material GetMaterial(this GameObject g)
        {
            return g.GetComponent<Renderer>().material;
        }
        public static Renderer GetRenderer(this GameObject g)
        {
            return g.GetComponent<Renderer>();
        }
        public static Color GetColor(this GameObject g)
        {
            try
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                // g.GetComponent<Renderer>().GetPropertyBlock(block);
                return block.GetColor("_Color");
                // return g.GetComponent<Renderer>().material.color;
            }
            catch
            {
                return Color.white;
            }
        }
        public static Color LerpPlus(Color start, Color end, float t)
        {
            float r = Mathf.Lerp(start.r, end.r, t);
            float g = Mathf.Lerp(start.g, end.g, t);
            float b = Mathf.Lerp(start.b, end.b, t);
            float a = Mathf.Lerp(start.a, end.a, t);
            return new Color(r, g, b, a);
        }
        public static bool Cast<T1, T2>(object o1, object o2, out T1 t1, out T2 t2)
        {
            bool success = true;
            if (o1 != null && o1.GetType() == typeof(T1))
                t1 = (T1)o1;
            else
            {
                t1 = default(T1);
                success = false;
            }

            if (o2 != null && o2.GetType() == typeof(T2))
                t2 = (T2)o2;
            else
            {
                t2 = default(T2);
                success = false;
            }

            return success;
        }
        public static bool Cast<T>(this object o, out T t)
        {
            if (o != null && o.GetType() == typeof(T))
            {
                t = (T)o;
                return true;
            }
            else
            {
                t = default(T);
                return false;
            }
        }

        public static float Abs(this float f)
        {
            if (f < 0)
                return f * -1;
            else
                return f;
        }
        public static T Last<T>(this T[] arr)
        {
            if (arr.Length > 0)
                return arr[arr.Length - 1];
            else
                return default(T);
        }
        public static T Last<T>(this List<T> arr)
        {
            if (arr.Count > 0)
                return arr[arr.Count - 1];
            else
                return default(T);
        }
        public static T Last<T>(this List<T> arr, out int index)
        {
            if (arr.Count > 0)
            {
                index = arr.Count - 1;
                return arr[arr.Count - 1];
            }
            else
            {
                index = -1;
                return default(T);
            }
        }
        public static int LastIndex<T>(this List<T> list)
        {
            return list.Count - 1;
        }
        public static float DistanceTo(this Vector2 a, Vector2 b)
        {
            return Vector2.Distance(a, b);
        }
        public static Vector2 SetLength(this Vector2 v, float dist)
        {
            return v.normalized * dist;
        }
        public static Vector2 Clamp(this Vector2 v, float minLength, float maxLength)
        {
            if (v.magnitude > maxLength)
                return v.SetLength(maxLength);
            else if (v.magnitude < minLength)
                return v.SetLength(minLength);
            else
                return v;
        }
        public static float Min(this Vector2 v)
        {
            return v.x;
        }
        public static float Max(this Vector2 v)
        {
            return v.x;
        }
        public static bool GetKeyDown(this IEnumerable<KeyCode> k)
        {
            foreach (KeyCode _k in k)
                if (UnityEngine.Input.GetKeyDown(_k))
                    return true;

            return false;
        }
        public static bool GetKey(this IEnumerable<KeyCode> k)
        {
            foreach (KeyCode _k in k)
                if (UnityEngine.Input.GetKey(_k))
                    return true;

            return false;
        }

        public static bool GetKeyDown(this KeyCode k)
        {
            return UnityEngine.Input.GetKeyDown(k);
        }
        public static Dictionary<T1, T2> SwapValues<T1, T2>(this Dictionary<T1, T2> d, int a, int b)
        {
            if (a < 0 || b < 0 || a >= d.Count || b >= d.Count)
            {
                Debug.LogError("Value out of range");
                return d;
            }

            // lol
            var newd = new Dictionary<T1, T2>();
            for (int i = 0; i < d.Count; i++)
            {
                T1 key = d.Keys.ElementAt(i);
                if (i == a)
                    key = d.Keys.ElementAt(b);
                else if (i == b)
                    key = d.Keys.ElementAt(a);

                newd.Add(key, d[key]);
            }

            return newd;
        }

        public static Dictionary<T1, T2> ChangeKey<T1, T2>(this Dictionary<T1, T2> d, T1 oldKey, T1 newKey)
        {
            // lol
            var newd = new Dictionary<T1, T2>();
            for (int i = 0; i < d.Count; i++)
            {
                var key = d.Keys.ElementAt(i);
                if (key.Equals(oldKey))
                    key = newKey;

                newd.Add(key, d[oldKey]);
            }

            return newd;
        }
        public static Transform[] GetChildren(this Transform t)
        {
            var temp = new List<Transform>();
            for (int i = 0; i < t.childCount; i++)
                temp.Add(t.GetChild(i));

            return temp.ToArray();

        }
        public static void DestroyChildren(this Transform t, int start = 0)
        {
            var children = t.GetChildren();
            GameObject[] tempArr = new GameObject[children.Length - start];
            for (int i = start; i < children.Length; i++)
                tempArr[i - start] = children[i].gameObject;

            foreach (GameObject g in tempArr)
                if (Application.isPlaying)
                    GameObject.Destroy(g);
                else
                    GameObject.DestroyImmediate(g);
        }

        public static bool isLike(this string s, string other)
        {
            var one = s.Trim().ToLower();
            var two = other.Trim().ToLower();
            Debug.Log("lol");

            return one.Equals(two);
        }

        public static string Colorize(this string s, string hex)
        {
            return "<color=" + hex + ">" + s + "</color>";
        }
        public static string Colorize(this string s, Color c)
        {
            //string hex = "#" + c.r.ToString("X2") + c.g.ToString("X2") + c.b.ToString("X2");
            byte r = (byte)(c.r * 255);
            byte g = (byte)(c.g * 255);
            byte b = (byte)(c.b * 255);
            string hex = "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
            return s.Colorize(hex);
        }
        public static string Resize(this string s, int points)
        {
            // doesn't work
            return "<size =" + points + ">" + s + "</size>";
        }
        public static string Italic(this string s)
        {
            return "<i>" + s + "</i>";
        }
        public static string Bold(this string s)
        {
            return "<b>" + s + "</b>";
        }
        public static string Underline(this string s)
        {
            return "<u>" + s + "</u>";
        }
    }
}