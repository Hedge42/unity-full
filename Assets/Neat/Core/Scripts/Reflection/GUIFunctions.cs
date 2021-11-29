using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public static partial class GUIFunctions 
    {
        public static void ToGUIObject(this Object o)
        {

        }
        public static GUIStyle FontStyle(this GUIStyle style, FontStyle s)
        {
            GUIStyle g = new GUIStyle(style);
            g.fontStyle = s;
            return g;
        }
        public static GUIStyle Bold(this GUIStyle style)
        {
            return style.FontStyle(UnityEngine.FontStyle.Bold);
        }
        public static GUIStyle FontSize(this GUIStyle style, int fontSize)
        {
            GUIStyle g = new GUIStyle(style);
            g.fontSize = fontSize;
            return g;
        }
        public static GUIStyle TextColor(this GUIStyle style, Color c)
        {
            GUIStyle g = new GUIStyle(style);
            g.normal.textColor = c;
            return g;
        }
        public static GUIStyle Rich(this GUIStyle style)
        {
            GUIStyle s = new GUIStyle(style);
            s.richText = true;
            return s;
        }
        public static GUIContent Tooltip(this string s, string tooltip)
        {
            return new GUIContent(s, tooltip);
        }
    }
}
