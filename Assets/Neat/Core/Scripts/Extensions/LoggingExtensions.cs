using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Neat.Tools
{
    public static class LoggingExtensions
    {

        public static void Log(this Object sender, object value, params string[] lines)
        {
            var sb = new StringBuilder(value.ToString());
            foreach (var line in lines)
                sb.AppendLine(line);
            Debug.Log(value, sender);
        }

        public static void Log(this object value, params string[] lines)
        {
            var sb = new StringBuilder(value.ToString());
            foreach (var line in lines)
                sb.AppendLine(line);
            Debug.Log(sb.ToString());
        }

        public static void Log(this object value)
        {
            if (value == null)
                Debug.Log("null");

            Debug.Log(value.ToString());
        }
    }
}
