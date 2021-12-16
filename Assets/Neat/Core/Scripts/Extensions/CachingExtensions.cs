using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
{
    public static partial class Functions
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

        private static Hashtable set;
        public static Hashtable table
        {
            get
            {
                if (set == null)
                    set = new Hashtable();
                return set;
            }
        }
        public static TOut CacheObject<TSender, TOut>(TSender _this) where TOut : ScriptableObject
        {
            throw new NotImplementedException();
            TOut result = table[_this] as TOut; // what happens
            if (result != null)
            {
                return result;
            }
            else
            {
                table.Add(_this, result = ScriptableObject.CreateInstance<TOut>());
            }
            return result;
        }
        public static U Cache<T, U>(this T _t, ref U _u, Func<U> construct)
        {
            if (ReferenceEquals(_u, null)) // _u == null)
                _u = construct();
            return _u;
        }
    }
}
