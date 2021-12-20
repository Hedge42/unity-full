using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Neat.Experimental
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                if (ReferenceEquals(_instance, null))
                    _instance = GameObject.FindObjectOfType<T>();
                return _instance;
            }
            set
            {
                if (ReferenceEquals(_instance, null))
                    _instance = value;
                else
                    Destroy(value.gameObject);
            }
        }

        private void Awake()
        {
            // this would reference parent class
            instance = gameObject.GetComponent<T>();
        }
    }
}
