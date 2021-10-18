using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Neat.Extensions
{
    public static class UserDataExtensions
    {
        /// <summary>
        /// Stores value in a static dictionary using self as key
        /// </summary>
        public static void SetData(this object o, object value)
        {
            UserData.staticData[o] = value;
        }

        /// <summary>
        /// Stores value in a static dictionary using given key
        /// </summary>
        public static void SetData(this object o, object key, object value)
        {
            if (!UserData.entries.ContainsKey(o))
                UserData.entries.Add(o, new UserData());

            UserData.entries[o].SetUserData(key, value);
        }

        /// <summary>
        /// Removes value set by SetData with self as key
        /// </summary>
        public static void RemoveData(this object o)
        {
            if (UserData.entries.ContainsKey(o))
                UserData.entries[o].RemoveUserData(o);
        }

        /// <summary>
        /// Removes value set by SetData with given key
        /// </summary>
        public static void RemoveData(this object o, object key)
        {
            if (UserData.entries.ContainsKey(o))
                UserData.entries[o].RemoveUserData(key);
        }

        /// <summary>
        /// Retrieves value set by SetData with self as key
        /// </summary>
        public static T GetData<T>(this object o)
        {
            try
            {
                return (T)UserData.staticData[o];
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Retrives value set by SetData with given key.<br/>
        /// Returns default on failure
        /// </summary>
        public static T GetData<T>(this object o, object key)
        {
            try
            {
                return UserData.entries[o].GetUserValue<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        private static bool HasData(this object key)
        {
            return key != null 
                && UserData.entries != null 
                && UserData.entries.ContainsKey(key);
        }
    }

    public class UserData
    {
        private static Dictionary<object, UserData> _entries;
        public static Dictionary<object, UserData> entries
        {
            get
            {
                if (_entries == null)
                    _entries = new Dictionary<object, UserData>();
                return _entries;
            }
        }
        private static Dictionary<object, object> _staticData;
        public static Dictionary<object, object> staticData
        {
            get
            {
                if (_staticData == null)
                    _staticData = new Dictionary<object, object>();
                return _staticData;
            }
        }

        private Dictionary<object, object> _keyDict;
        public Dictionary<object, object> keyDict
        {
            get
            {
                if (_keyDict == null)
                    _keyDict = new Dictionary<object, object>();
                return _keyDict;
            }
        }

        public static void SetStaticData(object key, object value)
        {
            staticData[key] = value;
        }
        public static T GetStaticData<T>(object key)
        {
            if (HasStaticData(key))
                return (T)staticData[key];
            else
                return default(T);

        }
        public static void RemoveStaticData(object key)
        {
            if (HasStaticData(key))
                staticData.Remove(key);
        }
        public static bool HasStaticData(object key)
        {
            return staticData.ContainsKey(key);
        }

        public void SetUserData(object key, object value)
        {
            keyDict[key] = value;
        }
        public void RemoveUserData(object key)
        {
            keyDict.Remove(key);
        }
        public T GetUserValue<T>(object key)
        {
            if (keyDict.ContainsKey(key) && keyDict[key].GetType() == typeof(T))
                return (T)keyDict[key];
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