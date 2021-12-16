using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using TMPro;

namespace Neat.Experimental
{
    // test class
    public class InstancerMono : MonoBehaviour
    {
        // public static InstancerMono instance = GameObject.FindObjectOfType<InstancerMono>();

        public static Instancer<int> instances = new Instancer<int>();
        public static int main => instances.Main();

        int next = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                instances.Add(next++);
                print(next);
            }
        }
    }
}
