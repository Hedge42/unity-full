using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public class GUIWindow2 : MonoBehaviour
    {
        public Object target;

        private void OnGUI()
        {
            if (target != null)
            {
                // GUI.Window(id, Rect, DrawWindow, title);
            }
        }
    }
}
