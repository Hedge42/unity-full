using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Tools
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
}
