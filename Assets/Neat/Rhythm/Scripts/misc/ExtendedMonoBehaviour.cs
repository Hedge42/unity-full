﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Extensions
{
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        public T CacheComponent<T>(ref T _data)
        {
            if (_data == null)
                _data = GetComponent<T>();
            return _data;
        }
        public T Cache<T>(Func<T> constructor, ref T _data)
        {
            if (_data == null)
                _data = constructor();
            return _data;
        }
    }
    public static class GameExtensions
    {
        public static T2 CacheGetComponent<T1, T2>(this T1 _this, ref T2 _data) where T1 : MonoBehaviour
        {
            // ADD component??
            if (ReferenceEquals(_data, null)) // source?
                                              // if (_data == null)
                _data = _this.GetComponent<T2>();
            return _data;
        }
        public static T2 CacheAddComponent<T1, T2>(this T1 _this, ref T2 _data) where T1 : MonoBehaviour
        {
            // ADD component??
            if (ReferenceEquals(_data, null)) // source?
                                              // if (_data == null)

                _data = _this.gameObject.GetOrAddComponent<T2>();
            return _data;
        }
        public static TOut CacheObject<TSender, TOut>(this TSender _this, ref TOut _data) where TOut : ScriptableObject
        {
            if (_data == null)
            //if (ReferenceEquals(_data, null)) // source?
                                              // if (_data == null)
                _data = ScriptableObject.CreateInstance<TOut>();
            return _data;
        }
        public static U Cache<T, U>(this T _t, ref U _u, Func<U> construct)
        {
            if (ReferenceEquals(_u, null)) // _u == null)
                _u = construct();
            return _u;
        }
    }
}
