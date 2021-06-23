using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neet.Data
{
    public static class UserDataExtensions
    {
        public static void SetData(this object o, object value)
        {
            if (UserData.entries == null)
                UserData.entries = new Dictionary<object, UserData>();

            if (!UserData.entries.ContainsKey(o))
                UserData.entries[o] = new UserData();

            UserData.entries[o].SetData(value);
        }
        public static void SetData(this object o, object key, object value)
        {
            if (UserData.entries == null)
                UserData.entries = new Dictionary<object, UserData>();

            if (!UserData.entries.ContainsKey(o))
                UserData.entries[o] = new UserData();

            UserData.entries[o].SetData(key, value);
        }
        public static T GetData<T>(this object o)
        {
            if (o.HasData())
            {
                return UserData.entries[o].GetData<T>();
            }
            else
            {
                return default(T);
            }
        }
        public static T GetData<T>(this object o, object key)
        {
            if (o.HasData())
            {
                return UserData.entries[o].GetData<T>(key);
            }
            else
            {
                return default(T);
            }
        }

        public static bool HasData(this object o)
        {
            return o != null 
                && UserData.entries != null 
                && UserData.entries.ContainsKey(o);
        }
    }

    public class UserData
    {
        public static Dictionary<object, UserData> entries;
        public static Dictionary<object, object> staticData;
        public Dictionary<object, object> keyDict;
        public Dictionary<System.Type, object> typeDict;

        public static void SetStaticData(object key, object value)
        {
            if (staticData == null)
                staticData = new Dictionary<object, object>();

            staticData[key] = value;
        }
        public static T GetStaticData<T>(object key)
        {
            if (HasStaticData(key))
                return (T)staticData[key];
            else
                return default(T);

        }
        public static bool HasStaticData(object key)
        {
            return staticData != null && staticData.ContainsKey(key);
        }

        public void SetData(object value)
        {
            if (typeDict == null)
                typeDict = new Dictionary<System.Type, object>();
            typeDict[value.GetType()] = value;
        }
        public void SetData(object key, object value)
        {
            if (keyDict == null)
                keyDict = new Dictionary<object, object>();
            keyDict[key] = value;
        }

        public T GetData<T>(object key)
        {
            if (keyDict != null && keyDict.ContainsKey(key) && keyDict[key].GetType() == typeof(T))
            {
                return (T)keyDict[key];
            }
            else
                return default(T);
        }
        public T GetData<T>()
        {
            if (typeDict != null && typeDict.ContainsKey(typeof(T)))
            {
                return (T)typeDict[typeof(T)];
            }
            else
                return default(T);
        }
        public string[] ReadData()
        {
            object[] keys = keyDict.Keys.ToArray();
            object[] values = keyDict.Values.ToArray();

            string[] s = new string[keyDict.Count];
            for (int i = 0; i < s.Length; i++)
                s[i] = keys[i].ToString() + " -> " + values[i].ToString();
            return s;
        }
    }
}