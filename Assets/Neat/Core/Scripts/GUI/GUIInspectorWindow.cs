using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Neat.Tools
{
    public class GUIInspectorWindow : MonoBehaviour
    {
        public GUIInspector inspector;
        public Object target;
        public Rect rect;

        private void OnGUI()
        {
            GUIInspector.from(target);
            GUI.Window(GetInstanceID(), rect, DrawWindow, target.name);
        }
        public void DrawWindow(int id)
        {

        }
    }
}
